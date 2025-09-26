import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { Property, PropertyFilters } from '../../../types';
import { fetchProperties } from '../propertySlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionButtons } from '../../../components/molecules/ActionButtons';
import { usePagination } from '../../../hooks/usePagination';
import { useModal } from '../../../hooks/useModal';
import { propertyService } from '../propertyService';
import { PAGE_SIZES, TABLE_DEFAULTS } from '../../../config/constants';
import { BsPlus } from 'react-icons/bs';
import { ActionModal } from '../../../components/organisms/ActionModal';

const filterFields = [
  { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
  { name: 'address', label: 'Address', type: 'text' as const, placeholder: 'Search by address' },
  { name: 'minPrice', label: 'Min Price', type: 'number' as const, placeholder: 'Min price' },
  { name: 'maxPrice', label: 'Max Price', type: 'number' as const, placeholder: 'Max price' },
] as const;

const PropertyList = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { properties, loading } = useSelector((state: RootState) => state.properties);

  const {
    pageSize,
    currentPage,
    currentFilters,
    handlePageChange,
    handleItemsPerPageChange,
    handleFilterChange,
  } = usePagination<Property, PropertyFilters>({
    fetchData: (filters) => dispatch(fetchProperties(filters)),
    defaultPageSize: TABLE_DEFAULTS.PAGE_SIZE,
  });

  const modal = useModal<Property>({
    onSuccess: () => {
      modal.handleClose();
      dispatch(fetchProperties({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
    },
  });

  const handleAdd = () => {
    modal.handleOpen('create');
  };

  const handleEdit = (property: Property) => {
    modal.handleOpen('edit', property);
  };

  const handleDelete = (property: Property) => {
    modal.handleOpen('delete', property);
  };

  const handleView = (property: Property) => {
    navigate(`/properties/${property.Id}`);
  };

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
        <ActionButtons
          onView={() => handleView(property)}
          onEdit={() => handleEdit(property)}
          onDelete={() => handleDelete(property)}
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
      <span className="sr-only sm:not-sr-only">Add Property</span>
    </Button>
  );

  return (
    <>
      <PageLayout
        title="Properties"
        subtitle="Manage your real estate properties"
        actions={actions}
        filters={
          <FilterForm<PropertyFilters>
            fields={filterFields}
            onSubmit={handleFilterChange}
            loading={loading}
          />
        }
      >
        <DataTable<Property>
          columns={columns}
          data={properties}
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
        entityType="property"
        actionType={modal.mode}
        data={modal.data}
        isLoading={modal.isLoading}
        onSubmit={async (data) => {
          await modal.handleAction(async () => {
            switch (modal.mode) {
              case 'create':
                await propertyService.createProperty(data);
                break;
              case 'edit':
                if (modal.data) {
                  await propertyService.updateProperty(modal.data.Id, data);
                }
                break;
              case 'delete':
                if (modal.data) {
                  await propertyService.deleteProperty(modal.data.Id);
                }
                break;
            }
          });
        }}
      />
    </>
  );
};

export default PropertyList;