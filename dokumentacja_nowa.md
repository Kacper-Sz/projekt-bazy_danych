# Dokumentacja Projektu - Sklep Komputerowy

## Spis Treści
1. [Wprowadzenie](#1-wprowadzenie)
2. [Stos Technologiczny](#2-stos-technologiczny)
3. [Architektura Systemu](#3-architektura-systemu)
4. [Baza Danych MongoDB](#4-baza-danych-mongodb)
5. [Biblioteka DLL - Warstwa Logiki](#5-biblioteka-dll---warstwa-logiki)
6. [Testy Jednostkowe](#6-testy-jednostkowe)
7. [Aplikacja MAUI - Interfejs Użytkownika](#7-aplikacja-maui---interfejs-użytkownika)
8. [Konkretne Funkcjonalności](#8-konkretne-funkcjonalności)
9. [Docker i Automatyzacja](#9-docker-i-automatyzacja)
10. [Problemy i Rozwiązania](#10-problemy-i-rozwiązania)
11. [Podsumowanie](#11-podsumowanie)

---

## 1. Wprowadzenie

### 1.1 Przedmiot Projektu
Projekt stanowi w pełni funkcjonalny sklep internetowy specjalizujący się w sprzedaży części komputerowych. System został zaprojektowany z myślą o elastycznym zarządzaniu produktami o zróżnicowanych specyfikacjach technicznych, kompleksowej obsłudze kont użytkowników oraz efektywnym procesowaniu zamówień.

### 1.2 Cel Biznesowy
Aplikacja ma umożliwić:
- Przeglądanie i wyszukiwanie produktów z zaawansowanym filtrowaniem
- Zarządzanie kontem użytkownika z systemem uwierzytelniania
- Składanie zamówień z automatyczną kontrolą stanów magazynowych
- Administrację produktami przez uprawnionych użytkowników

### 1.3 Screenshoty Aplikacji

**[MIEJSCE NA SCREENSHOT - Ekran główny aplikacji]**

**[MIEJSCE NA SCREENSHOT - Lista produktów z filtrowaniem]**

**[MIEJSCE NA SCREENSHOT - Koszyk zakupowy]**

**[MIEJSCE NA SCREENSHOT - Panel użytkownika]**

---

## 2. Stos Technologiczny

### 2.1 Backend i Logika Biznesowa
- **C# (.NET)** - podstawowy język programowania
- **MongoDB Driver dla .NET** - oficjalny sterownik do komunikacji z bazą danych
- **MongoDB.Bson** - obsługa formatów dokumentów

### 2.2 Baza Danych
- **MongoDB 7.0** - dokumentowa baza danych NoSQL
- **Docker** - konteneryzacja bazy danych
- **JavaScript** - skrypty inicjalizacyjne bazy danych

### 2.3 Przechowywanie Multimediów
- **Cloudinary** - chmurowe rozwiązanie do przechowywania i serwowania zdjęć produktów
- **CloudinaryDotNet** - oficjalna biblioteka .NET do integracji

### 2.4 Interfejs Użytkownika
- **.NET MAUI** - wieloplatformowy framework UI
- **XAML** - język definiowania interfejsu
- **MVVM Pattern** - architektura aplikacji

### 2.5 Testowanie
- **xUnit** - framework do testów jednostkowych
- **Moq** (opcjonalnie) - mockowanie zależności

### 2.6 Infrastruktura
- **Docker Compose** - orkiestracja kontenerów
- **Git** - system kontroli wersji

---

## 3. Architektura Systemu

### 3.1 Architektura Warstwowa
Projekt został podzielony na trzy niezależne moduły zgodnie z zasadą separacji odpowiedzialności:

```
┌─────────────────────────────────────┐
│     App (MAUI) - Warstwa GUI        │
│  - ViewModels (MVVM)                │
│  - Views (XAML)                     │
│  - Wrappers                         │
└──────────────┬──────────────────────┘
               │ (referencja)
               ▼
┌─────────────────────────────────────┐
│   DLL - Warstwa Logiki Biznesowej   │
│  - Managers (Product, User, Order)  │
│  - Models                           │
│  - Filtry i Sortowanie              │
│  - Komunikacja z MongoDB            │
└──────────────┬──────────────────────┘
               │ (połączenie)
               ▼
┌─────────────────────────────────────┐
│        MongoDB w Dockerze           │
│  - Kolekcja Products                │
│  - Kolekcja Users                   │
│  - Kolekcja Orders                  │
└─────────────────────────────────────┘

        ┌──────────────────┐
        │  Tests (xUnit)   │ ──► testuje DLL
        └──────────────────┘
```

### 3.2 Moduł DLL (Biblioteka Klas)
**Lokalizacja:** `dll/dll/`

Moduł stanowiący rdzeń aplikacji, zawiera:
- **DataManager** - zarządzanie konfiguracją połączenia
- **MongoDbManager** - inicjalizacja połączenia z bazą danych
- **ShoppingCart** - logika koszyka zakupowego
- **Managers:**
  - `ProductManager` - operacje CRUD na produktach
  - `UserManager` - rejestracja i logowanie
  - `OrderManager` - tworzenie i zarządzanie zamówieniami
- **Models:** definicje struktur danych (Product, User, Order)
- **Extensions:** rozszerzenia dla filtrowania i sortowania

### 3.3 Moduł Tests
**Lokalizacja:** `dll/Tests/`

Kompleksowe testy jednostkowe wszystkich metod z biblioteki DLL:
- `ProductManagerTest.cs` - 496 linii testów produktów
- `OrderManagerTests.cs` - testy zarządzania zamówieniami
- `UserManagerTests.cs` - testy uwierzytelniania
- `ShoppingCartTest.cs` - testy koszyka

### 3.4 Moduł App (MAUI)
**Lokalizacja:** `App/App/App/`

Wieloplatformowa aplikacja GUI w architekturze MVVM:
- **ViewModels** - logika prezentacji
- **Views** - interfejs użytkownika (XAML)
- **Wrappers** - adaptory dla modeli DLL
- **Sessions** - zarządzanie sesją użytkownika

---

## 4. Baza Danych MongoDB

### 4.1 Struktura Bazy Danych
Nazwa bazy: **`shop`**

### 4.2 Kolekcja Products

**Opis:** Przechowuje informacje o produktach dostępnych w sklepie.

**Struktura dokumentu:**
```csharp
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }               // Nazwa produktu

    [BsonElement("manufacturer")]
    public string Manufacturer { get; set; }       // Producent

    [BsonElement("category")]
    public string Category { get; set; }           // Kategoria

    [BsonElement("price")]
    public decimal Price { get; set; }             // Cena

    [BsonElement("stock")]
    public int Stock { get; set; }                 // Stan magazynowy

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; }           // URL do zdjęcia

    [BsonElement("specs")]
    public Dictionary<string, string> Specs { get; set; }  // Specyfikacje techniczne

    [BsonElement("description")]
    public string Description { get; set; }        // Opis produktu

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }        // Data dodania
}
```

**Przykładowy dokument w bazie:**
```json
{
  "_id": ObjectId("6931a62442ebb44d99ce5f49"),
  "name": "PlayStation 5",
  "manufacturer": "Sony",
  "category": "Gaming",
  "price": NumberDecimal("2499.99"),
  "stock": 15,
  "imageUrl": "https://res.cloudinary.com/dv1nk0kbi/image/upload/...",
  "specs": {
    "CPU": "AMD Zen 2",
    "GPU": "AMD RDNA 2",
    "RAM": "16GB GDDR6"
  },
  "description": "Next-gen gaming console",
  "createdAt": ISODate("2025-12-01T00:00:00Z")
}
```

**Decyzje projektowe:**
- **imageUrl zamiast BLOB** - przechowywanie URL zamiast binarnych danych zdjęć znacząco optymalizuje wydajność bazy i zmniejsza rozmiar dokumentów
- **Dictionary<string, string> Specs** - elastyczna struktura pozwalająca na różne specyfikacje dla różnych kategorii produktów (np. CPU ma inne parametry niż monitor)

### 4.3 Kolekcja Users

**Opis:** Zarządza kontami użytkowników i uwierzytelnianiem.

**Struktura dokumentu:**
```csharp
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; }          // Imię

    [BsonElement("lastName")]
    public string LastName { get; set; }           // Nazwisko

    [BsonElement("email")]
    public string Email { get; set; }              // Email (unikalny)

    [BsonElement("role")]
    public string Role { get; set; }               // Rola (customer/admin)

    [BsonElement("password")]
    public string Password { get; set; }           // Hasło

    [BsonElement("phoneNumber")]
    public string PhoneNumber { get; set; }        // Numer telefonu

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }        // Data rejestracji
}
```

**Walidacja haseł:**
- Minimalna długość: 8 znaków
- Wymagana wielka litera
- Wymagana mała litera
- Wymagana cyfra
- Wymagany znak specjalny

**Walidacja emaila:** Regex: `^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$`

### 4.4 Kolekcja Orders

**Opis:** Rejestruje wszystkie zamówienia złożone przez użytkowników.

**Struktura dokumentu:**
```csharp
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("customerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId CustomerId { get; set; }       // Referencja do User

    [BsonElement("items")]
    public List<OrderItem> Items { get; set; }     // Lista produktów

    [BsonElement("status")]
    public string Status { get; set; }             // Status zamówienia

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }        // Data złożenia

    [BsonElement("totalAmount")]
    public decimal TotalAmount { get; set; }       // Całkowita kwota

    [BsonElement("deliveryAddress")]
    public DeliveryAddress DeliveryAddress { get; set; }  // Adres dostawy

    [BsonElement("paymentMethod")]
    public string PaymentMethod { get; set; }      // Metoda płatności
}

public class OrderItem
{
    [BsonElement("productId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ProductId { get; set; }        // Referencja do Product

    [BsonElement("quantity")]
    public int Quantity { get; set; }              // Ilość
}

public class DeliveryAddress
{
    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("street")]
    public string Street { get; set; }

    [BsonElement("postalCode")]
    public string PostalCode { get; set; }
}
```

**Relacje:**
- `customerId` łączy zamówienie z kontem użytkownika
- `OrderItem.productId` łączy pozycję zamówienia z konkretnym produktem
- Dokumentowa natura bazy pozwala na zagnieżdżenie `items` i `deliveryAddress` bezpośrednio w zamówieniu

---

## 5. Biblioteka DLL - Warstwa Logiki

### 5.1 Konfiguracja Połączenia

**DataManager.cs:**
```csharp
public static class DataManager
{
    private static string connectionString = 
        "mongodb://root:password@localhost:1500/?authSource=admin";
    private static string databaseName = "shop";

    public static string ConnectionString() => connectionString;
    public static string DatabaseName() => databaseName;
    public static void SetConnectionString(string connectionString)
        => DataManager.connectionString = connectionString;
}
```

Centralizacja konfiguracji umożliwia łatwą zmianę parametrów połączenia.

### 5.2 MongoDbManager

**MongoDbManager.cs:**
```csharp
public class MongoDbManager
{
    private readonly IMongoDatabase _database;

    public MongoDbManager(string connectionString, string databaseName)
    {
        MongoClient client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public MongoDbManager() 
        : this(DataManager.ConnectionString(), DataManager.DatabaseName()) { }

    public IMongoDatabase Database => _database;
}
```

Singleton pattern zapewniający jedno połączenie do bazy danych.

### 5.3 ProductManager

**Główne metody:**

#### Dodawanie produktu z automatycznym uploadem zdjęcia
```csharp
public async Task AddPhotoProductAsync(Product product, string imagePath)
{
    // Upload do Cloudinary
    ImageUploadParams uploadParams = new ImageUploadParams()
    {
        File = new FileDescription(imagePath)
    };

    ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        throw new Exception("Error occurred while uploading photo");

    product.ImageUrl = uploadResult.SecureUrl.ToString();
    product.CreatedAt = DateTime.Now;

    if (!ValidateProductData(product))
        throw new Exception("Invalid product data");

    await _products.InsertOneAsync(product);
}
```

#### Walidacja danych produktu
```csharp
private bool ValidateProductData(Product product)
    => product != null
    && !string.IsNullOrEmpty(product.Name)
    && !string.IsNullOrEmpty(product.Manufacturer)
    && !string.IsNullOrEmpty(product.Category)
    && product.Price > 0
    && !string.IsNullOrEmpty(product.ImageUrl)
    && product.Specs != null && product.Specs.All(spec => 
        !string.IsNullOrWhiteSpace(spec.Key) && 
        !string.IsNullOrWhiteSpace(spec.Value))
    && !string.IsNullOrEmpty(product.Description)
    && product.CreatedAt != default;
```

#### Pobieranie produktu po ID
```csharp
public async Task<Product?> GetProductByIdAsync(string id)
{
    ObjectId productId = new ObjectId(id);
    return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
}
```

#### Usuwanie produktu (tylko admin)
```csharp
public async Task DeleteProductAsync(string id, string userRole)
{
    if (userRole != "admin")
        throw new Exception("not admin");

    Product? item = await GetProductByIdAsync(id);
    if (item == null)
        throw new Exception("item null");

    // Usunięcie zdjęcia z Cloudinary
    string publicId = Path.GetFileNameWithoutExtension(item.ImageUrl);
    DeletionParams deletionParams = new DeletionParams(publicId);
    DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

    if (deletionResult.Result != "ok")
        throw new Exception("Deletion failed");

    // Usunięcie produktu z bazy
    await _products.DeleteOneAsync(p => p.Id == new ObjectId(id));
}
```

#### Pobieranie unikalnych producentów
```csharp
public async Task<List<string>> GetDistinctManufacturersAsync()
{
    FilterDefinition<Product> filter = Builders<Product>.Filter.Gte(p => p.Stock, 1);
    return await _products.Distinct<string>("manufacturer", filter).ToListAsync();
}
```

### 5.4 ProductFilter - Extension Methods

**ProductFilter.cs:**
```csharp
public static class ProductFilter
{
    public static List<Product> FilterByCategory(
        this List<Product> products, string category) =>
        products.Where(p => p.Category == category).ToList();

    public static List<Product> FilterByManufacturer(
        this List<Product> products, string manufacturer) =>
        products.Where(p => p.Manufacturer == manufacturer).ToList();

    public static List<Product> FilterByPriceRange(
        this List<Product> products, decimal minPrice, decimal maxPrice) =>
        products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

    public static List<Product> FilterByStock(
        this List<Product> products, int minStock) =>
        products.Where(p => p.Stock >= minStock).ToList();

    public static List<Product> SearchByName(
        this List<Product> products, string searchTerm) =>
        products.Where(p =>
            (!string.IsNullOrEmpty(p.Name) && 
             p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Description) && 
             p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Category) && 
             p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Manufacturer) && 
             p.Manufacturer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
            (p.Specs != null && p.Specs.Values.Any(v => 
             v != null && v.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
        ).ToList();
}
```

**Zalety Extension Methods:**
- Możliwość łączenia filtrów (fluent API)
- Czytelny kod: `products.FilterByCategory("GPU").FilterByPriceRange(1000, 5000)`
- Łatwość testowania

### 5.5 ProductSort - Sortowanie

**ProductSort.cs:**
```csharp
public static class ProductSort
{
    public static List<Product> Sort(
        this List<Product> products, 
        ProductSortEnum sortBy)
    {
        return sortBy switch
        {
            ProductSortEnum.PRICE_ASC => 
                products.OrderBy(p => p.Price).ToList(),
            ProductSortEnum.PRICE_DESC => 
                products.OrderByDescending(p => p.Price).ToList(),
            ProductSortEnum.NAME_ASC => 
                products.OrderBy(p => p.Name).ToList(),
            ProductSortEnum.NAME_DESC => 
                products.OrderByDescending(p => p.Name).ToList(),
            ProductSortEnum.DATE_ASC => 
                products.OrderBy(p => p.CreatedAt).ToList(),
            ProductSortEnum.DATE_DESC => 
                products.OrderByDescending(p => p.CreatedAt).ToList(),
            ProductSortEnum.STOCK_ASC => 
                products.OrderBy(p => p.Stock).ToList(),
            ProductSortEnum.STOCK_DESC => 
                products.OrderByDescending(p => p.Stock).ToList(),
            _ => products
        };
    }
}
```

### 5.6 UserManager

**Rejestracja z walidacją:**
```csharp
public async Task<UserRejestrationEnum> RegisterUserAsync(User user)
{
    if (!ValidateUserData(user))
        return UserRejestrationEnum.UNCOMPLETED_DATA;
        
    User existingUser = await _users.Find(u => u.Email == user.Email)
                                     .FirstOrDefaultAsync();
    if (existingUser != null)
        return UserRejestrationEnum.EMAIL_ALREADY_EXISTS;
    
    var passwordValidationResult = ValidatePasswordDetailed(user.Password);
    if (passwordValidationResult != UserRejestrationEnum.GOOD)
        return passwordValidationResult;
    
    if (!ValidateEmailFormat(user.Email))
        return UserRejestrationEnum.INVALID_EMAIL_FORMAT;
        
    await _users.InsertOneAsync(user);
    return UserRejestrationEnum.GOOD;
}
```

**Szczegółowa walidacja hasła:**
```csharp
private UserRejestrationEnum ValidatePasswordDetailed(string password)
{
    if (password.Length < MIN_PASSWORD_LENGTH)
        return UserRejestrationEnum.PASSWORD_TOO_SHORT;
    
    if (!password.Any(char.IsUpper))
        return UserRejestrationEnum.PASSWORD_MISSING_UPPERCASE;
    
    if (!password.Any(char.IsLower))
        return UserRejestrationEnum.PASSWORD_MISSING_LOWERCASE;
    
    if (!password.Any(char.IsDigit))
        return UserRejestrationEnum.PASSWORD_MISSING_DIGIT;
    
    if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        return UserRejestrationEnum.PASSWORD_MISSING_SPECIAL_CHAR;
    
    return UserRejestrationEnum.GOOD;
}
```

**Logowanie:**
```csharp
public async Task<User?> LoginAsync(string email, string password)
    => await _users.Find(u => u.Email == email && u.Password == password)
                   .FirstOrDefaultAsync();
```

### 5.7 OrderManager

**Tworzenie zamówienia:**
```csharp
public async Task<OrderCreationEnum> CreateOrderAsync(Order order)
{
    if (!ValidateOrderData(order))
        return OrderCreationEnum.INVALID_DATA;
        
    await _orders.InsertOneAsync(order);
    return OrderCreationEnum.SUCCESS;
}

private bool ValidateOrderData(Order order)
    => order.CustomerId != ObjectId.Empty
    && order.Items != null && order.Items.Count > 0
    && !string.IsNullOrEmpty(order.Status)
    && order.CreatedAt != default
    && order.DeliveryAddress != null 
    && !string.IsNullOrEmpty(order.DeliveryAddress.City) 
    && !string.IsNullOrEmpty(order.DeliveryAddress.Street) 
    && !string.IsNullOrEmpty(order.DeliveryAddress.PostalCode)
    && !string.IsNullOrEmpty(order.PaymentMethod);
```

**Pobieranie zamówień użytkownika:**
```csharp
public async Task<List<Order>> GetOrdersByCustomerAsync(string customerId) 
{
    ObjectId objectId = new ObjectId(customerId);
    return await _orders.Find(o => o.CustomerId == objectId).ToListAsync();
}
```

**Aktualizacja statusu:**
```csharp
public async Task UpdateOrderStatusAsync(string id, string newStatus)
{
    UpdateDefinition<Order> update = Builders<Order>.Update.Set(o => o.Status, newStatus);
    ObjectId objectId = new ObjectId(id);
    await _orders.UpdateOneAsync(o => o.Id == objectId, update);
}
```

### 5.8 ShoppingCart

**Dodawanie produktu do koszyka:**
```csharp
public void AddItem(Product product, int quantity)
{
    if (quantity <= 0)
        throw new Exception("Quantity must be greater than zero");

    if (quantity > product.Stock)
        throw new Exception($"Not enough in stock, max: {product.Stock}");

    products[product] = products.TryGetValue(product, out int existingQuantity)
        ? existingQuantity + quantity
        : quantity;

    totalAmount = products.Sum(p => p.Key.Price * p.Value);
}
```

**Automatyczna aktualizacja stanów magazynowych:**
```csharp
private async Task<bool> UpdateProductStock(Product product, int quantity)
{
    ObjectId productId = product.Id;
    
    FilterDefinition<Product> filter = Builders<Product>.Filter.And(
        Builders<Product>.Filter.Eq(p => p.Id, productId),
        Builders<Product>.Filter.Gte(p => p.Stock, quantity)
    );

    UpdateDefinition<Product> update = 
        Builders<Product>.Update.Inc(p => p.Stock, -quantity);

    UpdateResult result = await _products.UpdateOneAsync(filter, update);
    
    return result.ModifiedCount > 0;
}
```

---

## 6. Testy Jednostkowe

### 6.1 Framework i Struktura
Wykorzystano **xUnit** do kompleksowego testowania wszystkich metod biblioteki DLL.

**Statystyki testów:**
- `ProductManagerTest.cs` - 496 linii
- `OrderManagerTests.cs`
- `UserManagerTests.cs`
- `ShoppingCartTest.cs`
- `OrderListExtensionTest.cs`

### 6.2 Przykładowe Testy ProductManager

**Test pobierania produktu po ID:**
```csharp
[Fact]
public async Task GetProductByIdAsyncTest1()
{
    string productId = "6931a62442ebb44d99ce5f49";

    Product? product = await productManager.GetProductByIdAsync(productId);

    Assert.NotNull(product);
    Assert.Equal("PlayStation 5", product.Name);
}

[Fact]
public async Task GetProductByIdAsyncTest2()
{
    string productId = "6931a62442ebb44d99ce5f47";

    Product? product = await productManager.GetProductByIdAsync(productId);

    Assert.NotNull(product);
    Assert.Equal("Dell XPS 13 Laptop", product.Name);
}

[Fact]
public async Task GetProductByIdAsyncTest4()
{
    string productId = "6931ad234000044d99ce5f50"; // Nieprawidłowe ID

    Product? product = await productManager.GetProductByIdAsync(productId);

    Assert.Null(product);
}
```

### 6.3 Testy Filtrowania

```csharp
private readonly List<Product> productsList = new List<Product>
{
    new Product { 
        Name = "Zebra", 
        Category = "Audio", 
        Manufacturer = "Sony", 
        Stock = 10, 
        Price = 2000, 
        CreatedAt = new DateTime(2025, 12, 1) 
    },
    new Product { 
        Name = "Apple", 
        Category = "Smartphone", 
        Manufacturer = "Apple", 
        Stock = 8, 
        Price = 3849, 
        CreatedAt = new DateTime(2025, 11, 15) 
    },
    new Product { 
        Name = "Mango", 
        Category = "Monitor", 
        Manufacturer = "Samsung", 
        Stock = 3, 
        Price = 3850, 
        CreatedAt = new DateTime(2025, 10, 20) 
    }
};
```

### 6.4 Pokrycie Testami
Testy zapewniają:
- Weryfikację poprawności operacji CRUD
- Testowanie przypadków brzegowych
- Walidację danych wejściowych
- Sprawdzanie obsługi błędów
- Testowanie filtrów i sortowania

---

## 7. Aplikacja MAUI - Interfejs Użytkownika

### 7.1 Architektura MVVM
Aplikacja wykorzystuje wzorzec **Model-View-ViewModel**:

```
Views (XAML) ←→ ViewModels ←→ DLL (Models + Managers)
```

### 7.2 Kluczowe ViewModels

**LoginViewModel:**
- Logowanie użytkowników
- Przekierowanie do rejestracji
- Walidacja danych logowania

**AccountViewModel:**
```csharp
class AccountViewModel : BaseViewModel
{
    private ObservableCollection<OrderVM> orders;
    public ObservableCollection<OrderVM> Orders { get; set; }
    
    public RelayCommand LogoutCommand { get; set; }
    
    public AccountViewModel()
    {
        Orders = new ObservableCollection<OrderVM>();
        LogoutCommand = new RelayCommand(LogoutAsync);
    }
}
```

### 7.3 BaseViewModel
```csharp
class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
```

Implementacja **INotifyPropertyChanged** zapewnia automatyczne odświeżanie interfejsu przy zmianie danych.

### 7.4 Session Management
**Session.cs** - zarządzanie zalogowanym użytkownikiem:
- Przechowywanie danych użytkownika
- Sprawdzanie uprawnień (customer/admin)
- Wylogowywanie

---

## 8. Konkretne Funkcjonalności

### 8.1 Filtrowanie Produktów

**[MIEJSCE NA SCREENSHOT - Filtrowanie po kategorii]**

**Kod implementacji:**
```csharp
// W ViewModelu
var allProducts = await productManager.GetAllProductsAsync();
var filtered = allProducts
    .FilterByCategory(selectedCategory)
    .FilterByManufacturer(selectedManufacturer)
    .FilterByPriceRange(minPrice, maxPrice);
```

**Demonstracja:**
1. Pobranie wszystkich produktów z bazy
2. Zastosowanie filtra kategorii (np. "GPU")
3. Zastosowanie filtra producenta (np. "NVIDIA")
4. Zastosowanie zakresu cenowego (np. 1000-5000 PLN)

**Wynik:** Lista zawężona do GPU-ów NVIDIA w przedziale 1000-5000 PLN.

### 8.2 Sortowanie Produktów

**[MIEJSCE NA SCREENSHOT - Sortowanie po cenie]**

**Kod implementacji:**
```csharp
var sortedProducts = filteredProducts.Sort(ProductSortEnum.PRICE_ASC);
```

**Dostępne opcje sortowania:**
- Cena rosnąco/malejąco
- Nazwa alfabetycznie A-Z/Z-A
- Data dodania (najnowsze/najstarsze)
- Stan magazynowy (najwięcej/najmniej)

### 8.3 Dodawanie Produktu (Admin)

**[MIEJSCE NA SCREENSHOT - Panel dodawania produktu]**

**Proces:**
1. Wybór zdjęcia z dysku
2. Upload do Cloudinary
3. Wypełnienie formularza (nazwa, producent, kategoria, cena, stan, specyfikacje)
4. Walidacja danych
5. Zapis do MongoDB

**Kod:**
```csharp
await productManager.AddPhotoProductAsync(newProduct, imageFilePath);
```

**Automatyzacja:**
- Zdjęcie uploadowane do Cloudinary
- Zwrócony URL zapisywany w bazie
- Data `createdAt` ustawiana automatycznie

### 8.4 Proces Zakupowy

**[MIEJSCE NA SCREENSHOT - Koszyk z produktami]**

**Kroki:**
1. **Przeglądanie produktów** - użytkownik przegląda katalog
2. **Dodanie do koszyka:**
   ```csharp
   shoppingCart.AddItem(selectedProduct, quantity);
   ```
3. **Walidacja stanu:**
   - System sprawdza czy wystarczająca ilość w magazynie
   - Wyświetla błąd jeśli `quantity > product.Stock`
4. **Finalizacja zamówienia:**
   ```csharp
   var order = shoppingCart.CreateOrder(
       user, 
       deliveryAddress, 
       paymentMethod
   );
   await orderManager.CreateOrderAsync(order);
   ```
5. **Aktualizacja stanów:**
   ```csharp
   foreach (var item in order.Items)
   {
       await productManager.UpdateStockAsync(item.ProductId, -item.Quantity);
   }
   ```

### 8.5 System Rejestracji

**[MIEJSCE NA SCREENSHOT - Formularz rejestracji]**

**Walidacja w czasie rzeczywistym:**
- Hasło za krótkie
- Brak wielkiej litery
- Email zajęty
- Wszystko OK

**Kod:**
```csharp
var result = await userManager.RegisterUserAsync(newUser);

switch (result)
{
    case UserRejestrationEnum.GOOD:
        // Sukces
        break;
    case UserRejestrationEnum.EMAIL_ALREADY_EXISTS:
        // Wyświetl błąd
        break;
    case UserRejestrationEnum.PASSWORD_TOO_SHORT:
        // Wyświetl błąd
        break;
    // ... inne przypadki
}
```

### 8.6 Wyszukiwanie Pełnotekstowe

**Kod:**
```csharp
var searchResults = allProducts.SearchByName(searchQuery);
```

**Przeszukiwane pola:**
- Nazwa produktu
- Opis
- Kategoria
- Producent
- Wartości w specyfikacjach technicznych

**Przykład:** Wyszukiwanie "16GB" znajdzie produkty z 16GB RAM w specyfikacjach.

---

## 9. Docker i Automatyzacja

### 9.1 Docker Compose

**docker-compose.yml:**
```yaml
services:
  mongo:
    image: mongo:latest
    container_name: projectShop
    restart: unless-stopped
    ports:
      - "1500:27017"
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: password
      MONGO_INITDB_DATABASE: shop
    volumes:
      - mongo-data:/data/db
      - ./initdb:/docker-entrypoint-initdb.d:ro

volumes:
  mongo-data:
```

**Parametry:**
- **Port 1500** - niestandardowy port (zamiast domyślnego 27017) unika konfliktów
- **Wolumen persistent** - dane zachowywane między restartami kontenera
- **Initdb volume** - automatyczne wykonanie skryptów inicjalizacyjnych

### 9.2 Skrypt Inicjalizacyjny

**initCollections.js:**
```javascript
db = db.getSiblingDB('shop');

// Tworzenie kolekcji
db.createCollection('users');
db.createCollection('orders');
db.createCollection('products');

const fs = require('fs');

function readJSON(path) {
    return JSON.parse(fs.readFileSync(path, 'utf8'));
}

// Wczytanie danych
const products = readJSON('/docker-entrypoint-initdb.d/Products.json');
const users    = readJSON('/docker-entrypoint-initdb.d/Users.json');
const orders   = readJSON('/docker-entrypoint-initdb.d/Orders.json');

// Konwersja typów
users.forEach(u => {
    u._id = ObjectId(u._id);
    u.createdAt = new Date(u.createdAt);
});

products.forEach(p => {
    p._id = ObjectId(p._id);
    p.createdAt = new Date(p.createdAt);
    p.price = NumberDecimal(p.price.toString());
});

orders.forEach(o => {
    o._id = ObjectId(o._id);
    o.customerId = ObjectId(o.customerId);
    o.createdAt = new Date(o.createdAt);
    o.items.forEach(i => i.productId = ObjectId(i.productId));
    
    if (o.totalAmount !== undefined && o.totalAmount !== null) {
        const amountString = 
            typeof o.totalAmount === "object" && o.totalAmount.$numberDecimal
                ? o.totalAmount.$numberDecimal
                : o.totalAmount.toString();
        o.totalAmount = NumberDecimal(amountString);
    }
});

// Inserting
db.products.insertMany(products);
db.users.insertMany(users);
db.orders.insertMany(orders);

print("Database seeded successfully!");
```

### 9.3 Uruchomienie Systemu

**Krok 1 - Start bazy danych:**
```bash
cd database
docker-compose up -d
```

**Krok 2 - Weryfikacja:**
```bash
docker ps
# Powinien być widoczny kontener "projectShop"

docker logs projectShop
# Powinno być: "Database seeded successfully!"
```

**Krok 3 - Uruchomienie aplikacji:**
```bash
cd App
dotnet build
dotnet run
```

### 9.4 Zalety Automatyzacji

 **Reprodukowalność** - każdy developer może uruchomić identyczne środowisko  
 **Szybkość** - baza gotowa w kilkadziesiąt sekund  
 **Spójność danych** - dane testowe zawsze takie same  
 **Brak konfiguracji manualnej** - zero ręcznych kroków w MongoDB  
 **Łatwość resetowania** - `docker-compose down -v && docker-compose up -d`

---

## 10. Problemy i Rozwiązania

### 10.1 Problem: Przechowywanie Zdjęć

**Rozważane opcje:**
1. GridFS MongoDB
2. Przechowywanie jako Base64 w dokumentach
3. System plików na serwerze
4. Zewnętrzne CDN

**Wybrane rozwiązanie: Cloudinary**

**Uzasadnienie:**
- Optymalizacja wydajności - zdjęcia nie obciążają bazy danych
- Automatyczna transformacja obrazów (resize, crop, format)
- CDN - szybkie ładowanie z serwerów edge
- Łatwość zarządzania - API do upload/delete
- Free tier wystarczający dla projektu edukacyjnego

**Implementacja:**
```csharp
private readonly Cloudinary _cloudinary;

public ProductManager()
{
    Account account = new Account(
        "dv1nk0kbi",
        "989932878854628",
        "U0TsweDyygH_mYnfGelE-I_MIzI"
    );
    _cloudinary = new Cloudinary(account);
    _cloudinary.Api.Secure = true;
}
```

### 10.2 Problem: Typy Decimal w MongoDB

**Problem:** MongoDB nie ma natywnego typu `decimal` dla precyzyjnych wartości finansowych.

**Rozwiązanie:** Użycie `NumberDecimal` (BSON type 128-bit decimal):
```javascript
p.price = NumberDecimal(p.price.toString());
o.totalAmount = NumberDecimal(amountString);
```

**W C#:**
```csharp
[BsonElement("price")]
public decimal Price { get; set; }  // Automatycznie mapowane na NumberDecimal
```

### 10.3 Problem: ObjectId String Conversion

**Problem:** MongoDB używa `ObjectId`, C# string.

**Rozwiązanie:**
```csharp
[BsonId]
[BsonRepresentation(BsonType.ObjectId)]
public ObjectId Id { get; set; }

// Konwersja w kodzie
ObjectId productId = new ObjectId(stringId);
```

### 10.4 Problem: Brak Transakcji ACID

**Problem:** MongoDB nie gwarantuje transakcji atomowych na poziomie wielu dokumentów (w wersji standalone).

**Rozwiązanie:** Careful ordering operations:
```csharp
// Najpierw sprawdź stan
var product = await GetProductByIdAsync(productId);
if (product.Stock < quantity)
    throw new Exception("Not enough stock");

// Potem aktualizuj z dodatkową walidacją
FilterDefinition<Product> filter = Builders<Product>.Filter.And(
    Builders<Product>.Filter.Eq(p => p.Id, productId),
    Builders<Product>.Filter.Gte(p => p.Stock, quantity)  // Double-check
);

UpdateResult result = await _products.UpdateOneAsync(filter, update);
if (result.ModifiedCount == 0)
    throw new Exception("Stock changed, retry");
```

**Alternatywa:** MongoDB Replica Set + Transactions (dla produkcji).



---

## 11. Podsumowanie


 **Funkcjonalny sklep internetowy** z pełnym procesem zakupowym  
 **Architektura warstwowa** - separacja GUI, logiki biznesowej i danych  
 **Dokumentowa baza danych** - elastyczna struktura dla różnych produktów  
 **Automatyzacja** - Docker + skrypty inicjalizacyjne  
 **Testy jednostkowe** - gwarancja, że metody działają z naszymi przewidywaniami    
 **Cloudinary** - zarządzanie multimediami, żeby nie przechowywać ich w bazie danych  
 **MAUI** - interfejs użytkownika   

## Bezpieczeństwo

**Zaimplementowano:**
- Dostęp do bazy danych zabezpieczony loginem i hasłem
- Walidacja danych wejściowych
- Walidacja hasła (długość, znaki specjalne)
- Walidacja email (regex)

# TE WNIOSKI TRZEBA JAKOS POZMIENIAC BO DZIWNIE TO BRZMI
### 11.6 Wnioski

Projekt wykazał, że **MongoDB** jest doskonałym wyborem dla systemów e-commerce z elastycznym katalogiem produktów. Dokumentowa natura bazy pozwala na przechowywanie produktów o różnych specyfikacjach bez konieczności modyfikacji schematu.

**Docker** znacząco upraszcza deployment i zapewnia spójność środowisk developerskich, co było kluczowe dla zespołowego developmentu.

Wykorzystanie **Cloudinary** dla multimediów okazało się właściwą decyzją - odciążyło bazę danych i przyspieszyło ładowanie aplikacji.

**Architektura warstwowa** (DLL/Tests/App) zapewniła **separację odpowiedzialności** i ułatwiła testowanie oraz dalszy rozwój aplikacji.

**Extension Methods** w C# udowodniły swoją wartość jako eleganckie rozwiązanie dla filtrowania i sortowania, tworząc czytelny i łatwy w utrzymaniu kod.

---

## Załączniki

# TUTAJ NIE WIEM CZY JEST TO POTRZEBNE JAK COS TO WUSUNAC

### A. Wymagania Systemowe

**Do uruchomienia:**
- Docker Desktop
- .NET 8.0 SDK
- Visual Studio 2022 lub Visual Studio Code z C# DevKit

**Połączenie:**
- MongoDB: `localhost:1500`
- Credentials: `root/password`

### B. Instrukcja Instalacji

```bash
# 1. Klonowanie repozytorium
git clone <repository-url>
cd <Nazwa folderu - repozytorium>

# 2. Uruchomienie bazy danych
cd database
docker compose up -d --> utworzenie kontenera

# 3. Uruchomienie i wejście do bazy danych
docker compose start --> uruchomienie kontenera (opcja 1)
docker start projectShop --> uruchomienie kontenera (opcja 2)
docker exec -it projectShop mongosh -u root -p password --> wejście do bazy danych

# 5. Dodatkowe komendy
docker exec -it projectShop mongodump --db shop --username root --password password --authenticationDatabase admin --out /dump --> robienie dump'a

docker cp projectShop:/dump "sciezka gdzei chcemy go zapisac u siebie na komputerze"  --> po zrobieniu dump'a można go przekopiować z kontenera na nasze urządzenie

mongoimport   --username root   --password password   --authenticationDatabase admin   --db NAZWA BAZY   --collection NAZWA KOLEKCJI   --file /docker-entrypoint-initdb.d/PLIK.json   --jsonArray --> import danych do bazy


```

### C. Connection String

```csharp

link = "mongodb://root:password@"IP tam gdzie jest uruchomiona baza na Dockerze":1500/?authSource=admin"
DataManager.SetConnectionString(link);
```

### D. Kluczowe Zależności (NuGet)

**DLL projekt:**
- MongoDB.Driver 2.28.0
- CloudinaryDotNet 2.0.0

**Tests projekt:**
- xUnit 2.6.0
- xUnit.runner.visualstudio 2.5.0

**App projekt:**
- Microsoft.Maui.Controls 8.0.0
- CommunityToolkit.Mvvm 8.2.0

---

**Data utworzenia dokumentacji:** Grudzień 2025  
**Wersja projektu:** 1.0  
**Autorzy:** [Kacper Szulc oraz Michał Gołaszewski]
