import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { PropertyImage, PropertyImageFilters } from '../../../types';
import { fetchPropertyImages } from '../propertyImageSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionModal } from '../../../components/organisms/ActionModal';
import { BsEye, BsPencil, BsTrash } from 'react-icons/bs';
import { propertyImageService } from '../propertyImageService';

const PropertyImageList = () => {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [propertyImageToDelete, setPropertyImageToDelete] = useState<PropertyImage | null>(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedPropertyImage, setSelectedPropertyImage] = useState<PropertyImage | null>(null);
  const [pageSize, setPageSize] = useState(10);
  const [currentPage, setCurrentPage] = useState(1);
  const [currentFilters, setCurrentFilters] = useState<PropertyImageFilters>({});
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { propertyImages, loading, error, metadata } = useSelector((state: RootState) => state.propertyImages);

  const handlePageChange = async (page: number) => {
    try {
      if (page > 0 && (!metadata || page <= metadata.TotalPages) && page !== currentPage) {
        setCurrentPage(page);
        dispatch(fetchPropertyImages({ ...currentFilters, PageNumber: page, PageSize: pageSize })).unwrap()
          .catch(error => {
            console.error('Error fetching property images:', error);
          });
      }
    } catch (error) {
      console.error('Error in handlePageChange:', error);
    }
  };

  const handleEditPropertyImage = (propertyImage: PropertyImage) => {
    setSelectedPropertyImage(propertyImage);
    setIsEditModalOpen(true);
  };

  const handleUpdatePropertyImage = async (data: Partial<PropertyImage>) => {
    if (selectedPropertyImage) {
      try {
        await propertyImageService.updatePropertyImage(selectedPropertyImage.Id, data);
        
        // Recargar la lista después de actualizar
        await dispatch(fetchPropertyImages({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setSelectedPropertyImage(null);
        setIsEditModalOpen(false);
      } catch (error) {
        console.error('Error updating property image:', error);
      }
    }
  };

  const handleDeletePropertyImage = (propertyImage: PropertyImage) => {
    setPropertyImageToDelete(propertyImage);
    setIsDeleteModalOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (propertyImageToDelete) {
      try {
        await propertyImageService.deletePropertyImage(propertyImageToDelete.Id);

        // Recargar la lista después de eliminar
        await dispatch(fetchPropertyImages({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setIsDeleteModalOpen(false);
        setPropertyImageToDelete(null);
      } catch (error) {
        console.error('Error deleting property image:', error);
      }
    }
  };

  const handleItemsPerPageChange = async (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
    await dispatch(fetchPropertyImages({
      ...currentFilters,
      PageNumber: 1,
      PageSize: newPageSize
    }));
  };

  useEffect(() => {
    dispatch(fetchPropertyImages({
      ...currentFilters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  }, [dispatch, pageSize, currentFilters]);

  const handleFilter = (filters: PropertyImageFilters) => {
    setCurrentFilters(filters);
    setCurrentPage(1);
    dispatch(fetchPropertyImages({
      ...filters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  };

  const filterFields = [
    { name: 'propertyId', label: 'Property ID', type: 'text' as const, placeholder: 'Search by property ID' },
    { name: 'enabled', label: 'Status', type: 'select' as const, options: [
      { value: '', label: 'All' },
      { value: 'true', label: 'Enabled' },
      { value: 'false', label: 'Disabled' }
    ]},
  ];

  const columns = [
    { 
      header: 'Image', 
      accessor: (image: PropertyImage) => (
        <img 
          src={image.FileUrl} 
          alt={`Property ${image.IdProperty}`} 
          className="w-20 h-20 object-cover rounded"
        />
      )
    },
    { header: 'Property ID', accessor: 'IdProperty' as const },
    { header: 'File Name', accessor: 'FileName' as const },
    { 
      header: 'Status',
      accessor: (image: PropertyImage) => (
        <span className={`px-2 py-1 rounded ${image.Enabled ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {image.Enabled ? 'Enabled' : 'Disabled'}
        </span>
      )
    },
    {
      header: 'Actions',
      accessor: (propertyImage: PropertyImage) => (
        <div className="flex justify-end gap-2">
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              window.open(propertyImage.FileUrl, '_blank');
            }}
            className="hover:bg-theme-silver hover:text-theme-white hover:border-theme-silver transition-all duration-200 ease-in-out p-1"
            title="View"
          >
            <BsEye className="w-4 h-4" />
          </Button>
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              handleEditPropertyImage(propertyImage);
            }}
            className="hover:bg-theme-black hover:text-theme-white hover:border-theme-black transition-all duration-200 ease-in-out p-1"
            title="Edit"
          >
            <BsPencil className="w-4 h-4" />
          </Button>
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              handleDeletePropertyImage(propertyImage);
            }}
            className="hover:bg-red-500 hover:text-white hover:border-red-500 transition-all duration-200 ease-in-out p-1"
            title="Delete"
          >
            <BsTrash className="w-4 h-4" />
          </Button>
        </div>
      ),
      className: 'text-right'
    }
  ];

  const actions = (
    <Button
      onClick={() => setIsAddModalOpen(true)}
    >
      Add Property Image
    </Button>
  );

  return (
    <>
      <div>
        {/* Toast container */}
        <div className="fixed top-4 right-4 z-50" />
      </div>
      <PageLayout
        title="Property Images"
        subtitle="Manage property images"
        actions={actions}
        filters={
          <FilterForm<PropertyImageFilters>
            fields={filterFields}
            onSubmit={handleFilter}
            loading={loading}
          />
        }
      >
        {error && (
          <div className="bg-red-50 text-red-600 p-4 rounded-md">
            {error}
          </div>
        )}
        
        <DataTable
          columns={columns}
          data={propertyImages}
          loading={loading}
          keyExtractor={(item) => item.Id}
          pagination={metadata ? {
            currentPage,
            totalPages: metadata.TotalPages,
            onPageChange: handlePageChange,
            itemsPerPage: pageSize,
            onItemsPerPageChange: handleItemsPerPageChange,
            totalItems: metadata.TotalItems || 0
          } : undefined}
        />
      </PageLayout>
      <ActionModal
        isOpen={isAddModalOpen}
        onClose={() => setIsAddModalOpen(false)}
        entityType="propertyImage"
        actionType="create"
        onSubmit={async (data) => {
          try {
            await propertyImageService.createPropertyImage(data);
            dispatch(fetchPropertyImages({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
            setIsAddModalOpen(false);
          } catch (error) {
            console.error('Error creating property image:', error);
          }
        }}
      />
      <ActionModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        entityType="propertyImage"
        actionType="edit"
        data={selectedPropertyImage}
        onSubmit={handleUpdatePropertyImage}
      />
      <ActionModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        entityType="propertyImage"
        actionType="delete"
        data={propertyImageToDelete}
        onSubmit={handleConfirmDelete}
      />
    </>
  );
};

export default PropertyImageList;