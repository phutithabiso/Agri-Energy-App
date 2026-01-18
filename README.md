# 🌱 Agri-Energy Connect  

---

## 📖 Project Overview
Agri-Energy Connect is an enterprise application designed to bridge South Africa’s agricultural sector with renewable energy providers. The platform empowers farmers to list products, connect with eco-conscious buyers, and access green energy solutions. Employees and stakeholders can monitor agricultural trends, register farmers, and analyze data for sustainable growth.

This project demonstrates:
- Performance-driven prototype development  
- Agile + DevOps methodologies  
- Enterprise architecture frameworks (TOGAF + ITIL)  
- Secure database integration and role-based authentication  

---

## 🚀 Features
### 👩‍🌾 Farmers
- Register and manage personal profiles  
- Add new agricultural products (name, category, production date, description)  
- View, edit, and delete product listings  
- Search products with filters  
- Offline accessibility for rural regions  

### 🧑‍💼 Employees
- Register new farmers into the system  
- View all farmers and their details  
- Access and filter all products by category, date, or farmer  
- Analyze agricultural trends and performance  

### 🔐 Authentication
- Secure login with hashed passwords  
- Role-based access (Farmer vs Employee)  
- Error handling for invalid credentials and registration issues  

---

## 🗄️ Database Development
- **SQL Server + Entity Framework Core** for relational data management  
- **Tables:** Users, Farmers, Products  
- **Relationships:** Farmer ↔ Products, Users ↔ Roles  
- **Sample Data:** Preloaded for demonstration (e.g., fruits, vegetables, dairy products)  

---

## ⚡ Performance Optimizations
### Prototype
- Profiling & diagnostics with VisualVM and dotMemory  
- Code efficiency improvements (flattened loops, removed redundancy)  
- In-memory caching for repetitive data  
- Load testing with Apache JMeter  

### Final Product Guidelines
- Microservices architecture for modular scalability  
- Asynchronous background tasks (email dispatch, audit logging)  
- Optimized database queries with indexes and pooling  
- Observability with Azure Monitor + App Insights  
- CI/CD pipelines with performance-driven testing  

---

## 🛠️ Methodologies
### Agile (Scrum)
- Iterative delivery via sprints  
- Early stakeholder feedback (farmers, cooperatives, marketing teams)  
- Low-risk releases with MVP-first approach  
- Adaptability to regulatory changes  

### DevOps Integration
- Automated pipelines with GitHub Actions + Azure DevOps  
- CI/CD for rapid, reliable deployments  
- Infrastructure as Code (Azure Resource Manager templates)  
- Continuous monitoring with telemetry  
- Tools: Postman, Docker, Kubernetes, Jenkins  

### Architecture Frameworks
- **TOGAF:** Strategic alignment, ADM cycle, architecture repository  
- **ITIL:** Service-centric delivery, SLA/KPI tracking, incident/change management  
- **Zachman (Supplementary):** Classification schema for modeling  

---

## 🖥️ Tech Stack
- **Frontend:** WPF (XAML, C#)  
- **Backend:** ASP.NET Core, Entity Framework Core, SQL Server  
- **DevOps Tools:** GitHub Actions, Azure DevOps, Docker, Kubernetes, Jenkins  
- **Monitoring:** Azure Monitor, App Insights  
- **Testing:** Apache JMeter, Postman  

---

## 📈 Business Value
- **Empowerment:** Farmers gain visibility in eco-conscious markets  
- **Efficiency:** Centralized digital platform reduces manual errors  
- **Sustainability:** Aligns agriculture with renewable energy initiatives  
- **Storytelling:** Positions the system as a digital bridge between rural farming and green technology  

---

## 📌 How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/phutithabiso/Agri-Energy-App.git
