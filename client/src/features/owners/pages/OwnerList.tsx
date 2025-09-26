import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { Owner, OwnerFilters } from '../../../types';
import { fetchOwners } from '../ownerSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionModal } from '../../../components/organisms/ActionModal';
import { BsEye, BsPencil, BsTrash } from 'react-icons/bs';
import { ownerService } from '../ownerService';

const OwnerList = () => {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [ownerToDelete, setOwnerToDelete] = useState<Owner | null>(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedOwner, setSelectedOwner] = useState<Owner | null>(null);
  const [pageSize, setPageSize] = useState(10);
  const [currentPage, setCurrentPage] = useState(1);
  const [currentFilters, setCurrentFilters] = useState<OwnerFilters>({});
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { owners, loading, error, metadata } = useSelector((state: RootState) => state.owners);

  const handlePageChange = async (page: number) => {
    try {
      if (page > 0 && (!metadata || page <= metadata.TotalPages) && page !== currentPage) {
        setCurrentPage(page);
        dispatch(fetchOwners({ ...currentFilters, PageNumber: page, PageSize: pageSize })).unwrap()
          .catch(error => {
            console.error('Error fetching owners:', error);
          });
      }
    } catch (error) {
      console.error('Error in handlePageChange:', error);
    }
  };

  const handleEditOwner = (owner: Owner) => {
    setSelectedOwner(owner);
    setIsEditModalOpen(true);
  };

  const handleUpdateOwner = async (data: Partial<Owner>) => {
    if (selectedOwner) {
      try {
        await ownerService.updateOwner(selectedOwner.Id, data);
        
        // Recargar la lista después de actualizar
        await dispatch(fetchOwners({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setSelectedOwner(null);
        setIsEditModalOpen(false);
      } catch (error) {
        console.error('Error updating owner:', error);
      }
    }
  };

  const handleDeleteOwner = (owner: Owner) => {
    setOwnerToDelete(owner);
    setIsDeleteModalOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (ownerToDelete) {
      try {
        await ownerService.deleteOwner(ownerToDelete.Id);

        // Recargar la lista después de eliminar
        await dispatch(fetchOwners({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setIsDeleteModalOpen(false);
        setOwnerToDelete(null);
      } catch (error) {
        console.error('Error deleting owner:', error);
      }
    }
  };

  const handleItemsPerPageChange = async (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
    await dispatch(fetchOwners({
      ...currentFilters,
      PageNumber: 1,
      PageSize: newPageSize
    }));
  };

  useEffect(() => {
    dispatch(fetchOwners({
      ...currentFilters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  }, [dispatch, pageSize, currentFilters]);

  const handleFilter = (filters: OwnerFilters) => {
    setCurrentFilters(filters);
    setCurrentPage(1);
    dispatch(fetchOwners({
      ...filters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  };

  const filterFields = [
    { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
    { name: 'address', label: 'Address', type: 'text' as const, placeholder: 'Search by address' },
  ];

  const columns = [
    { header: 'Name', accessor: 'Name' as const },
    { header: 'Address', accessor: 'Address' as const },
    {
      header: 'Actions',
      accessor: (owner: Owner) => (
        <div className="flex justify-end gap-2">
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/owners/${owner.Id}`);
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
              handleEditOwner(owner);
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
              handleDeleteOwner(owner);
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
      Add Owner
    </Button>
  );

  return (
    <>
      <div>
        {/* Toast container */}
        <div className="fixed top-4 right-4 z-50" />
      </div>
      <PageLayout
        title="Owners"
        subtitle="Manage property owners"
        actions={actions}
        filters={
          <FilterForm<OwnerFilters>
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
          data={owners}
          loading={loading}
          keyExtractor={(item) => item.Id}
          onRowClick={(item) => navigate(`/owners/${item.Id}`)}
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
        entityType="owner"
        actionType="create"
        onSubmit={async (data) => {
          try {
            await ownerService.createOwner(data);
            dispatch(fetchOwners({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
            setIsAddModalOpen(false);
          } catch (error) {
            console.error('Error creating owner:', error);
          }
        }}
      />
      <ActionModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        entityType="owner"
        actionType="edit"
        data={selectedOwner}
        onSubmit={handleUpdateOwner}
      />
      <ActionModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        entityType="owner"
        actionType="delete"
        data={ownerToDelete}
        onSubmit={handleConfirmDelete}
      />
    </>
  );
};

export default OwnerList;