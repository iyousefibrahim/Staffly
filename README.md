# Staffly - MVC .NET 8 Core Project

Staffly is an employee management system built using **ASP.NET Core 8 MVC**. This project follows modern design principles such as **Repository Pattern** and **Unit of Work**. It is structured using a **3-Tier Architecture** to provide a flexible and scalable framework.

## Technologies Used:
- **ASP.NET Core 8 MVC**
- **Entity Framework Core (EF Core)**
- **Microsoft SQL Server**
- **Identity** for Authentication and Authorization
- **AutoMapper** for Object Mapping

## Features:
### Employee and Department Management:
- **CRUD operations** for **Departments** and **Employees**
- **Search functionality** for both **Departments** and **Employees**
- **View details** of Departments and Employees

### Authentication:
- **Sign In** and **Sign Up** functionality for users
- **Password Reset** feature with email confirmation via Gmail

### User Management:
- **CRUD operations** for **Users**
- **Search functionality** for Users
- **Update User Details** and **Delete User**
  
### Roles Management:
- **Search functionality** for **Roles**
- **Update User Roles**
- **View Role Details**

### Additional Features:
- **AutoMapper** for seamless object-to-object mapping
- **Unit of Work** to handle database transactions
- **Repository Pattern** and **Generic Repository Pattern** for data access

## Project Architecture:
- **3-Tier Architecture**:
  - **Presentation Layer** (MVC Views)
  - **Business Logic Layer** (Services)
  - **Data Access Layer** (Repositories)
