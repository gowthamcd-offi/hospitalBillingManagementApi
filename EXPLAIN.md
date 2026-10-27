# Generic Repository Pattern & SOLID Principles

## Generics

### What is Generics?
Generics allow you to define type-safe data structures without committing to actual data types. This provides code reuse and type safety.

### Where Generics Are Used

#### 1. `IRepository<T>` - Base Generic Interface

```csharp
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
}
```

**Why Generics?**
- **Type Safety** - Compile-time checking ensures you only work with the correct entity type
- **Code Reusability** - Single interface works for Patient, Product, Order, etc.
- **No Casting** - No need to cast objects when retrieving from repository
- **Performance** - No boxing/unboxing of value types
- **Maintenance** - Changes to CRUD methods affect all repositories

**Example Usage:**
```csharp
public interface IPatientRepository : IRepository<Patient>
public interface IProductRepository : IRepository<Product>
```

#### 2. Repository Implementations Use Generic IMongoCollection<T>

```csharp
public class PatientRepository : IPatientRepository
{
    private readonly IMongoCollection<Patient> _collection;
    
    public PatientRepository(MongoDbContext context)
    {
        _collection = context.Patients;  // IMongoCollection<Patient>
    }
}
```

**Why Generics Here?**
- MongoDB driver provides generic collections
- Type-safe queries without casting
- IntelliSense shows correct properties for each entity

### When Generics Are Better Than Non-Generic

| Scenario | Non-Generic | Generic |
|----------|-------------|---------|
| Code Reuse | Duplicate code per entity | Single interface |
| Type Safety | Runtime errors possible | Compile-time safety |
| Casting | `var patient = (Patient)obj` | `Patient patient = repo.GetById()` |
| Performance | Boxing/unboxing for value types | No boxing |

---

## SOLID Principles

### 1. Single Responsibility Principle (SRP)

**Statement:** A class should have one and only one reason to change.

**Where Used:**

| Class | Single Responsibility |
|-------|----------------------|
| `PatientRepository` | Only data access for patients |
| `PatientService` | Only business logic for patients |
| `BillingService` | Only billing operations |
| `ConsoleUI` | Only user interaction |
| `UpiPaymentProcessor` | Only payment processing |

**Why Important:**
- If MongoDB changes, only repositories change
- If billing logic changes, only BillingService changes
- If UI changes, only ConsoleUI changes
- No cascading changes across the application

**Example:**
```csharp
// ❌ Violation - Repository doing business logic
public class PatientRepository
{
    public async Task CreatePatient(...)
    {
        // Business logic mixed with data access
        if (string.IsNullOrEmpty(name)) throw new Exception();
        // ...
    }
}

// ✅ Correct - Separation of concerns
public class PatientRepository : IRepository<Patient> { }
public class PatientService : IPatientService { }
```

---

### 2. Open/Closed Principle (OCP)

**Statement:** Software entities should be open for extension but closed for modification.

**Where Used:**

#### Adding New Payment Processors

```csharp
public interface IPaymentProcessor
{
    bool ProcessPayment(Patient patient, string paymentDetail, decimal amount);
}

// New payment method - no existing code changed
public class CreditCardProcessor : IPaymentProcessor
{
    public bool ProcessPayment(Patient patient, string paymentDetail, decimal amount)
    {
        // Credit card logic
    }
}

// BillingService works with any processor
public class BillingService : IBillingService
{
    private readonly IPaymentProcessor _processor;  // Works with UpiPaymentProcessor or CreditCardProcessor
}
```

**Why Important:**
- Add new payment methods without changing BillingService
- Add new repositories without changing services
- Add new UI screens without changing business logic

---

### 3. Liskov Substitution Principle (LSP)

**Statement:** Objects of a superclass should be replaceable with objects of a subclass without affecting correctness.

**Where Used:**

All repository implementations can substitute their interfaces:

```csharp
// Any repository implementation works with IPatientRepository
IPatientRepository repo1 = new PatientRepository(context);
IPatientRepository repo2 = new PatientRepository(anotherContext);

// Both work identically in services
var service = new PatientService(repo1, productRepo);
```

**Why Important:**
- Services don't care which repository implementation they get
- Easy to swap in mock repositories for testing
- Easy to swap in different database implementations

**Testability:**
```csharp
// In tests, use mock repository
var mockRepo = new Mock<IPatientRepository>();
var service = new PatientService(mockRepo.Object, productRepo);
```

---

### 4. Interface Segregation Principle (ISP)

**Statement:** Clients should not be forced to depend on methods they do not use.

**Where Used:**

#### Generic Base Interface (ISP)
```csharp
// Small, focused interface
public interface IRepository<T>
{
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
}

// Domain-specific extensions
public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> SearchAsync(string identifier);  // Only for patients
}
```

**Why Important:**
- ProductRepository doesn't need SearchAsync
- PatientRepository doesn't need GetByProductIdAsync
- Each interface has only what it needs

#### Separated Service Interfaces
```csharp
// IPatientService - Only patient operations
public interface IPatientService
{
    Task<Patient> CreateAsync(...);
    Task<bool> AddMedicineAsync(...);
}

// IBillingService - Only billing operations
public interface IBillingService
{
    Task<Patient?> GetBillDetailsAsync(...);
    Task<bool> SettleBillAsync(...);
}
```

---

### 5. Dependency Inversion Principle (DIP)

**Statement:** High-level modules should not depend on low-level modules. Both should depend on abstractions.

**Where Used:**

#### Service Layer Depends on Interfaces

```csharp
// ❌ Violation - High-level depends on low-level
public class PatientService
{
    private readonly PatientRepository _repository;  // Concrete class
    public PatientService() { _repository = new PatientRepository(); }
}

// ✅ Correct - Both depend on abstraction
public class PatientService
{
    private readonly IPatientRepository _repository;  // Interface
    public PatientService(IPatientRepository repository) { }
}
```

#### Dependency Injection Flow

```
Program.cs (High-level)
    ↓ depends on
IPatientRepository (Abstraction)
    ↓ implemented by
PatientRepository (Low-level)
    ↓ uses
MongoDB.Driver (Low-level)
```

**Benefits:**
- Easy to test with mock repositories
- Easy to swap implementations
- Loose coupling between layers

---

## Summary

| Concept | Location | Benefit |
|---------|----------|---------|
| **Generics** | `IRepository<T>` | Code reuse, type safety |
| **SRP** | Separated classes | Single reason to change |
| **OCP** | Interface extensions | Open for new features |
| **LSP** | Repository implementations | Swappable implementations |
| **ISP** | Fine-grained interfaces | Only needed methods |
| **DIP** | Constructor injection | Loose coupling, testable |