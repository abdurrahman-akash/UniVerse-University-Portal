# School Management System

A web-based School / University Management System built with **ASP.NET Core (.NET 10)**, **Entity Framework Core**, and **ASP.NET Core Identity**.

---

## 🚀 Features

- **Authentication & Role-Based Access Control**:
  - ASP.NET Core Identity integration.
  - Three primary roles: `Admin`, `Faculty`, and `Student`.
  - Secure login, logout, and access-denied handling.
- **Dashboard**:
  - Interactive overview with key metrics and statistics.
- **Student Management**:
  - Create, view, edit, and delete student records.
  - Repository and Service layered architecture.
- **Faculty Management**:
  - Complete CRUD operations for faculty members.
- **Department Management**:
  - Track academic departments and assignments.
- **User Management**:
  - Admin management for users and roles.
- **Notice Board**:
  - Publish and manage announcements.

---

## 🛠️ Technology Stack

- **Framework**: .NET 10.0 (ASP.NET Core MVC)
- **Database**: Microsoft SQL Server / LocalDB
- **ORM**: Entity Framework Core 10.0
- **Security**: ASP.NET Core Identity
- **Architecture**: Repository Pattern + Service Layer + MVC

---

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022 / 2025](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit
- [SQL Server Express / LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

---

## ⚙️ Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd SchoolManagementSystem
```

### 2. Configure Database Connection
Update the connection string in `appsettings.json` if necessary:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=UniversityDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply Migrations & Update Database
Run the following command in the Package Manager Console or terminal:
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```
Navigate to `https://localhost:<port>` or `http://localhost:<port>` in your browser.

---

## 🔐 Default Admin Credentials

On first run, the database seeder automatically creates roles and an initial Administrator account:

- **Email**: `admin@universe.edu`
- **Password**: `Admin123`
- **Role**: `Admin`

---

## 📁 Project Structure

```text
SchoolManagementSystem/
├── Controllers/         # MVC & API Controllers
├── Data/                # DbContext and database seeders
├── Helpers/             # Helper classes and utilities
├── Migrations/          # EF Core migration files
├── Models/              # Domain entities and ViewModels
├── Properties/          # Launch settings
├── Repositories/        # Data access layer (Repository pattern)
├── Services/            # Business logic layer
├── Views/               # Razor views
├── wwwroot/             # Static files (CSS, JS, images, libraries)
├── appsettings.json     # Configuration file
└── Program.cs           # Application entry point & DI configuration
```

---

## 📝 License
This project is for educational and institutional management purposes.

