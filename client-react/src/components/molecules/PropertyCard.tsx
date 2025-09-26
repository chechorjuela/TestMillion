import { motion } from 'framer-motion';
import { Property } from '../../types/property';
import { Link } from 'react-router-dom';

interface PropertyCardProps {
  property: Property;
}

export const PropertyCard = ({ property }: PropertyCardProps) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      exit={{ opacity: 0, y: -20 }}
      className="bg-white rounded-lg shadow-md overflow-hidden"
    >
      <Link to={`/properties/${property.idOwner}`}>
        <div className="aspect-w-16 aspect-h-9">
          <img
            src={property.image}
            alt={property.name}
            className="w-full h-48 object-cover"
          />
        </div>
        <div className="p-4">
          <h3 className="text-lg font-semibold text-gray-900 mb-2">
            {property.name}
          </h3>
          <p className="text-gray-600 text-sm mb-2">
            {property.addressProperty}
          </p>
          <p className="text-primary-600 font-bold">
            ${property.priceProperty.toLocaleString()}
          </p>
        </div>
      </Link>
    </motion.div>
  );
};