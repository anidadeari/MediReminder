# MediReminder API

A RESTful Web API built with ASP.NET Core (.NET 10) that helps patients and caregivers manage medication schedules, track intake history, and monitor adherence.

## Project Overview

MediReminder is a Service-Oriented Architecture (SOA) project developed for the SOA course at South East European University. The system enables users to register and authenticate securely using JWT tokens, manage medications with dosage details, log medication intake, view adherence statistics, receive upcoming dose reminders, and provides administrators with user management capabilities.

## Technologies Used

- ASP.NET Core Web API (.NET 10)
- PostgreSQL with pgAdmin4
- Entity Framework Core (Code-First)
- JWT Bearer Authentication
- BCrypt.Net for password hashing
- Swagger / OpenAPI for documentation
- Repository Pattern + Service Layer + Dependency Injection

## Features

### Authentication and Authorization
- User registration and login with JWT
- Role-based authorization (Admin / User)
- Auto-seeded admin account on first run
- Password hashing with BCrypt

### Medication Management
- Full CRUD operations for medications
- Filter active medications
- Get medication statistics

### Medication Logs
- Record medication intake (taken/missed)
- Add notes for each log entry
- View historical logs

### Reminders (Complex Business Logic)
- Calculate next dose time based on frequency
- Generate adherence reports for any period
- View upcoming reminders sorted by next dose time

### Admin Features
- View all registered users
- Delete users
- Register new administrators

## Prerequisites

- .NET 10 SDK
- PostgreSQL
- pgAdmin4

## Setup Instructions

1. Clone the repository:
   git clone https://github.com/anidadeari/MediReminder.git

2. Configure the database connection in appsettings.json

3. Run database migrations:
   dotnet ef database update

4. Run the application:
   dotnet run

5. Access Swagger UI at: https://localhost:7018/swagger

## Default Admin Credentials

On first run, an administrator account is automatically created:
- Email: admin@medireminder.com
- Password: Admin123!

## API Endpoints

### Authentication
- POST /api/Auth/register - Register a new user
- POST /api/Auth/login - Login and receive JWT token
- POST /api/Auth/register-admin - Register a new admin (Admin only)

### Users (Admin only)
- GET /api/Users - Get all users
- GET /api/Users/{id} - Get user by ID
- DELETE /api/Users/{id} - Delete a user

### Medications
- GET /api/Medications - Get user medications
- GET /api/Medications/active - Get active medications
- GET /api/Medications/{id} - Get medication by ID
- GET /api/Medications/{id}/stats - Get medication statistics
- POST /api/Medications - Create a new medication
- PUT /api/Medications/{id} - Update a medication
- DELETE /api/Medications/{id} - Delete a medication

### Medication Logs
- GET /api/MedicationLogs/{medicationId} - Get logs for a medication
- POST /api/MedicationLogs - Add a new log
- DELETE /api/MedicationLogs/{id} - Delete a log

### Reminders
- GET /api/Reminders/next/{medicationId} - Get next dose time
- GET /api/Reminders/adherence/{medicationId}?days=7 - Get adherence report
- GET /api/Reminders/upcoming - Get upcoming reminders

## Author

Anida Deari (@anidadeari)

## Course

Service-Oriented Architecture (SOA) - Final Project
South East European University