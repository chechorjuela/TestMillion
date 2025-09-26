import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { propertyImageService } from '../propertyImageService';
import type {PropertyImage} from '../../../types';
import { Button } from '../../../components/atoms/Button';
import { PageLayout } from '../../../components/templates/PageLayout';
import { Spinner } from '../../../components/atoms/Spinner';
import { BsArrowLeft } from 'react-icons/bs';

const PropertyImageView = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [propertyImage, setPropertyImage] = useState<PropertyImage | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPropertyImage = async () => {
      try {
        if (!id) return;
        const data = await propertyImageService.getPropertyImageById(id);
        setPropertyImage(data);
      } catch (err) {
        setError('Failed to load property image details');
        console.error('Error fetching property image:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchPropertyImage();
  }, [id]);

  if (loading) {
    return (
      <PageLayout title="Property Image Details">
        <div className="flex justify-center items-center h-64">
          <Spinner />
        </div>
      </PageLayout>
    );
  }

  if (error || !propertyImage) {
    return (
      <PageLayout title="Property Image Details">
        <div className="bg-red-50 text-red-600 p-4 rounded-md">
          {error || 'Property image not found'}
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout
      title="Property Image Details"
      actions={
        <Button
          variant="outline"
          onClick={() => navigate('/property-images')}
          className="flex items-center gap-2"
        >
          <BsArrowLeft /> Back to List
        </Button>
      }
    >
      <div className="bg-white shadow rounded-lg p-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-lg font-semibold mb-4">Image Information</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">File</label>
                <div className="mt-2">
                  <img
                    src={propertyImage.File}
                    alt="Property"
                    className="max-w-full h-auto rounded-lg shadow-md"
                  />
                </div>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">Enabled</label>
                <p className="mt-1">
                  {propertyImage.Enabled ? (
                    <span className="text-green-600">Yes</span>
                  ) : (
                    <span className="text-red-600">No</span>
                  )}
                </p>
              </div>
            </div>
          </div>

          {propertyImage.Property && (
            <div>
              <h3 className="text-lg font-semibold mb-4">Property Information</h3>
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Property Name</label>
                  <p className="mt-1">{propertyImage.Property.Name}</p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Property Address</label>
                  <p className="mt-1">{propertyImage.Property.Address}</p>
                </div>
                <Button
                  variant="outline"
                  size="sm"
                  className="mt-2"
                  onClick={() => navigate(`/properties/${propertyImage.Property.Id}`)}
                >
                  View Property
                </Button>
              </div>
            </div>
          )}
        </div>
      </div>
    </PageLayout>
  );
};

export default PropertyImageView;