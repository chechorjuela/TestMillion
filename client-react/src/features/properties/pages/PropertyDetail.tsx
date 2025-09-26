import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { motion } from 'framer-motion';
import { fetchPropertyById } from '../propertySlice';
import type {RootState, AppDispatch} from '../../../store/store';
import { Button } from '../../../components/atoms/Button';

const PropertyDetail = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();
  const { selectedProperty: property, loading, error } = useSelector(
    (state: RootState) => state.properties
  );

  useEffect(() => {
    if (id) {
      dispatch(fetchPropertyById(id));
    }
  }, [dispatch, id]);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-50 text-red-600 p-4 rounded-md">
          {error}
        </div>
      </div>
    );
  }

  if (!property) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 mb-4">Property not found</h2>
          <Button onClick={() => navigate('/')}>Back to Properties</Button>
        </div>
      </div>
    );
  }

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      exit={{ opacity: 0 }}
      className="container mx-auto px-4 py-8"
    >
      <Button
        variant="outline"
        className="mb-6"
        onClick={() => navigate('/')}
      >
        ← Back to Properties
      </Button>

      <div className="bg-white rounded-lg shadow-lg overflow-hidden">
        <div className="relative h-96">
          <img
            src={property.MainImage || 'https://via.placeholder.com/800x600?text=No+Image'}
            alt={property.Name}
            className="w-full h-full object-cover"
          />
        </div>

        <div className="p-6">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">
            {property.Name}
          </h1>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-2">
                Location
              </h2>
              <p className="text-gray-600">{property.Address}</p>
            </div>
            
            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-2">
                Price
              </h2>
              <p className="text-2xl font-bold text-primary-600">
                ${property.Price.toLocaleString()}
              </p>
            </div>
            
            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-2">
                Year Built
              </h2>
              <p className="text-gray-600">{property.Year}</p>
            </div>

            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-2">
                Code Internal
              </h2>
              <p className="text-gray-600">{property.CodeInternal}</p>
            </div>

            {property.Owner && (
              <div className="col-span-2">
                <h2 className="text-lg font-semibold text-gray-900 mb-2">
                  Owner Information
                </h2>
                <div className="flex items-center space-x-4">
                  <img
                    src={property.Owner.Photo || 'https://via.placeholder.com/100x100?text=No+Photo'}
                    alt={property.Owner.Name}
                    className="w-12 h-12 rounded-full object-cover"
                  />
                  <div>
                    <p className="font-medium text-gray-900">{property.Owner.Name}</p>
                    <p className="text-gray-600">{property.Owner.Address}</p>
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </motion.div>
  );
};

export default PropertyDetail;