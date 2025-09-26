import { Link } from 'react-router-dom';
import { motion } from 'framer-motion';

export const Logo = () => {
  return (
    <Link to="/" className="flex-shrink-0">
      <motion.h1 
        className="text-2xl font-bold text-primary-600"
        whileHover={{ scale: 1.05 }}
      >
        Million
      </motion.h1>
    </Link>
  );
};