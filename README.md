# EmployeeHub

Fullstack Employee Management System built with **Angular**, **.NET 8/9+**, **Keycloak Authentication**, and **SQL Server**.

---

## 🚀 Overview

EmployeeHub is a modern fullstack web application for managing employees in an organization.  
It demonstrates authentication, authorization, CRUD operations, and a clean layered architecture.

---

## 🧱 Tech Stack

### Frontend
- Angular 20+
- Angular Material
- Keycloak Angular
- RxJS
- TypeScript

### Backend
- .NET (ASP.NET Core Web API)
- Entity Framework Core
- SQL Server
- JWT Authentication (Keycloak)

### Infrastructure
- Keycloak (Identity & Access Management)
- Docker (optional for DB / Keycloak)
- GitHub for version control

---

## 🔐 Authentication

Authentication is handled via **Keycloak (OpenID Connect / OAuth2)**.

- Login via Keycloak
- JWT token stored in frontend
- HTTP Interceptor attaches Bearer token
- Backend validates token via JWT Bearer middleware

---

## 📦 Features (Current State)

### ✅ Implemented
- Keycloak Login / Logout
- Angular Shell Layout (Toolbar + Sidebar)
- Employee List (Material Table)
- Secure API communication
- JWT Authentication
- CORS configuration
- Loading & Error handling
