import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { propertyTraceService } from '../propertyTraceService';
import type {PropertyTrace} from '../../../types';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { Spinner } from '../../../components/atoms/Spinner';
import { BsArrowLeft } from 'react-icons/bs';

const PropertyTraceView = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [propertyTrace, setPropertyTrace] = useState<PropertyTrace | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPropertyTrace = async () => {
      try {
        if (!id) return;
        const data = await propertyTraceService.getPropertyTraceById(id);
        setPropertyTrace(data);
      } catch (err) {
        setError('Failed to load property trace details');
        console.error('Error fetching property trace:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchPropertyTrace();
  }, [id]);

  if (loading) {
    return (
      <PageLayout title="Property Trace Details">
        <div className="flex justify-center items-center h-64">
          <Spinner />
        </div>
      </PageLayout>
    );
  }

  if (error || !propertyTrace) {
    return (
      <PageLayout title="Property Trace Details">
        <div className="bg-red-50 text-red-600 p-4 rounded-md">
          {error || 'Property trace not found'}
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout
      title="Property Trace Details"
      actions={
        <Button
          variant="outline"
          onClick={() => navigate('/property-traces')}
          className="flex items-center gap-2"
        >
          <BsArrowLeft /> Back to List
        </Button>
      }
    >
      <div className="bg-white shadow rounded-lg p-6 space-y-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-lg font-semibold mb-4">Basic Information</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Name</label>
                <p className="mt-1">{propertyTrace.Name}</p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Date of Sale</label>
                <p className="mt-1">{new Date(propertyTrace.DateSale).toLocaleDateString()}</p>
              </div>
            </div>
          </div>
          <div>
            <h3 className="text-lg font-semibold mb-4">Financial Details</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Value</label>
                <p className="mt-1">${propertyTrace.Value.toLocaleString()}</p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Tax</label>
                <p className="mt-1">${propertyTrace.Tax.toLocaleString()}</p>
              </div>
            </div>
          </div>
        </div>

        {propertyTrace.Property && (
          <div className="mt-8">
            <h3 className="text-lg font-semibold mb-4">Property Information</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700">Property Name</label>
                <p className="mt-1">{propertyTrace.Property.Name}</p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Property Address</label>
                <p className="mt-1">{propertyTrace.Property.Address}</p>
              </div>
            </div>
          </div>
        )}
      </div>
    </PageLayout>
  );
};

export default PropertyTraceView;