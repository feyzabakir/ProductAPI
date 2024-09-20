# ProductAPI: RESTful Web API
Bu proje, basit bir ürün yönetimi için N-Tier mimarisi kullanılarak geliştirilen bir RESTful Web API'dir. CRUD işlemleri gerçekleştirilir ve JWT ile kimlik doğrulama ve rol yönetimi uygulanır. 

## Kullanılan Teknolojiler ve Tasarım Desenleri
- .NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- FluentValidation
- Serilog
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection

  ### Gereksinimler
    - .NET SDK 8.0
    - SQL Server
    - Visual Studio veya Visual Studio Code

 # Kurulum

 1.Projeyi klonlayın

```bash
git clone https://github.com/feyzabakir/ProductAPI.git
```

2.Proje dizinine gidin
```bash
cd ProductAPI
```

3.Veritabanı Bağlantısı

`appsettings.json` dosyasındaki `ConnectionStrings` bölümünü kendi SQL Server veritabanınıza göre yapılandırın:

```bash
"ConnectionStrings": {
  "SqlConnection": "Server=YourServerName;Database=ProductApiDb;Trusted_Connection=True;"
}
```

4.Gerekli NuGet paketlerini yükleyin
```bash
dotnet restore
```

5.dotnet-ef aracını yükleyin
```bash
dotnet tool install --global dotnet-ef
```

6.Migration'ları Uygulama

Migration'ları uygulamak için `ProductAPI.Repository` projesinde olmalısınız, çünkü bu proje veritabanı işlemlerini yönetiyor.

```bash
cd ProductAPI.Repository
```
Migration oluşturun:
```bash
dotnet ef migrations add InitialCreate --project ProductAPI.Repository --startup-project ProductAPI.API
```
Veritabanını güncelleyin:
```bash
dotnet ef database update --project ProductAPI.Repository --startup-project ProductAPI.API
```
Projeyi çalıştırın:
```bash
dotnet run --project ProductAPI.API
```

## API Endpoint'leri

| HTTP Metodu | Endpoint                | Açıklama                          | Rol Gereksinimi           |
|-------------|-------------------------|-----------------------------------|---------------------------|
| GET         | `/api/products`          | Tüm ürünleri getirir              | Herkes                   |
| GET         | `/api/products/{id}`     | Belirli bir ürünü getirir         | Herkes                   |
| POST        | `/api/products`          | Yeni bir ürün ekler               | Herkes                   |
| PUT         | `/api/products`          | Var olan bir ürünü günceller      | Admin                    |
| DELETE      | `/api/products/{id}`     | Belirli bir ürünü siler           | Admin                    |
| POST        | `/api/users/signup`      | Yeni kullanıcı kaydı oluşturur    | Herkes                   |
| POST        | `/api/users/login`       | Kullanıcı girişi yapar ve JWT alır| Herkes                   |
| PUT         | `/api/users`             | Kullanıcı bilgilerini günceller   | Admin                    |
| DELETE      | `/api/users/{id}`        | Belirli bir kullanıcıyı siler     | Admin                    |
| POST        | `/api/users/{id}/addrole`| Belirli bir kullanıcıya rol atar  | Herkes                   |


### Seed Datalar
Proje başlatıldığında, veritabanına önceden tanımlı bazı başlangıç verileri (seed data) eklenir. Bu sayede test ve geliştirme aşamasında gerekli olan temel veriler otomatik olarak veritabanında bulunur. Seed verileri şunları içerir:
- ProductSeed
- RoleSeed
    - Admin
    - User
- UserSeed
    - admin@example.com: Admin rolüne sahip yönetici kullanıcı.
    - user@example.com: User rolüne sahip standart kullanıcı.
 
### Identity Kullanımı
Projede kimlik doğrulama ve yetkilendirme işlemleri için `ASP.NET Core Identity` kullanılmıştır. Identity framework'ü, kullanıcı yönetimi, şifre doğrulama, roller ve diğer güvenlik gereksinimleri için gerekli altyapıyı sağlar. 
Uygulama başlangıcında bazı roller ve kullanıcılar otomatik olarak seed verileriyle oluşturulmuştur.

### Loglama
Projede loglama için `Serilog` kullanılmaktadır. Loglar hem konsola hem de bir dosyaya yazılır.
Loglar, logs/log-.txt dosyasına günlük olarak kaydedilir.
