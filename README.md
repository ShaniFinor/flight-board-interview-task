# FlightBoard Interview task
A professional full-stack, real-time flight board management system.
It includes a .NET 8 backend and a React frontend with SignalR integration.

## Setup and Run Instructions
### Backend:
run:
cd backend
dotnet restore
dotnet ef database update
dotnet run
http://localhost:5264/
### Frontend:
Ensure you have Node.js installed.
run:
cd frontend
npm install
npm start
http://localhost:3000/

## Architecture
The backend is structured using Clean Architecture:
Domain, Application, Infrastructure, API layers.
Uses EF Core with a repository pattern.
SignalR is used for broadcasting flight changes.
FluentValidation for input validation.
Structured logging with ILogger<T>.
Server side Validation using FluentValidation -FlightValidator.cs

## Frontend:
React for the UI.
Redux Toolkit for minimal local state management.
TanStack Query (React Query) for API calls.
SignalR for real-time updates.
Framer Motion for animations.
SignalR client for live updates.
Client side validation - AddFlightForm.tsx.

## Third-Party Libraries:
Backend - 
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.AspNetCore.SignalR
Moq, xUnit (Testing)

Frontend-
react, react-dom
typescript
@reduxjs/toolkit, react-redux
@tanstack/react-query
@microsoft/signalr
framer-motion
testing-library/react, jest

## Features:
Real-time flight board with SignalR.
Add/Delete/Search flights.
Optimistic UI updates with React Query.
Client-side + server-side validations.
Clean Architecture & separation of concerns.
Structured logging & test.
Docker support for full containerization.

Docker:
docker compose build --no-cache
docker compose up

Frontend – http://localhost:3000
Backend (Swagger) – http://localhost:5000/swagger

after change:
docker compose down --rmi all -v
docker compose build --no-cache
docker compose up