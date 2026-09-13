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
  - Catatan: `Key` wajib berukuran minimal 256 bit (minimal 32 karakter) untuk algoritma HS256.

## Setup & menjalankan
1. Pastikan PostgreSQL berjalan dan database `siperuk_db` sudah dibuat.
2. Restore dependensi: `dotnet restore`
3. Update database (jalankan migrasi & seed): `dotnet ef database update`
4. Jalankan server:
  - macOS/Linux: `dotnet run --launch-profile http` (atau `ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5234 dotnet run`)
  - Windows PowerShell: `dotnet run --launch-profile http`
  - URL API: `http://localhost:5234`
  - Swagger tersedia di `http://localhost:5234/swagger`

## Akun seed
- Admin: `admin@gmail.com` / password `admin123`
- Staff: `staff@gmail.com` / password `staff123`

## Akses PostgreSQL
- **Kredensial Default**:
  - Host: `localhost`
  - Port: `5432`
  - Database: `siperuk_db`
  - Username: `postgres`
  - Password: `postgres`

- **Akses via CLI (`psql`)**:
  ```bash
  PGPASSWORD=postgres psql -h localhost -U postgres -d siperuk_db
  ```
  *(Catatan macOS Homebrew: jika `psql` command not found, gunakan path `/opt/homebrew/opt/postgresql@18/bin/psql` atau export ke PATH)*

  Perintah umum di dalam psql:
  - `\dt` : Melihat daftar tabel
  - `\d "Users"` : Melihat skema tabel Users
  - `SELECT "Id", "FullName", "Email", "Role" FROM "Users";` : Menampilkan data user
  - `\q` : Keluar dari psql

- **Akses via GUI (TablePlus / DBeaver / Postico / VS Code Extension)**:
  - Driver: PostgreSQL
  - Host: `localhost`
  - Port: `5432`
  - User: `postgres`
  - Password: `postgres`
  - Database: `siperuk_db`

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