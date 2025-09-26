import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { Owner, OwnerFilters } from '../../../types';
import { fetchOwners } from '../ownerSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionButtons } from '../../../components/molecules/ActionButtons';
import { usePagination } from '../../../hooks/usePagination';
import { useModal } from '../../../hooks/useModal';
import { ownerService } from '../ownerService';
import { PAGE_SIZES, TABLE_DEFAULTS } from '../../../config/constants';
import { BsPlus } from 'react-icons/bs';
import { ActionModal } from '../../../components/organisms/ActionModal';

const filterFields = [
  { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
  { name: 'address', label: 'Address', type: 'text' as const, placeholder: 'Search by address' },
  { name: 'startBirthdate', label: 'From Date', type: 'date' as const },
  { name: 'endBirthdate', label: 'To Date', type: 'date' as const },
] as const;

const OwnerList = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { owners, loading } = useSelector((state: RootState) => state.owners);

  const {
    pageSize,
    currentPage,
    currentFilters,
    handlePageChange,
    handleItemsPerPageChange,
    handleFilterChange,
  } = usePagination<Owner, OwnerFilters>({
    fetchData: (filters) => dispatch(fetchOwners(filters)),
    defaultPageSize: TABLE_DEFAULTS.PAGE_SIZE,
  });

  const modal = useModal<Owner>({
    onSuccess: () => {
      modal.handleClose();
      dispatch(fetchOwners({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
    },
  });

  const handleAdd = () => {
    modal.handleOpen('create');
  };

  const handleEdit = (owner: Owner) => {
    modal.handleOpen('edit', owner);
  };

  const handleDelete = (owner: Owner) => {
    modal.handleOpen('delete', owner);
  };

  const handleView = (owner: Owner) => {
    navigate(`/owners/${owner.Id}`);
  };

  const columns = [
    { 
      header: 'Photo', 
      accessor: (owner: Owner) => (
        <div className="w-12 h-12">
          <img 
            src={owner.Photo || 'https://via.placeholder.com/48'} 
            alt={owner.Name}
            className="w-full h-full rounded-full object-cover"
          />
        </div>
      ),
      className: 'w-16'
    },
    { header: 'Name', accessor: 'Name' as const },
    { header: 'Address', accessor: 'Address' as const },
    { 
      header: 'Birthday',
      accessor: (owner: Owner) => new Date(owner.Birthdate).toLocaleDateString(),
    },
    {
      header: 'Actions',
      accessor: (owner: Owner) => (
        <ActionButtons
          onView={() => handleView(owner)}
          onEdit={() => handleEdit(owner)}
          onDelete={() => handleDelete(owner)}
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
      <span className="sr-only sm:not-sr-only">Add Owner</span>
    </Button>
  );

  return (
    <>
      <PageLayout
        title="Owners"
        subtitle="Manage property owners"
        actions={actions}
        filters={
          <FilterForm<OwnerFilters>
            fields={filterFields}
            onSubmit={handleFilterChange}
            loading={loading}
          />
        }
      >
        <DataTable<Owner>
          columns={columns}
          data={owners}
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
        entityType="owner"
        actionType={modal.mode}
        data={modal.data}
        isLoading={modal.isLoading}
        onSubmit={async (data) => {
          await modal.handleAction(async () => {
            switch (modal.mode) {
              case 'create':
                await ownerService.createOwner(data);
                break;
              case 'edit':
                if (modal.data) {
                  await ownerService.updateOwner(modal.data.Id, data);
                }
                break;
              case 'delete':
                if (modal.data) {
                  await ownerService.deleteOwner(modal.data.Id);
                }
                break;
            }
          });
        }}
      />
    </>
  );
};

export default OwnerList;