# OpenFoodSearch - Vibe Coding

Full-stack app that fetches food product data from the [Open Food Facts](https://world.openfoodfacts.org/) API, stores it in PostgreSQL, and displays it in an Angular frontend.

## Stack

- **Backend**: ASP.NET Core 8 Web API + Entity Framework Core + Npgsql (PostgreSQL)
- **Frontend**: Angular 17 (standalone components)
- **Database**: PostgreSQL 16 (via Docker)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) and npm
- [Docker](https://www.docker.com/) and Docker Compose
- [dotnet-ef tool](https://learn.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

## Setup

### 1. Start PostgreSQL
```bash
docker compose up -d
```

### 2. Run Backend
```bash
cd Backend
dotnet restore
dotnet run
# API available at http://localhost:5000
```

### 3. Run Frontend
```bash
cd frontend
npm install
npm start
# App available at http://localhost:4200
```

## API Endpoints

| Method | URL | Description |
|--------|-----|-------------|
| GET | `/api/products` | List all products (optional `?search=term`) |
| POST | `/api/products/fetch?query=chocolate` | Import products from Open Food Facts |

## Usage

1. Open http://localhost:4200
2. Click **"Importar da API"** to fetch food products from Open Food Facts
3. Use the search bar to filter products
