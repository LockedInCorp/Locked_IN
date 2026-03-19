## Locked_IN

Locked_IN is a full‑stack web application that helps gamers discover teammates, manage game profiles, create and join groups, and communicate in real time.  
It consists of:
- **Backend**: ASP.NET Core 9 Web API with SignalR, PostgreSQL, MinIO object storage and JWT authentication.
- **Frontend**: React + TypeScript SPA built with Vite, Tailwind CSS and Zustand, communicating with the backend and SignalR hubs.

### Features

- **User accounts & profiles**
  - Registration, login and JWT‑based authentication (token stored in HTTP‑only cookies).
  - User profile management with availability, preferences and experience tags.
  - Game‑specific profiles (per‑game roles, ranks, etc.).

- **Teams & groups**
  - Create and edit teams / groups with detailed metadata.
  - Search and discover groups with filters.
  - Join requests, invitations and team membership management.

- **Friends & social**
  - Friend requests, accept / reject flow.
  - Viewing friend profiles and their game profiles.

- **Chat & real‑time communication**
  - One‑to‑one and group chat backed by SignalR hubs (`/chathub`, `/teamjoinhub`, `/teamrequesthub`).
  - Live updates for join requests, invitations and messages.
  - File uploads (avatars, team icons, attachments) stored in MinIO buckets.

- **Modern UI**
  - React 19 + Vite + TypeScript.
  - Tailwind CSS 4 with Radix UI components and lucide icons.
  - State management with Zustand and data fetching with TanStack Query.

---

## Project Structure

- `Locked_IN/`
  - `Locked_IN_Backend/` – ASP.NET Core 9 backend (`Locked_IN_Backend.csproj`, `Program.cs`, controllers, services, repositories, SignalR hubs, EF Core context and entities).
  - `locked_in_frontend/` – React + TypeScript frontend (Vite, Tailwind, React Router, Zustand, SignalR client, tests).
  - `docker-compose.yml` – multi‑container setup for backend app, PostgreSQL database and MinIO.
  - `deploy/` – built frontend and backend artifacts suitable for publishing.

---

## Prerequisites

- **Node.js** 20+ and npm (for the frontend).
- **.NET SDK** 9.0+ (for backend development and EF Core tooling).
- **Docker & Docker Compose** (recommended / required for running the full stack via containers).

Optional (for local, non‑Docker setup):
- **PostgreSQL** 15+.
- **MinIO** or another S3‑compatible object storage.

---

## Running with Docker (recommended)

From the repository root (`Locked_IN`), run:

```bash
cd Locked_IN
docker compose up --build
```

This will start:
- **app**: ASP.NET Core backend (and, depending on configuration, may serve the built frontend) exposed on `http://localhost:5100` (mapped to container port `8080`).
- **db**: PostgreSQL 15 with database `locked_in`, exposed on `localhost:5435`.
- **minio**: MinIO object storage, available at:
  - API: `http://localhost:9000`
  - Console: `http://localhost:9001`

Default credentials (from `docker-compose.yml`):
- PostgreSQL: `POSTGRES_USER=postgres`, `POSTGRES_PASSWORD=postgres`.
- MinIO: `MINIO_ROOT_USER=minioadmin`, `MINIO_ROOT_PASSWORD=minioadmin`.

Environment variables used by the backend container:
- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__Default=Host=db;Port=5432;Database=locked_in;Username=postgres;Password=postgres;TrustServerCertificate=true;`
- `Minio__Endpoint=minio:9000`
- `Minio__AccessKey=minioadmin`
- `Minio__SecretKey=minioadmin`
- `Minio__Secure=false`
- `Minio__PublicUrl=http://localhost:9000`
- `Jwt__Secret` / `Jwt__Issuer` / `Jwt__Audience` – JWT configuration.

Once containers are up:
- Open the **frontend** (if served by the backend) at `http://localhost:5100`.
- The **API** and Swagger UI (in development environments) are accessible from the same host / port.

To stop:

```bash
docker compose down
```

---

## Local Development (without Docker)

You can also run backend and frontend separately on your host machine.

### Backend (ASP.NET Core)

1. Go to the backend directory:

   ```bash
   cd Locked_IN/Locked_IN_Backend
   ```

2. Configure a local development `appsettings.Development.json` (or `secret.json`) with:
   - Connection string for PostgreSQL (matching your local instance).
   - MinIO (or S3‑compatible) endpoint, access key, secret key and `Secure` flag.
   - `Jwt:Secret`, `Jwt:Issuer`, `Jwt:Audience`.

3. Apply EF Core migrations and run the backend:

   ```bash
   dotnet ef database update   # if migrations are present
   dotnet run
   ```

4. By default the app will:
   - Configure Identity, JWT authentication and authorization.
   - Run database migrations on startup.
   - Initialize public MinIO buckets: `useravatars`, `teamicons`, `attachments`.
   - Expose SignalR hubs at:
     - `/chathub`
     - `/teamjoinhub`
     - `/teamrequesthub`

### Frontend (React + Vite)

1. Go to the frontend directory:

   ```bash
   cd Locked_IN/locked_in_frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the dev server:

   ```bash
   npm run dev
   ```

4. Open the app at the URL printed by Vite (commonly `http://localhost:5173`).

Make sure the backend CORS policy in `Program.cs` (`AllowFrontend` and `AllowSignalR`) includes your frontend dev origin (it already allows `http://localhost:5173`).

---

## Frontend Stack Notes

- **Build & tooling**
  - Vite for development and build (`npm run dev`, `npm run build`, `npm run preview`).
  - TypeScript 5.8, ESLint for linting (`npm run lint`).
  - Playwright is configured for end‑to‑end tests (`@playwright/test`).

- **Styling**
  - Tailwind CSS 4 with additional plugins (`tailwindcss-animate`, `tailwindcss-fluid-type`).
  - Radix UI primitives (Avatar, Checkbox, Label, Radio Group, Select, Switch, etc.).

- **State & data**
  - Zustand stores for local and global UI state (e.g. group creation / editing flows).
  - TanStack React Query for server state and API calls.

- **Auth persistence**
  - Auth user data (id, email, nickname, avatar URL) is stored in a cookie under the key `auth_user_data`.
  - Cookies are configured as `SameSite=Lax` and `Secure` when running under HTTPS or production builds.

---

## Backend Stack Notes

- **Tech**
  - ASP.NET Core 9 (`net9.0`) Web API.
  - Entity Framework Core with `Npgsql` (PostgreSQL provider).
  - ASP.NET Core Identity (`User` entity with `int` keys).
  - JWT authentication via `Microsoft.AspNetCore.Authentication.JwtBearer`.
  - Real‑time functionality via SignalR.
  - MinIO for object storage integration.
  - Swagger / OpenAPI via Swashbuckle.

- **Composition**
  - `Program.cs` configures:
    - DbContext and Identity.
    - JWT authentication and authorization, reading the token from `AuthToken` cookie.
    - CORS policies for the SPA and SignalR hubs.
    - Swagger in development.
    - Middleware pipeline including a custom `ExceptionHandlingMiddleware`.
    - AutoMapper, FluentValidation, and a set of repository / service abstractions.

---

## Environment & Configuration

Key configuration sections (depending on environment):
- `ConnectionStrings:Default` / `DefaultConnection` – PostgreSQL connection string.
- `Jwt:Secret`, `Jwt:Issuer`, `Jwt:Audience` – JWT signing and validation.
- `Minio:Endpoint`, `Minio:AccessKey`, `Minio:SecretKey`, `Minio:Secure`, `Minio:PublicUrl` – MinIO connection and public URL.

In production, these values are typically supplied via environment variables (see `docker-compose.yml`).  
In development, they may be stored in `appsettings.Development.json` or user secrets.

---

## Scripts & Common Commands

### Frontend

- **Run dev server**: `npm run dev`
- **Build for production**: `npm run build`
- **Preview production build**: `npm run preview`
- **Lint**: `npm run lint`

### Backend

- **Run the app**:

  ```bash
  dotnet run --project Locked_IN/Locked_IN_Backend/Locked_IN_Backend.csproj
  ```

- **Apply database migrations** (if set up):

  ```bash
  dotnet ef database update --project Locked_IN/Locked_IN_Backend/Locked_IN_Backend.csproj
  ```

---

## Testing

### Frontend (Playwright)

From `locked_in_frontend`:

```bash
npm run test:e2e        # or the script name you configure for Playwright
```

Adjust or add an npm script in `package.json` (e.g. `"test:e2e": "playwright test"`) if not present yet.

### Backend

If backend test projects are added in the future, they can be run with:

```bash
dotnet test
```

---

## Deployment

The `deploy/` directory contains ready‑to‑serve frontend assets and a published backend:

- `deploy/frontend/` – static files built with Vite.
- `deploy/backend/` – published ASP.NET Core app with `web.config` and runtime files.

Typical deployment options:
- Host the backend in IIS, Kestrel behind a reverse proxy, or in containers.
- Serve the `deploy/frontend` assets from the backend itself (via `UseStaticFiles` and `MapFallbackToFile("index.html")`) or from a static file host / CDN.

Make sure to:
- Configure environment variables for connection strings, JWT and MinIO.
- Point the frontend environment (Vite env vars) at the correct backend base URL.

