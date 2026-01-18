# 🌱 Agri-Energy App  

A **C# WPF desktop application** built in **Visual Studio** with **SQL Server** integration.  
This project is a prototype academic application demonstrating CRUD operations, role-based authentication, and database connectivity for managing agricultural products and farmers.  

---

## 📖 Project Description
Agri-Energy App allows **Farmers** and **Employees** to interact with agricultural product data in a simple, role-based system.  

- **Farmers** can register, log in, and manage their own products (add, edit, delete, search).  
- **Employees** can register new farmers, view all farmers, and access all products in the system.  
- **Authentication** is role-based, with secure login and error handling for invalid credentials.  
- **Database** is implemented using **Entity Framework Core**, with sample data preloaded for demonstration.  

---

## 🚀 Features
### 👩‍🌾 Farmers
- Register and manage personal profiles  
- Add new agricultural products (name, category, production date, description)  
- View, edit, and delete their own products  
- Search products by name or category  

### 🧑‍💼 Employees
- Register new farmers  
- View all farmers and their details  
- Access all products in the system  
- Filter products by category or date  

### 🔐 Authentication
- Login with email + password  
- Role-based access (Farmer vs Employee)  
- Error handling for invalid login/registration attempts  

---

## 🗄️ Database
- **SQL Server** database with tables for:
  - `Users` (Id, FirstName, LastName, Email, PasswordHash, Role, DateRegistered, LastLogin)  
  - `Products` (Id, Name, Category, ProductionDate, Description, FarmerId)  
- Sample data is preloaded (e.g., Banana, Apple, Carrot, Rice).  

---

## 🖥️ Tech Stack
- **Frontend:** WPF (XAML, C#)  
- **Backend:** Entity Framework Core  
- **Database:** SQL Server  
- **IDE:** Visual Studio  

---

## 📌 How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/phutithabiso/Agri-Energy-App.git
