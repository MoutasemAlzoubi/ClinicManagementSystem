# Clinic Management System

A clinic management application rebuilt around the original project structure and coding style: ASP.NET Core Web API (.NET 8), Entity Framework Core, SQL Server, ASP.NET Core Identity/JWT, and Angular 16.

## Included functionality

- Public English landing page and patient registration.
- Identity login with JWT authentication.
- Roles: `Admin`, `Doctor`, `Patient`.
- Admin-only doctor account creation.
- Patient self-registration and patient profile linkage.
- Patient, doctor, country, medical record, appointment, and prescription APIs.
- Patient appointment requests with doctor time-overlap validation.
- Separate Admin, Doctor, and Patient dashboard components in Angular, with separate API service classes.
- Backend ownership checks for patient and doctor data.
- Restrictive delete behavior to protect clinical history.

## Requirements

- .NET 8 SDK
- SQL Server (LocalDB or SQL Server Express/Developer)
- Node.js 18 or 20 recommended for Angular 16
- npm

## Backend setup

1. Open `Clinic Management System.sln` in Visual Studio.
2. Confirm the SQL Server connection in `Clinic Management System/appsettings.json`. The default is Windows integrated authentication against `localhost`.
3. Set the development JWT signing key and initial Admin credentials. A demo-only local configuration is supplied in `appsettings.Development.json`. Change the demo Admin password immediately. Never use the development key or demo password in production.
4. Open Package Manager Console in the API project and run:

   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

   Or from a terminal in the API project:

   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.25
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Run the API using the `https` profile. The launch profile uses `https://localhost:7253` and `http://localhost:5251`. Swagger is available at `/swagger` in Development.

The application also calls `Database.Migrate()` during startup so later migrations are applied automatically. The first migration must be generated once before the first run.

### Admin account (Development only)

The development configuration currently seeds:

- Email: `admin@clinic.local`
- Password: `ChangeThisAdmin123`

Change this password after first login. For production, remove the demo credentials and configure `AdminSeed__Email`, `AdminSeed__Password`, `Jwt__Key`, `Jwt__Issuer`, and `Jwt__Audience` using environment variables or a secret manager. Use a unique random JWT key of at least 32 characters.

## Frontend setup

1. Open a terminal in `clinic-management-system`.
2. Install dependencies:

   ```bash
   npm install
   ```

3. Start Angular:

   ```bash
   npm start
   ```

4. Open `http://localhost:4200`.

The Angular API URLs are kept inside the separate files under `src/app/Service`. If your API runs on another URL, update the `apiUrl` value in those service files. Trust the local ASP.NET HTTPS development certificate if the browser reports a certificate warning:

```bash
dotnet dev-certs https --trust
```

## Roles and access

- **Admin:** manages clinic data, creates doctor accounts, reviews appointments, and manages appointment status.
- **Doctor:** views their own appointments and their clinical records; can create clinical records and prescriptions only for patients with a non-cancelled appointment with that doctor.
- **Patient:** registers publicly, requests appointments, views their own appointments, records, and prescriptions.

Angular route guards improve navigation, but authorization is enforced independently by API role attributes and ownership checks.

## Code structure and style

The backend follows the same flow used in the original Country and Patient modules:

- `Data` contains the database entities (`Patient`, `Country`, `Doctor`, etc.).
- `Model` contains request/response DTOs, with each DTO in its own file.
- `Controllers` receive DTOs and call service interfaces only. They do not query `ClinicContext` directly.
- `Service` contains each interface and its implementation. Services map DTO values to `Data` entities, use `ClinicContext`, call `SaveChanges()`, and map database results back to DTOs.
- Normal CRUD and data operations use synchronous EF Core methods, matching the original `SaveChanges()` / `ToList()` style. Async methods are limited to Identity/account workflows such as sign-in, patient registration, and creating doctor accounts.

The Angular side keeps API calls in separate classes under `src/app/Service`; components inject those services and use `subscribe()` to handle results. The public home, login, registration, and each role dashboard are separate components.

## Important implementation notes

- The old EF Core migration files were removed because the database has not been created and the current model needed a clean initial schema. Generate a new `InitialCreate` migration using the commands above.
- Do not delete clinical records in normal operations. Patient/doctor deletion is blocked when clinical history exists; deactivate doctors instead.
- Appointment overlap validation is performed server-side in a serializable transaction. For high-volume production deployment, consider a dedicated scheduling strategy and concurrency monitoring.
- This is a functional development foundation, not a certified medical-record system. Before real patient use, add audit trails, consent/privacy workflows, backup and retention policies, secure hosting, logging/monitoring, and jurisdiction-specific compliance review.

## Project structure

- `Clinic Management System/` — ASP.NET Core Web API.
- `clinic-management-system/` — Angular application.


## Build and verification checklist

Backend:

```bash
dotnet restore
dotnet build
```

Frontend:

```bash
npm ci
npm run build
```

After starting both applications, verify:

1. Public home page loads and displays active doctors.
2. Patient registration rejects duplicate email and creates both Identity user and Patient profile.
3. Patient can sign in and cannot access Admin/Doctor API endpoints.
4. Admin can create a Doctor account; public Doctor registration is not available.
5. Doctor can only view appointments and records assigned to that doctor.
6. Patient can only view their own appointments, records, and prescriptions.
7. Two overlapping appointments for the same doctor cannot both be accepted.
8. Admin can confirm/cancel appointments.
9. Doctor can create a medical record or prescription only for a patient who has a non-cancelled appointment with that doctor.
10. Deleting a patient/doctor with clinical history is blocked.

**Verification status:** the Angular production build was run successfully using the dependency versions already present in the original project archive. Angular reports bundle/style budget warnings, but the build completes. The .NET SDK is not installed in the preparation environment, so `dotnet build` could not be run here. Run `dotnet restore` and `dotnet build` in Visual Studio on your Windows machine before treating the backend build as verified.
