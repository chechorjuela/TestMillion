# Million Properties API

API para gestionar propiedades inmobiliarias utilizando .NET, MongoDB y React/Next.js.

## Tecnologías Utilizadas

- Backend: .NET 8
- Base de datos: MongoDB
- Caché: Redis
- Frontend: Next.js (en desarrollo)
- Pruebas: NUnit

## Requisitos Previos

- .NET 8 SDK
- MongoDB Server
- Redis Server
- Node.js y npm (para el frontend)

## Configuración

1. Clone el repositorio:
   ```bash
   git clone <repository-url>
   cd TestMillion
   ```

2. Configure la base de datos MongoDB:
   - Asegúrese de que MongoDB está ejecutándose
   - La base de datos se creará automáticamente al iniciar la aplicación

3. Configure Redis:
   - Asegúrese de que Redis está ejecutándose
   - El caché se configurará automáticamente

4. Configure los ajustes de la aplicación:
   - Actualice `appsettings.json` con sus configuraciones:
   ```json
   {
     "MongoDb": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "MillionProperties"
     },
     "Redis": {
       "ConnectionString": "localhost:6379"
     }
   }
   ```

## Ejecución

1. Ejecute la API:
   ```bash
   cd src/Presentation
   dotnet run
   ```

2. Acceda a la documentación de la API:
   - Swagger UI: http://localhost:5000/swagger
   - API endpoint base: http://localhost:5000/api

## Estructura de la Base de Datos

### Colecciones

1. `properties`
   - `Id`: string (ObjectId)
   - `IdOwner`: string (referencia a owners)
   - `Name`: string
   - `Address`: string
   - `Price`: decimal
   - `CodeInternal`: string
   - `Year`: int

2. `owners`
   - `Id`: string (ObjectId)
   - `Name`: string
   - `Address`: string

3. `propertyimages`
   - `Id`: string (ObjectId)
   - `IdProperty`: string (referencia a properties)
   - `File`: string (ruta del archivo)
   - `Enabled`: boolean

## Endpoints de la API

### Propiedades

1. `GET /api/properties`
   - Lista propiedades con filtros opcionales
   - Parámetros:
     - `name`: string (opcional)
     - `address`: string (opcional)
     - `minPrice`: decimal (opcional)
     - `maxPrice`: decimal (opcional)
     - `page`: int (por defecto: 1)
     - `pageSize`: int (por defecto: 10)
     - `sortBy`: string (opcional)
     - `ascending`: boolean (por defecto: true)

2. `GET /api/properties/{id}`
   - Obtiene los detalles de una propiedad específica

3. `POST /api/properties`
   - Crea una nueva propiedad
   - Body: PropertyDto

## Pruebas

Ejecute las pruebas unitarias:
```bash
cd tests/TestMillion.Tests
dotnet test
```

## Optimizaciones Implementadas

1. Índices de MongoDB para búsquedas eficientes
2. Sistema de caché con Redis
3. Paginación para grandes conjuntos de datos
4. Filtros optimizados
5. Compresión de respuestas HTTP

## Contacto

Para acceso al repositorio o consultas:
- crios@millionluxury.com
- amorau@millionluxury.com