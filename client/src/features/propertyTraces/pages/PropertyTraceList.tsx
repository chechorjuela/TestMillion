import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { PropertyTrace, PropertyTraceFilters } from '../../../types';
import { fetchPropertyTraces } from '../propertyTraceSlice';
import type { RootState, AppDispatch } from '../../../store/store';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { FilterForm } from '../../../components/templates/FilterForm';
import { DataTable } from '../../../components/templates/DataTable';
import { ActionModal } from '../../../components/organisms/ActionModal';
import { BsEye, BsPencil, BsTrash } from 'react-icons/bs';
import { propertyTraceService } from '../propertyTraceService';

const PropertyTraceList = () => {
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [propertyTraceToDelete, setPropertyTraceToDelete] = useState<PropertyTrace | null>(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedPropertyTrace, setSelectedPropertyTrace] = useState<PropertyTrace | null>(null);
  const [pageSize, setPageSize] = useState(10);
  const [currentPage, setCurrentPage] = useState(1);
  const [currentFilters, setCurrentFilters] = useState<PropertyTraceFilters>({});
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { propertyTraces, loading, error, metadata } = useSelector((state: RootState) => state.propertyTraces);

  const handlePageChange = async (page: number) => {
    try {
      if (page > 0 && (!metadata || page <= metadata.TotalPages) && page !== currentPage) {
        setCurrentPage(page);
        dispatch(fetchPropertyTraces({ ...currentFilters, PageNumber: page, PageSize: pageSize })).unwrap()
          .catch(error => {
            console.error('Error fetching property traces:', error);
          });
      }
    } catch (error) {
      console.error('Error in handlePageChange:', error);
    }
  };

  const handleEditPropertyTrace = (propertyTrace: PropertyTrace) => {
    setSelectedPropertyTrace(propertyTrace);
    setIsEditModalOpen(true);
  };

  const handleUpdatePropertyTrace = async (data: Partial<PropertyTrace>) => {
    if (selectedPropertyTrace) {
      try {
        // Convert the data to match the expected DTO format
        const updateData = {
          name: data.name,
          value: data.value,
          tax: data.tax,
          dateSale: data.dateSale,
          idProperty: selectedPropertyTrace.IdProperty,
        };
        await propertyTraceService.updatePropertyTrace(selectedPropertyTrace.Id, updateData);
        
        // Recargar la lista después de actualizar
        await dispatch(fetchPropertyTraces({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setSelectedPropertyTrace(null);
        setIsEditModalOpen(false);
      } catch (error) {
        console.error('Error updating property trace:', error);
      }
    }
  };

  const handleDeletePropertyTrace = (propertyTrace: PropertyTrace) => {
    setPropertyTraceToDelete(propertyTrace);
    setIsDeleteModalOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (propertyTraceToDelete) {
      try {
        await propertyTraceService.deletePropertyTrace(propertyTraceToDelete.Id);

        // Recargar la lista después de eliminar
        await dispatch(fetchPropertyTraces({
          ...currentFilters,
          PageNumber: currentPage,
          PageSize: pageSize
        })).unwrap();

        setIsDeleteModalOpen(false);
        setPropertyTraceToDelete(null);
      } catch (error) {
        console.error('Error deleting property trace:', error);
      }
    }
  };

  const handleItemsPerPageChange = async (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
    await dispatch(fetchPropertyTraces({
      ...currentFilters,
      PageNumber: 1,
      PageSize: newPageSize
    }));
  };

  useEffect(() => {
    dispatch(fetchPropertyTraces({
      ...currentFilters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  }, [dispatch, pageSize, currentFilters]);

  const handleFilter = (filters: PropertyTraceFilters) => {
    setCurrentFilters(filters);
    setCurrentPage(1);
    dispatch(fetchPropertyTraces({
      ...filters,
      PageNumber: 1,
      PageSize: pageSize
    }));
  };

  const filterFields = [
    { name: 'name', label: 'Name', type: 'text' as const, placeholder: 'Search by name' },
    { name: 'propertyId', label: 'Property ID', type: 'text' as const, placeholder: 'Search by property ID' },
    { name: 'minValue', label: 'Min Value', type: 'number' as const, placeholder: 'Min value' },
    { name: 'maxValue', label: 'Max Value', type: 'number' as const, placeholder: 'Max value' },
  ];

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
        <div className="flex justify-end gap-2">
          <Button
            size="sm"
            variant="outline"
            onClick={(e) => {
              e.stopPropagation();
              navigate(`/property-traces/${propertyTrace.Id}`);
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
              handleEditPropertyTrace(propertyTrace);
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
              handleDeletePropertyTrace(propertyTrace);
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
      Add Property Trace
    </Button>
  );

  return (
    <>
      <div>
        {/* Toast container */}
        <div className="fixed top-4 right-4 z-50" />
      </div>
      <PageLayout
        title="Property Traces"
        subtitle="Manage property transaction history"
        actions={actions}
        filters={
          <FilterForm<PropertyTraceFilters>
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
          data={propertyTraces}
          loading={loading}
          keyExtractor={(item) => item.Id}
          onRowClick={(item) => navigate(`/property-traces/${item.Id}`)}
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
        entityType="propertyTrace"
        actionType="create"
        onSubmit={async (data) => {
          try {
            await propertyTraceService.createPropertyTrace(data);
            dispatch(fetchPropertyTraces({ ...currentFilters, PageNumber: 1, PageSize: pageSize }));
            setIsAddModalOpen(false);
          } catch (error) {
            console.error('Error creating property trace:', error);
          }
        }}
      />
      <ActionModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        entityType="propertyTrace"
        actionType="edit"
        data={selectedPropertyTrace}
        onSubmit={handleUpdatePropertyTrace}
      />
      <ActionModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        entityType="propertyTrace"
        actionType="delete"
        data={propertyTraceToDelete}
        onSubmit={handleConfirmDelete}
      />
    </>
  );
};

export default PropertyTraceList;