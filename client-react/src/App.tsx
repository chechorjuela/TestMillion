import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Provider } from 'react-redux';
import { store } from './store/store';
import { Toaster } from 'react-hot-toast';

// Styles
import './index.css';

// Components
import { Navigation } from './components/organisms/Navigation';

// Pages
import PropertyList from './features/properties/pages/PropertyList';
import PropertyDetail from './features/properties/pages/PropertyDetail';
import OwnerList from './features/owners/pages/OwnerList';
import OwnerView from './features/owners/pages/OwnerView';
import PropertyTraceList from './features/propertyTraces/pages/PropertyTraceList';
import PropertyTraceView from './features/propertyTraces/pages/PropertyTraceView';
import PropertyImageList from './features/propertyImages/pages/PropertyImageList';
import PropertyImageView from './features/propertyImages/pages/PropertyImageView';

import { apiClient } from './lib/api/client';

function App() {
  return (
    <Provider store={store}>
      <Router>
<div className="min-h-screen bg-gray-50">
          <Toaster
            position="top-right"
            toastOptions={{
              duration: 3000,
              style: {
                borderRadius: '8px',
                background: '#333',
                color: '#fff',
              },
              success: {
                style: {
                  background: '#10B981',
                },
                iconTheme: {
                  primary: '#fff',
                  secondary: '#10B981',
                },
              },
              error: {
                style: {
                  background: '#EF4444',
                },
                iconTheme: {
                  primary: '#fff',
                  secondary: '#EF4444',
                },
              },
            }}
          />
          <Navigation />
          <Routes>
            <Route path="/" element={<Navigate to="/properties" replace />} />
            <Route path="/properties" element={<PropertyList />} />
            <Route path="/properties/:id" element={<PropertyDetail />} />
            <Route path="/owners" element={<OwnerList />} />
            <Route path="/owners/:id" element={<OwnerView />} />
            <Route path="/property-traces" element={<PropertyTraceList />} />
            <Route path="/property-traces/:id" element={<PropertyTraceView />} />
            <Route path="/property-images" element={<PropertyImageList />} />
            <Route path="/property-images/:id" element={<PropertyImageView />} />
          </Routes>
        </div>
      </Router>
    </Provider>
  )
}

export default App
