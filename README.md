# SIPERUK Backend

API backend untuk sistem peminjaman ruangan kampus (SIPERUK) berbasis ASP.NET Core, PostgreSQL, dan JWT authentication.

## Tech stack
- ASP.NET Core 10
- Entity Framework Core 10 (Npgsql)
- JWT Bearer authentication
- Swagger / OpenAPI

## Prasyarat
- .NET SDK 10 terpasang
- PostgreSQL berjalan lokal (default: host `localhost`, port `5432`)
- (Opsional) `dotnet-ef` CLI: `dotnet tool install --global dotnet-ef` (versi 10.0.3)

## Konfigurasi
- Atur connection string di `appsettings.json` atau `appsettings.Development.json`:
  - `Host=localhost; Database=siperuk_db; Username=localhost; Password=password`
- JWT di `appsettings.json` key `Jwt`: `Key`, `Issuer`, `Audience`, `ExpirationMinutes`.

## Setup & menjalankan
1. Restore depedensi: `dotnet restore`
2. Update database (jalankan migrasi): `dotnet ef database update`
3. Jalankan server dalam mode Development (memakai `appsettings.Development.json`):
  - macOS/Linux: `ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5000 dotnet watch run`
  - Windows PowerShell: `$Env:ASPNETCORE_ENVIRONMENT="Development"; $Env:ASPNETCORE_URLS="http://localhost:5000"; dotnet watch run`
  - Swagger tersedia di `http://localhost:5000/swagger`

## Akun seed
- Admin: `admin@gmail.test` / password `admin123`
- Staff: `staff@gmail.test` / password `staff123`

## Migrasi database
- Tambah migrasi baru: `dotnet ef migrations add <NamaMigrasi>`
- Terapkan migrasi: `dotnet ef database update`

## Struktur utama
- `Program.cs` – konfigurasi service, JWT, Swagger
- `Data/AppDbContext.cs` – DbContext dan relasi
- `Models/` – entitas (User, Room, Booking, Status, History)
- `Seeders/` – data awal user dan status booking
- `Controllers/` – endpoint API (Auth, Room, Booking, dll.)

## Catatan
- HTTPS dimatikan untuk kemudahan lokal; aktifkan kembali untuk produksi.
- Ubah `Password` pada connection string sesuai kredensial PostgreSQL Anda.

## Lisensi
Internal use. Sesuaikan sebelum publikasi.