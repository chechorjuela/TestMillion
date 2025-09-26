import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ownerService } from '../ownerService';
import type {Owner} from '../../../types';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { Spinner } from '../../../components/atoms/Spinner';
import { BsArrowLeft } from 'react-icons/bs';

const OwnerView = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [owner, setOwner] = useState<Owner | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchOwner = async () => {
      try {
        if (!id) return;
        const data = await ownerService.getOwnerById(id);
        setOwner(data);
      } catch (err) {
        setError('Failed to load owner details');
        console.error('Error fetching owner:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchOwner();
  }, [id]);

  if (loading) {
    return (
      <PageLayout title="Owner Details">
        <div className="flex justify-center items-center h-64">
          <Spinner />
        </div>
      </PageLayout>
    );
  }

  if (error || !owner) {
    return (
      <PageLayout title="Owner Details">
        <div className="bg-red-50 text-red-600 p-4 rounded-md">
          {error || 'Owner not found'}
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout
      title="Owner Details"
      actions={
        <Button
          variant="outline"
          onClick={() => navigate('/owners')}
          className="flex items-center gap-2"
        >
          <BsArrowLeft /> Back to List
        </Button>
      }
    >
      <div className="bg-white shadow rounded-lg p-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-lg font-semibold mb-4">Basic Information</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Name</label>
                <p className="mt-1">{owner.Name}</p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Address</label>
                <p className="mt-1">{owner.Address}</p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Birthday</label>
                <p className="mt-1">{new Date(owner.Birthday).toLocaleDateString()}</p>
              </div>
            </div>
          </div>

          {owner.Properties && owner.Properties.length > 0 && (
            <div>
              <h3 className="text-lg font-semibold mb-4">Properties Owned</h3>
              <div className="space-y-4">
                {owner.Properties.map((property) => (
                  <div key={property.Id} className="border rounded p-4">
                    <h4 className="font-medium">{property.Name}</h4>
                    <p className="text-sm text-gray-600">{property.Address}</p>
                    <p className="text-sm text-gray-600">${property.Price.toLocaleString()}</p>
                    <Button
                      variant="outline"
                      size="sm"
                      className="mt-2"
                      onClick={() => navigate(`/properties/${property.Id}`)}
                    >
                      View Property
                    </Button>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </PageLayout>
  );
};

export default OwnerView;