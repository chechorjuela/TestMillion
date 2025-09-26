# TestMillion Client

Frontend application built with React + TypeScript + Vite for the TestMillion project.

## 🚀 Technology Stack

- **React 18**: Modern JavaScript library for building user interfaces
- **TypeScript**: Static type-checking for enhanced development experience
- **Vite**: Next generation frontend tooling for fast development
- **Redux Toolkit**: State management solution
- **React Router**: Navigation and routing
- **TailwindCSS**: Utility-first CSS framework
- **Headless UI**: Unstyled, accessible UI components
- **Framer Motion**: Animation library
- **Vitest**: Unit testing framework
- **Testing Library**: Testing utilities

## 📁 Project Structure

```
src/
├── components/           # Reusable UI components
│   ├── atoms/           # Basic building blocks (buttons, inputs, etc.)
│   ├── molecules/       # Combinations of atoms
│   ├── organisms/       # Complex components
│   └── templates/       # Page layouts
├── config/              # Configuration files
├── features/           # Feature-based modules
│   ├── propertyImages/
│   ├── properties/
│   └── ...
├── hooks/              # Custom React hooks
├── services/           # API and other services
├── store/             # Redux store configuration
├── test/              # Test utilities and setup
├── types/             # TypeScript type definitions
└── App.tsx            # Application root component
```

### 🏗️ Architecture

The project follows a combination of atomic design and feature-based architecture:

1. **Atomic Design Components**:
   - `atoms`: Basic components like Button, Input, Logo
   - `molecules`: Composite components like ActionButtons, PropertyForm
   - `organisms`: Complex components like ActionModal
   - `templates`: Page layouts and structures

2. **Feature-based Organization**:
   Each feature (e.g., propertyImages) contains:
   - Pages
   - Components specific to the feature
   - Redux slice
   - Services
   - Types

3. **Global State Management**:
   - Uses Redux Toolkit for state management
   - Slices for each feature
   - Typed actions and reducers

4. **Type Safety**:
   - Comprehensive TypeScript types
   - Type-safe API calls
   - Strongly typed Redux store

## 📦 Main Dependencies

### Core
- `@reduxjs/toolkit`: State management
- `react-router-dom`: Routing
- `axios`: HTTP client
- `clsx`: CSS class construction

### UI & Styling
- `@headlessui/react`: Accessible UI components
- `@tailwindcss/forms`: Form styling
- `framer-motion`: Animations
- `react-icons`: Icon library

### Development
- `typescript`: Type checking
- `vite`: Build tool
- `eslint`: Code linting
- `prettier`: Code formatting

### Testing
- `vitest`: Testing framework
- `@testing-library/react`: React testing utilities
- `@testing-library/jest-dom`: DOM testing utilities
- `@testing-library/user-event`: User event simulation

## 🔧 Setup & Development

1. **Installation**:
   ```bash
   npm install
   ```

2. **Development**:
   ```bash
   npm run dev
   ```

3. **Build**:
   ```bash
   npm run build
   ```

4. **Preview Production Build**:
   ```bash
   npm run preview
   ```

## 🧪 Testing

The project uses Vitest and Testing Library for unit testing.

1. **Run Tests**:
   ```bash
   npm test
   ```

2. **Run Tests with Coverage**:
   ```bash
   npm run test:coverage
   ```

3. **Run Tests with UI**:
   ```bash
   npm run test:ui
   ```

### Test Structure

Tests are co-located with their components:
```
src/
├── components/
│   └── atoms/
│       ├── Button.tsx
│       └── __tests__/
│           └── Button.test.tsx
```

## 🔍 Code Quality

- ESLint configuration for TypeScript and React
- Prettier for consistent code formatting
- Pre-commit hooks for code quality checks

## 📚 Component Documentation

### Atoms
- `Button`: Reusable button component with variants
- `Input`: Form input with label and error handling
- `Modal`: Dialog component with accessibility features

### Molecules
- `ActionButtons`: Group of action buttons (edit, delete, etc.)
- `Pagination`: Page navigation component
- `PropertyForm`: Form for property data

### Organisms
- `ActionModal`: Modal for CRUD operations
- `DataTable`: Generic table component with sorting and pagination

### Templates
- `PageLayout`: Standard page layout with header and content areas

## 🔐 Environment Variables

Create a `.env` file in the root directory:

```env
VITE_API_URL=http://localhost:3000/api
```

## 🤝 Contributing

1. Branch naming convention: `feature/name` or `fix/name`
2. Commit messages follow conventional commits
3. All code must have corresponding tests
4. PR template must be filled out completely

## 📝 Scripts

- `dev`: Start development server
- `build`: Build for production
- `preview`: Preview production build
- `test`: Run tests
- `test:coverage`: Run tests with coverage
- `test:ui`: Run tests with UI
- `lint`: Lint code
- `format`: Format code with prettier

## 🚀 Deployment

Build the application for production:

```bash
npm run build
```

The built files will be in the `dist` directory, ready for deployment.

## 🐛 Common Issues & Solutions

1. **Missing Peer Dependencies**:
   ```bash
   npm install -D @types/react @types/react-dom
   ```

2. **Type Errors**:
   - Ensure TypeScript version matches in package.json
   - Run `npm install` to update dependencies

## 📈 Performance Considerations

- Code splitting with React.lazy()
- Memoization with useMemo and useCallback
- Image optimization with modern formats
- Tailwind CSS purging for production

# TestMillion Backend

Backend application built with .NET 8 following Clean Architecture and Domain-Driven Design principles.

## 🏗️ Architecture

The solution follows Clean Architecture with the following layers:

```
src/
├── TestMillion.Domain/           # Enterprise business rules
│   ├── Common/                   # Shared abstractions
│   ├── Entities/                 # Domain entities
│   ├── Interfaces/               # Repository interfaces
│   └── ValueObjects/             # Domain value objects
│
├── TestMillion.Application/      # Application business rules
│   ├── Common/                   # Cross-cutting concerns
│   │   ├── Behaviours/          # Pipeline behaviors
│   │   ├── Commands/            # Command interfaces
│   │   ├── Exceptions/          # Exception handling
│   │   ├── Mappings/            # AutoMapper profiles
│   │   ├── Models/              # DTOs and requests
│   │   ├── Queries/             # Query interfaces
│   │   └── Response/            # Response wrappers
│   └── DependencyInjection.cs   # DI configuration
│
└── TestMillion.Infrastructure/   # External concerns
    ├── Persistence/             # Data access
    │   └── MongoDB/             # MongoDB configuration
    ├── Repositories/            # Repository implementations
    └── DependencyInjection.cs   # DI configuration
```

### Domain Layer

- Contains enterprise business rules and core entities
- Implements DDD patterns (Entities, Value Objects)
- No dependencies on other projects

#### Key Components:
- `Entity.cs`: Base class for all entities
- `ValueObject.cs`: Base class for value objects
- `IBaseRepository.cs`: Generic repository interface

### Application Layer

- Contains application business rules
- Implements CQRS pattern with MediatR
- Handles cross-cutting concerns

#### Key Features:
- Validation behaviors
- Exception handling
- AutoMapper profiles
- Pagination support
- CQRS implementation

### Infrastructure Layer

- Implements external concerns
- MongoDB persistence
- Repository implementations

## 🗄️ Data Model

Core entities in the system:

- **Property**: Real estate property information
- **Owner**: Property owner details
- **PropertyImage**: Property images and metadata
- **PropertyTrace**: Property history and changes

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- MongoDB
- Docker (optional)

### Configuration

Add the following to your `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "your_mongodb_connection_string",
    "DatabaseName": "TestMillion"
  }
}
```

### Running the Application

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/TestMillion.git
   cd TestMillion
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run --project src/TestMillion.Api/TestMillion.Api.csproj
   ```

### Docker Support

Build and run with Docker:

```bash
docker-compose up --build
```

## 🧪 Testing

The solution includes various test types:

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/TestMillion.Application.Tests
```

## 📚 API Documentation

API documentation is available via Swagger UI when running the application:

```
http://localhost:5000/swagger
```

## 🔍 API Endpoints

### Properties
- `GET /api/properties`: List properties (with pagination)
- `GET /api/properties/{id}`: Get property details
- `POST /api/properties`: Create property
- `PUT /api/properties/{id}`: Update property
- `DELETE /api/properties/{id}`: Delete property

### Property Images
- `POST /api/properties/{id}/images`: Upload property image
- `GET /api/properties/{id}/images`: Get property images
- `DELETE /api/properties/{id}/images/{imageId}`: Delete property image

### Owners
- `GET /api/owners`: List owners
- `POST /api/owners`: Create owner
- `PUT /api/owners/{id}`: Update owner
- `DELETE /api/owners/{id}`: Delete owner

## 📦 Dependencies

### Core
- `Microsoft.NET.Sdk.Web`: Web SDK
- `MongoDB.Driver`: MongoDB driver
- `AutoMapper`: Object mapping
- `MediatR`: CQRS implementation

### Development
- `Swashbuckle.AspNetCore`: API documentation
- `FluentValidation`: Request validation
- `Serilog`: Logging

## 🔐 Security

- JWT authentication
- Role-based authorization
- Input validation
- Exception handling middleware
- Secure configuration management

## 🔄 Error Handling

The application implements a global exception handler that returns appropriate HTTP status codes:

- `400 Bad Request`: Validation errors
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Unexpected errors

All exceptions are logged with correlation IDs for tracking.
