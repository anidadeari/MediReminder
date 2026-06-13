# MediReminder API

A cloud-deployed RESTful Web API built with ASP.NET Core (.NET 10) that enables patients and caregivers to manage medication schedules, track medication intake, monitor adherence, and receive intelligent reminders.

## Project Overview

MediReminder is a Service-Oriented Architecture (SOA) project developed as a final course project at South East European University. The application provides secure JWT-based authentication, medication management, medication intake tracking, adherence reporting, reminder calculation services, and administrative user management.

The system follows a layered architecture using Controllers, Services, Repositories, and Entity Framework Core with PostgreSQL. Continuous Integration and Continuous Deployment (CI/CD) are implemented using GitHub Actions and Railway Cloud Platform.

## Technologies Used

* ASP.NET Core Web API (.NET 10)
* PostgreSQL Database
* Entity Framework Core (Code-First)
* JWT Bearer Authentication
* BCrypt Password Hashing
* Swagger / OpenAPI Documentation
* Repository Pattern
* Service Layer Architecture
* Dependency Injection
* GitHub Actions (CI/CD)
* Railway Cloud Deployment

## Key Features

### Authentication & Authorization

* User registration and login
* JWT token generation and validation
* Role-based access control (Admin/User)
* Secure password hashing using BCrypt

### Medication Management

* Create, update, retrieve, and delete medications
* Track active medications
* Generate medication statistics

### Medication Intake Logs

* Record taken or missed medications
* Add notes for each intake record
* Access historical medication logs

### Reminder Services

* Calculate upcoming medication schedules
* Generate adherence reports
* Display upcoming reminders

### Administration

* Manage registered users
* Register administrators
* Remove user accounts

## Deployment

The application is deployed on Railway Cloud Platform and uses PostgreSQL as the production database.

CI/CD automation is implemented through GitHub Actions, which automatically builds and tests the application whenever changes are pushed to the GitHub repository.

## Author

**Anida Deari**
South East European University
Faculty of Contemporary Sciences and Technologies

## Course

Service-Oriented Architecture (SOA) – Final Project
