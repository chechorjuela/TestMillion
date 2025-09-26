import { motion } from 'framer-motion';

interface PageLayoutProps {
  title: string;
  subtitle?: string;
  actions?: React.ReactNode;
  filters?: React.ReactNode;
  children: React.ReactNode;
}

export const PageLayout = ({
  title,
  subtitle,
  actions,
  filters,
  children,
}: PageLayoutProps) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      exit={{ opacity: 0, y: -20 }}
      className="container mx-auto px-4 py-8 bg-theme-white min-h-screen"
    >
      <div className="mb-8">
        <div className="flex justify-between items-start">
          <div>
            <h1 className="text-3xl font-bold text-theme-black">{title}</h1>
            {subtitle && (
              <p className="mt-2 text-sm text-theme-medium-gray">{subtitle}</p>
            )}
          </div>
          {actions && (
            <div className="flex gap-3">{actions}</div>
          )}
        </div>
        
        {filters && (
          <div className="mt-6">
            {filters}
          </div>
        )}
      </div>

      <div className="bg-theme-white rounded-lg shadow-lg border border-theme-silver/20">
        {children}
      </div>
    </motion.div>
  );
};