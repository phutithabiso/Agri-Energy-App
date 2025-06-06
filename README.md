# Agri-Energy Connect Platform

## Overview

Agri-Energy Connect is a WPF-based desktop enterprise application developed using C# in Visual Studio. It connects South African farmers with green energy technology providers, enabling efficient product management, user role handling, and data validation in a secure and scalable system.

This README provides detailed setup instructions, system requirements, core features, and usage guidelines for both technical and non-technical stakeholders.

---

## 📦 Features

### 👨‍🌾 Farmer Role:
- Secure login.
- Add new green-agriculture products.
- View and manage own product listings.

### 👩‍💼 Employee Role:
- Add and manage farmer profiles.
- View and filter products submitted by all farmers.
- Search by category and production date.

### 🔐 Authentication:
- Secure login with role-based access (Farmer or Employee).
- Password hashing for credential protection.

### 📊 User Interface:
- Clean, responsive WPF UI using XAML.
- Real-time data binding with Entity Framework Core.

### 🛠️ Data Management:
- SQL Server database integration via Entity Framework Core.
- Validation checks for all form entries (e.g., product name, production date).
- Error handling for improved stability.

---

## ⚙️ Technologies Used

- **Frontend:** WPF (Windows Presentation Foundation)
- **Backend:** C# (Object-Oriented)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **IDE:** Visual Studio 2022 or newer
- **Others:** Azure Monitor, GitHub Actions (for DevOps CI/CD)

---

## 🖥️ System Requirements

| Component       | Minimum Requirement             |
|----------------|----------------------------------|
| OS             | Windows 10 or newer              |
| .NET Runtime   | .NET 6 SDK or newer              |
| Database       | SQL Server 2019 or newer         |
| RAM            | 4 GB (8 GB recommended)          |
| Disk Space     | Minimum 200 MB for database      |
| Tools Required | Visual Studio, SQL Server Mgmt Studio |

---

## 🚀 Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/phutithabiso/Agri_Energy_App.git
cd Agri_Energy_App
