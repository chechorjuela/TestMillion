import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type {Property, PropertyFilters} from '../../../types';
import { fetchProperties } from '../propertySlice';
import { propertyService } from '../propertyService';
import type {RootState, AppDispatch} from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionModal } from '../../../components/organisms/ActionModal';
import { BsEye, BsPencil, BsTrash } from 'react-icons/bs';
import toast from 'react-hot-toast';

const PropertyList = () => {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [propertyToDelete, setPropertyToDelete] = useState<Property | null>(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedProperty, setSelectedProperty] = useState<Property | null>(null);
  const [pageSize, setPageSize] = useState(10);
  const [currentPage, setCurrentPage] = useState(1);
  const [currentFilters, setCurrentFilters] = useState<PropertyFilters>({});
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { properties, loading, error, metadata } = useSelector((state: RootState) => state.properties);

  const handlePageChange = async (page: number) => {
    try {
      if (page > 0 && (!metadata || page <= metadata.TotalPages) && page !== currentPage) {
        setCurrentPage(page);
        // Primero establecemos loading para mostrar el estado de carga
        dispatch(fetchProperties({ ...currentFilters, PageNumber: page, PageSize: pageSize })).unwrap()
          .catch(error => {
            // Manejar el error si es necesario
            console.error('Error fetching properties:', error);
          });
      }
    } catch (error) {
      console.error('Error in handlePageChange:', error);
    }
  };

  const handleEditProperty = (property: Property) => {
    setSelectedProperty(property);
    setIsEditModalOpen(true);
  };

  const handleUpdateProperty = async (data: Partial<Property>) => {
    if (selectedProperty) {
      try {
        const updateData = {
          ...data,
          idOwner: selectedProperty.IdOwner // Mantener el IdOwner existente
        };

        await propertyService.updateProperty(selectedProperty.Id, updateData);
        
        // Recargar la lista después de actualizar
        await dispatch(fetchProperties({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setSelectedProperty(null);
        setIsEditModalOpen(false);
      } catch (error) {
        console.error('Error updating property:', error);
      }
    }
  };

  const handleDeleteProperty = (property: Property) => {
    setPropertyToDelete(property);
    setIsDeleteModalOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (propertyToDelete) {
      try {
        await propertyService.deleteProperty(propertyToDelete.Id);

        // Recargar la lista después de eliminar
        await dispatch(fetchProperties({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setIsDeleteModalOpen(false);
        setPropertyToDelete(null);
      } catch (error) {
        console.error('Error deleting property:', error);
      }
    }
  };

  const handleItemsPerPageChange = async (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
    await dispatch(fetchProperties({
      ...currentFilters,
      PageNumber: 1,
      PageSize: newPageSize
    }));
  };

  useEffect(() => {
    dispatch(fetchProperties({
      ...currentFilters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  }, [dispatch, pageSize, currentFilters]);

  const handleFilter = (filters: PropertyFilters) => {
    setCurrentFilters(filters);
    setCurrentPage(1);
    dispatch(fetchProperties({
      ...filters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  };

  const filterFields = [
    { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
    { name: 'address', label: 'Address', type: 'text' as const, placeholder: 'Search by address' },
    { name: 'minPrice', label: 'Min Price', type: 'number' as const, placeholder: 'Min price' },
    { name: 'maxPrice', label: 'Max Price', type: 'number' as const, placeholder: 'Max price' },
  ];

  const columns = [
    { header: 'Name', accessor: 'Name' as const },
    { header: 'Address', accessor: 'Address' as const },
    { 
      header: 'Price', 
      accessor: (property: Property) => `$${property.Price.toLocaleString()}`,
      className: 'text-right'
    },
    {
      header: 'Actions',
      accessor: (property: Property) => (
        <div className="flex justify-end gap-2">
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/properties/${property.Id}`);
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
              handleEditProperty(property);
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
              handleDeleteProperty(property);
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
      Add Property
    </Button>
  );

  return (
    <>
      <div>
        {/* Toast container */}
        <div className="fixed top-4 right-4 z-50" />
      </div>
      <PageLayout
        title="Properties"
        subtitle="Manage your real estate properties"
        actions={actions}
        filters={
          <FilterForm<PropertyFilters>
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
          data={properties}
          loading={loading}
          keyExtractor={(item) => item.Id}
          onRowClick={(item) => navigate(`/properties/${item.Id}`)}
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
        entityType="property"
        actionType="create"
        onSubmit={async (data) => {
          try {
            await propertyService.createProperty(data);
            dispatch(fetchProperties({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
            setIsAddModalOpen(false);
            toast.success('Property created successfully');
          } catch (error) {
            console.error('Error creating property:', error);
            toast.error('Failed to create property');
          }
        }}
      />
      <ActionModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        entityType="property"
        actionType="edit"
        data={selectedProperty}
        onSubmit={handleUpdateProperty}
      />
      <ActionModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        entityType="property"
        actionType="delete"
        data={propertyToDelete}
        onSubmit={handleConfirmDelete}
      />
    </>
  );
};

export default PropertyList;