# Hospital Billing Management System

A console-based hospital billing management system built with .NET 10, MongoDB, and clean architecture following SOLID principles with generic repository pattern.

## Project Structure

```
hospitalBillingManagement/
├── Data/
│   ├── MongoDbContext.cs          # MongoDB database connection
│   └── MongoDbSettings.cs         # Configuration model
├── Models/
│   ├── Patient.cs                 # Patient entity
│   ├── Product.cs                 # Product entity
│   └── CartItem.cs                # Shopping cart item
├── Repositories/
│   ├── Interfaces/
│   │   ├── IRepository.cs         # Generic base repository interface
│   │   ├── IPatientRepository.cs  # Patient data interface
│   │   └── IProductRepository.cs  # Product data interface
│   └── Implementations/
│       ├── PatientRepository.cs   # Patient data implementation
│       └── ProductRepository.cs   # Product data implementation
├── Services/
│   ├── Interfaces/
│   │   ├── IPatientService.cs     # Patient business interface
│   │   ├── IBillingService.cs     # Billing business interface
│   │   └── IPaymentProcessor.cs   # Payment processor interface
│   └── Implementations/
│       ├── PatientService.cs      # Patient business logic
│       ├── BillingService.cs      # Billing business logic
│       └── UpiPaymentProcessor.cs # UPI payment implementation
├── UI/
│   └── ConsoleUI.cs               # Console-based user interface
├── Program.cs                     # Application entry point
└── appsettings.json               # External configuration
```

## SOLID Principles Applied

- **SRP** - Each class has one responsibility
- **OCP** - Open for extension, closed for modification
- **LSP** - All implementations work correctly as interfaces
- **ISP** - Fine-grained interfaces for specific needs
- **DIP** - Depend on abstractions, not implementations

## Generic Repository Pattern

`IRepository<T>` - Base interface with CRUD operations:
- `AddAsync()`
- `UpdateAsync()`
- `DeleteAsync()`
- `GetAllAsync()`

Domain repositories extend generic interface:
- `IPatientRepository : IRepository<Patient>`
- `IProductRepository : IRepository<Product>`

## File Descriptions

### Program.cs
Bootstraps application using dependency injection. Builds configuration from appsettings.json, creates database context, instantiates repositories and services, initializes ConsoleUI.

### Data/MongoDbSettings.cs
Configuration model with ConnectionString and DatabaseName properties.

### Data/MongoDbContext.cs
Manages MongoDB database connection. Exposes Patients and Products collections.

### Models/Patient.cs
Patient entity with Id, FirstName, LastName, Email, MobileNo, TotalAmountSpent, IsPaid, PurchasedProducts, PaymentDate, UpiId, PendingAmount.

### Models/Product.cs
Product entity with Id, ProductId, Name, Rate.

### Models/CartItem.cs
Shopping cart item with ProductId, ProductName, Quantity, Rate.

### Repositories/Interfaces/IRepository.cs
Generic CRUD interface for all entities.

### Repositories/Interfaces/IPatientRepository.cs
Patient repository with SearchAsync for email/mobile/name lookup.

### Repositories/Interfaces/IProductRepository.cs
Product repository with GetByProductIdAsync and GetByProductIdsAsync methods.

### Repositories/Implementations/PatientRepository.cs
MongoDB implementation with SearchAsync - smart search detecting identifier type.

### Repositories/Implementations/ProductRepository.cs
MongoDB implementation with seeding of initial products.

### Services/Interfaces/IPatientService.cs
Patient service interface.

### Services/Interfaces/IBillingService.cs
Billing service interface.

### Services/Interfaces/IPaymentProcessor.cs
Payment processor interface.

### Services/Implementations/PatientService.cs
Patient business logic.

### Services/Implementations/BillingService.cs
Billing and payment processing.

### Services/Implementations/UpiPaymentProcessor.cs
UPI payment implementation.

### UI/ConsoleUI.cs
Console menu system with C, D, G, M, S, P options.

## How to Run
1. Install MongoDB
2. Update appsettings.json
3. Run: dotnet run

## Git Commit Message

```
refactor: Remove GetByIdAsync, implement generic repository, clean code

- Removed GetByIdAsync(string id) from all repositories
- Added generic IRepository<T> base interface
- Domain repositories extend generic interface
- All patient lookups use SearchAsync(email/mobile/name)
- Removed all comments from code
- Cleaned up unused files
- Simplified code structure
```