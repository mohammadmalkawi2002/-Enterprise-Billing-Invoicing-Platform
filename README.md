🚀 Enterprise Billing & Invoicing Platform

A production-style ASP.NET Core Web API built using Clean Architecture for managing customers, invoices, payments, and financial reporting.
The system demonstrates enterprise backend development practices including secure authentication, background processing, reporting APIs, and scalable architecture.

✨ Features
📊 Billing & Invoicing

Full invoice lifecycle management (Draft → Sent → Paid → Overdue)

Customer management with validation rules

Payment tracking with automatic invoice status updates

Financial reporting APIs with filtering, sorting, and pagination

🔐 Authentication & Security

JWT Bearer Authentication

ASP.NET Core Identity integration

Role-Based Access Control (Admin, Accountant, Viewer)

Claims-based authorization

⚙️ Background Processing

Automated overdue invoice detection (Hangfire jobs)

Async email notifications with PDF invoice attachments

Retry mechanisms and structured logging

📄 Documents & Communication

Professional invoice PDF generation (QuestPDF)

Email delivery via MailKit/MimeKit

Configurable SMTP settings

🧱 Architecture & Quality

Clean Architecture layered design:

API (Presentation)

Application (Business Logic)

Infrastructure (Data & Services)

Domain (Core Models)

Repository & Unit of Work patterns

SOLID principles

Global exception handling middleware

FluentValidation for input validation

AutoMapper for DTO mapping

🛠 Tech Stack
Backend

.NET 8 / C# 12

ASP.NET Core Web API

Entity Framework Core

SQL Server

Libraries & Tools

Hangfire (Background Jobs)

FluentValidation

AutoMapper

Swagger / OpenAPI

QuestPDF

MailKit / MimeKit

Security

ASP.NET Identity

JWT Authentication

RBAC Authorization

📦 API Capabilities

RESTful endpoints with proper HTTP status codes

Pagination metadata via headers

Filtering, sorting, searching support

Structured error responses (Problem Details style)

🗄 Database Features

EF Core migrations

Soft delete with query filters

Audit fields (CreatedAt, UpdatedAt, DeletedAt)

Optimized queries using projection to DTOs

▶️ Getting Started
1️⃣ Clone Repository
git clone https:https://github.com/mohammadmalkawi2002/-Enterprise-Billing-Invoicing-Platform
cd your-project

2️⃣ Configure Database

Update connection string in:

appsettings.json


Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=BillingDb;Trusted_Connection=True;"
}

3️⃣ Apply Migrations
dotnet ef database update

4️⃣ Run Application
dotnet run


Swagger UI:

https://localhost:xxxx/swagger

👤 Default Roles Example
Role	Permissions
Admin	Full system access
Accountant	Billing & payments
Viewer	Read-only access
📈 Learning Goals

This project demonstrates:

Enterprise backend architecture

Secure API design

Business logic implementation

Background job orchestration

Clean code & maintainability practices

📬 Contact

Mohammad Malkawi
Backend .NET Developer

LinkedIn: (https://www.linkedin.com/in/mohammad-malkawi-461b08287/)

Email: mohammadmalkawi681@gmail.com

⭐ If you like this project

Give it a ⭐ on GitHub — it helps a lot 🙂
