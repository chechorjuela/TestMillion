import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { PropertyTrace, PropertyTraceFilters } from '../../../types';
import { fetchPropertyTraces } from '../propertyTraceSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionButtons } from '../../../components/molecules/ActionButtons';
import { usePagination } from '../../../hooks/usePagination';
import { useModal } from '../../../hooks/useModal';
import { propertyTraceService } from '../propertyTraceService';
import { PAGE_SIZES, TABLE_DEFAULTS } from '../../../config/constants';
import { BsPlus } from 'react-icons/bs';
import { ActionModal } from '../../../components/organisms/ActionModal';

const filterFields = [
  { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
  { name: 'minValue', label: 'Min Value', type: 'number' as const, placeholder: 'Min value' },
  { name: 'maxValue', label: 'Max Value', type: 'number' as const, placeholder: 'Max value' },
  { name: 'startDate', label: 'Start Date', type: 'date' as const },
  { name: 'endDate', label: 'End Date', type: 'date' as const },
] as const;

const PropertyTraceList = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { propertyTraces, loading } = useSelector((state: RootState) => state.propertyTraces);

  const {
    pageSize,
    currentPage,
    currentFilters,
    handlePageChange,
    handleItemsPerPageChange,
    handleFilterChange,
  } = usePagination<PropertyTrace, PropertyTraceFilters>({
    fetchData: (filters) => dispatch(fetchPropertyTraces(filters)),
    defaultPageSize: TABLE_DEFAULTS.PAGE_SIZE,
  });

  const modal = useModal<PropertyTrace>({
    onSuccess: () => {
      modal.handleClose();
      dispatch(fetchPropertyTraces({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
    },
  });

  const handleAdd = () => {
    modal.handleOpen('create');
  };

  const handleEdit = (propertyTrace: PropertyTrace) => {
    modal.handleOpen('edit', propertyTrace);
  };

  const handleDelete = (propertyTrace: PropertyTrace) => {
    modal.handleOpen('delete', propertyTrace);
  };

  const handleView = (propertyTrace: PropertyTrace) => {
    navigate(`/property-traces/${propertyTrace.Id}`);
  };

  const columns = [
    { header: 'Name', accessor: 'Name' as const },
    { 
      header: 'Date', 
      accessor: (item: PropertyTrace) => new Date(item.DateSale).toLocaleDateString() 
    },
    { 
      header: 'Value', 
      accessor: (item: PropertyTrace) => `$${item.Value.toLocaleString()}`,
      className: 'text-right'
    },
    { 
      header: 'Tax', 
      accessor: (item: PropertyTrace) => `$${item.Tax.toLocaleString()}`,
      className: 'text-right'
    },
    {
      header: 'Actions',
      accessor: (propertyTrace: PropertyTrace) => (
        <ActionButtons
          onView={() => handleView(propertyTrace)}
          onEdit={() => handleEdit(propertyTrace)}
          onDelete={() => handleDelete(propertyTrace)}
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
      <span className="sr-only sm:not-sr-only">Add Property Trace</span>
    </Button>
  );

  return (
    <>
      <PageLayout
        title="Property Traces"
        subtitle="Manage property transaction history"
        actions={actions}
        filters={
          <FilterForm<PropertyTraceFilters>
            fields={filterFields}
            onSubmit={handleFilterChange}
            loading={loading}
          />
        }
      >
        <DataTable<PropertyTrace>
          columns={columns}
          data={propertyTraces}
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
        entityType="propertyTrace"
        actionType={modal.mode}
        data={modal.data}
        isLoading={modal.isLoading}
        onSubmit={async (data) => {
          await modal.handleAction(async () => {
            switch (modal.mode) {
              case 'create':
                await propertyTraceService.createPropertyTrace(data);
                break;
              case 'edit':
                if (modal.data) {
                  await propertyTraceService.updatePropertyTrace(modal.data.Id, data);
                }
                break;
              case 'delete':
                if (modal.data) {
                  await propertyTraceService.deletePropertyTrace(modal.data.Id);
                }
                break;
            }
          });
        }}
      />
    </>
  );
};

export default PropertyTraceList;