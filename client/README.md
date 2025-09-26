# TestMillion Project

## Frontend (React Client)

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

## Backend (.NET Core)

### 🏗️ Architecture

The backend follows Clean Architecture principles with CQRS (Command Query Responsibility Segregation) and Hexagonal Architecture patterns:

```
src/
├── TestMillion.Application/     # Application layer (Use cases)
│   ├── Common/                  # Shared components
│   │   ├── Behaviours/         # Pipeline behaviors
│   │   ├── Exceptions/         # Custom exceptions
│   │   └── Interfaces/         # Core interfaces
│   ├── Properties/             # Property feature
│   │   ├── Commands/           # Write operations
│   │   └── Queries/            # Read operations
│   └── PropertyImages/         # Property Images feature
│       ├── Commands/
│       └── Queries/
├── TestMillion.Domain/         # Domain layer
│   ├── Common/                 # Shared domain components
│   ├── Entities/              # Domain entities
│   ├── Events/                # Domain events
│   └── ValueObjects/          # Value objects
├── TestMillion.Infrastructure/ # Infrastructure layer
│   ├── Identity/              # Authentication/Authorization
│   ├── Persistence/          # Data access
│   └── Services/             # External services
└── TestMillion.Presentation/  # API Controllers
    ├── Controllers/          # REST endpoints
    ├── Filters/              # Action filters
    └── Middleware/           # Custom middleware
```

### 🚀 Technology Stack

- **.NET 7**: Modern, high-performance framework
- **Entity Framework Core**: ORM for data access
- **MediatR**: CQRS and Mediator pattern implementation
- **FluentValidation**: Request validation
- **AutoMapper**: Object mapping
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Testing framework
- **Moq**: Mocking framework

### 📦 Key Design Patterns

1. **CQRS Pattern**
   - Commands for write operations
   - Queries for read operations
   - Separate models for reads and writes

2. **Hexagonal Architecture**
   - Domain-centric design
   - Ports and Adapters pattern
   - Clear dependency rules

3. **Repository Pattern**
   - Generic repository implementation
   - Unit of Work pattern
   - Specification pattern

4. **Mediator Pattern**
   - Decoupled command/query handlers
   - Pipeline behaviors
   - Cross-cutting concerns

### 🔧 Setup & Development

1. **Prerequisites**:
   - .NET 7 SDK
   - SQL Server (or compatible database)
   - Visual Studio 2022 or VS Code

2. **Database Setup**:
   ```bash
   dotnet ef database update
   ```

3. **Run Application**:
   ```bash
   dotnet run --project src/TestMillion.Presentation
   ```

4. **Run Tests**:
   ```bash
   dotnet test
   ```

### 🧪 Testing

The backend includes several types of tests:

1. **Unit Tests**:
   - Domain logic
   - Command/Query handlers
   - Validation rules

2. **Integration Tests**:
   - Repository operations
   - Database interactions
   - API endpoints

3. **Architecture Tests**:
   - Dependency rules
   - Layer separation
   - Naming conventions

### 📝 API Documentation

Swagger UI is available at `/swagger` when running in development mode.

### 🔐 Security

- JWT authentication
- Role-based authorization
- Input validation
- CORS configuration

### 🔍 Validation

Request validation is handled through:
- FluentValidation rules
- Model validation attributes
- Custom validation behaviors

### 🗄️ Database

- Code-first approach with Entity Framework Core
- Migration-based schema management
- Optimized queries with specifications

### 📈 Performance Features

- Async/await throughout
- Response caching
- Efficient query patterns
- Database indexing strategy

### 🔄 Error Handling

- Global exception handling
- Custom exception types
- Detailed error responses
- Logging with Serilog

### 🛠️ Development Tools

- Visual Studio 2022
- Rider (optional)
- SQL Server Management Studio
- Postman/Insomnia for API testing
