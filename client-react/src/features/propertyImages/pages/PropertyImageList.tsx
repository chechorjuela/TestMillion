import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { PropertyImage, PropertyImageFilters } from '../../../types';
import { fetchPropertyImages } from '../propertyImageSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionButtons } from '../../../components/molecules/ActionButtons';
import { usePagination } from '../../../hooks/usePagination';
import { useModal } from '../../../hooks/useModal';
import { propertyImageService } from '../propertyImageService';
import { PAGE_SIZES, TABLE_DEFAULTS } from '../../../config/constants';
import { BsPlus } from 'react-icons/bs';
import { ActionModal } from '../../../components/organisms/ActionModal';

const filterFields = [
  { name: 'propertyId', label: 'Property ID', type: 'text' as const, placeholder: 'Search by property ID' },
  { 
    name: 'enabled', 
    label: 'Status', 
    type: 'select' as const, 
    options: [
      { value: '', label: 'All' },
      { value: 'true', label: 'Enabled' },
      { value: 'false', label: 'Disabled' }
    ]
  },
] as const;

const PropertyImageList = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { propertyImages, loading } = useSelector((state: RootState) => state.propertyImages);

  const {
    pageSize,
    currentPage,
    currentFilters,
    handlePageChange,
    handleItemsPerPageChange,
    handleFilterChange,
  } = usePagination<PropertyImage, PropertyImageFilters>({
    fetchData: (filters) => dispatch(fetchPropertyImages(filters)),
    defaultPageSize: TABLE_DEFAULTS.PAGE_SIZE,
  });

  const modal = useModal<PropertyImage>({
    onSuccess: () => {
      modal.handleClose();
      dispatch(fetchPropertyImages({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
    },
  });

  const handleAdd = () => {
    modal.handleOpen('create');
  };

  const handleEdit = (propertyImage: PropertyImage) => {
    modal.handleOpen('edit', propertyImage);
  };

  const handleDelete = (propertyImage: PropertyImage) => {
    modal.handleOpen('delete', propertyImage);
  };

  const handleView = (propertyImage: PropertyImage) => {
    window.open(propertyImage.File, '_blank');
  };

  const columns = [
    { 
      header: 'Image', 
      accessor: (image: PropertyImage) => (
        <img 
          src={image.File} 
          alt={`Property ${image.IdProperty}`} 
          className="w-20 h-20 object-cover rounded"
        />
      )
    },
    { 
      header: 'Property', 
      accessor: (image: PropertyImage) => image.Property?.Name || 'No Property',
    },
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
        <ActionButtons
          onView={() => handleView(propertyImage)}
          onEdit={() => handleEdit(propertyImage)}
          onDelete={() => handleDelete(propertyImage)}
          className="justify-end"
        />
      ),
      className: 'w-40'
    }
  ];

  const actions = (
    <Button
      onClick={handleAdd}
      className="inline-flex items-center gap-1"
    >
      <BsPlus className="w-5 h-5" />
      <span className="sr-only sm:not-sr-only">Add Property Image</span>
    </Button>
  );

  return (
    <>
      <PageLayout
        title="Property Images"
        subtitle="Manage property images"
        actions={actions}
        filters={
          <FilterForm<PropertyImageFilters>
            fields={filterFields}
            onSubmit={handleFilterChange}
            loading={loading}
          />
        }
      >
        <DataTable<PropertyImage>
          columns={columns}
          data={propertyImages}
          loading={loading}
          keyExtractor={(item) => item.Id}
          onRowClick={handleView}
          pagination={{
            currentPage,
            pageSize,
            onPageChange: handlePageChange,
            onItemsPerPageChange: handleItemsPerPageChange,
            pageSizeOptions: PAGE_SIZES,
          }}
        />
      </PageLayout>

      <ActionModal
        isOpen={modal.isOpen}
        onClose={modal.handleClose}
        entityType="propertyImage"
        actionType={modal.mode}
        data={modal.data}
        isLoading={modal.isLoading}
        onSubmit={async (data) => {
          await modal.handleAction(async () => {
            switch (modal.mode) {
              case 'create':
                await propertyImageService.createPropertyImage(data);
                break;
              case 'edit':
                if (modal.data) {
                  await propertyImageService.updatePropertyImage(modal.data.Id, data);
                }
                break;
              case 'delete':
                if (modal.data) {
                  await propertyImageService.deletePropertyImage(modal.data.Id);
                }
                break;
            }
          });
        }}
      />
    </>
  );
};

export default PropertyImageList;