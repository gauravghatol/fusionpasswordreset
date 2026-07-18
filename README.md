# Fusion Password Reset Portal Production Readiness Guide

This document explains exactly how to take this repository from scaffold to a real client-ready production deployment.

Use the steps in order. Do not skip ahead to deployment before the earlier integration, security, and validation steps are complete.

## 1. What This Repository Is Right Now

This repository is a clean-architecture ASP.NET Core MVC `.NET 8` foundation for an Oracle Fusion password reset portal.

It already includes:

- Presentation, Application, Domain, and Infrastructure layers
- Entra ID authentication wiring
- SQL Server and EF Core structure
- Oracle API service abstraction
- Serilog and Application Insights wiring
- Key Vault integration points
- MVC pages for sign-in, dashboard, search, reset, success, and error flow

It is not yet a fully proven production app until the client-specific values, schemas, APIs, security rules, and deployment setup are completed and tested end to end.

## 2. Recommended Delivery Order

Follow this order:

1. Prepare local machine and SDK/runtime
2. Create GitHub repository and push source
3. Confirm real client requirements
4. Configure Azure Entra ID
5. Confirm SQL Server schema
6. Implement EF Core migrations
7. Replace placeholder authorization query
8. Integrate real APIM and Oracle APIs
9. Configure Azure Key Vault and managed identity
10. Harden security settings
11. Complete observability and auditing
12. Run local validation and automated tests
13. Deploy to non-production Azure environment
14. Execute UAT with client
15. Promote to production

## 3. Machine Prerequisites

Before doing any client work, install the following:

- `.NET 8 SDK`
- `.NET 8 ASP.NET Core Runtime`
- Visual Studio 2022 or later, or VS Code with C# tooling
- SQL Server access tools
- Azure CLI
- Git

Important:

- As of Friday, July 17, 2026, this machine can build the solution using the installed SDK tooling, but `dotnet test` needs the `.NET 8 runtime` installed because the app targets `net8.0`.

## 4. Create the GitHub Repository

Steps:

1. Create a new private GitHub repository.
2. Name it something client-appropriate, for example `fusion-password-reset-portal`.
3. Push this codebase into that repo.
4. Protect the default branch.
5. Require pull requests and code review.
6. Enable secret scanning and dependency alerts.
7. Add branch naming and release conventions.

Recommended branch model:

- `main` for production-ready code
- `develop` for integration testing
- feature branches for implementation work

## 5. Confirm Client Inputs Before Coding Further

Do not continue with production setup until these are confirmed in writing:

- Azure tenant ID
- Entra app registration details
- Allowed user groups or roles
- SQL Server name, database name, schema, and table definitions
- Real meaning of `TblFusionUserPWDRest`
- Real meaning of `TblFusionResetPWDInstance`
- Real Oracle Fusion API endpoints
- APIM subscription/header requirements
- OAuth client credentials flow details
- Password policy rules approved by the client
- Restricted-account rules approved by the client
- Audit retention requirements
- Deployment environment names such as `DEV`, `UAT`, `PROD`

## 6. Configure Azure Entra ID

This application uses Microsoft Entra ID authentication.

Steps:

1. Create or confirm the app registration in the client tenant.
2. Add the redirect URI for local development.
3. Add the redirect URI for Azure App Service.
4. Add the sign-out redirect URI.
5. Decide whether access control uses app roles, security groups, or both.
6. Add required claims to tokens.
7. Record the `TenantId`, `ClientId`, `Instance`, `Domain`, and callback path.

What to update in the repo:

- `src/FusionPasswordResetPortal.Web/appsettings*.json`
- `src/FusionPasswordResetPortal.Web/Extensions/ServiceCollectionExtensions.cs`

Production check:

- Sign-in works
- Sign-out works
- unauthorized users are blocked
- authorized users reach the dashboard

## 7. Confirm and Implement Real SQL Server Design

The current code contains scaffolded EF Core entities and a placeholder authorization query.

You must confirm:

- exact table names
- exact column names
- primary keys
- nullable rules
- indexes
- whether `TblFusionUserPWDRest` is an audit table only or also an authorization source
- whether there is a dedicated access-control table that should be used instead

Files to review:

- `src/FusionPasswordResetPortal.Domain/Entities/FusionPasswordResetAudit.cs`
- `src/FusionPasswordResetPortal.Domain/Entities/FusionResetPasswordInstance.cs`
- `src/FusionPasswordResetPortal.Infrastructure/Data/ApplicationDbContext.cs`
- `src/FusionPasswordResetPortal.Infrastructure/Repositories/UserAuthorizationRepository.cs`

## 8. Create Proper EF Core Migrations

Current status:

- The entities and configurations exist.
- Migrations are not yet created.

Steps:

1. Install the `.NET 8 runtime` if missing.
2. Run `dotnet restore`.
3. Run `dotnet build`.
4. Add the first migration.
5. Review the generated schema carefully.
6. Apply the migration to a development database only.
7. Validate column lengths, datatypes, and indexes with the DBA.

Suggested commands:

```powershell
dotnet restore
dotnet build
dotnet ef migrations add InitialCreate --project .\src\FusionPasswordResetPortal.Infrastructure --startup-project .\src\FusionPasswordResetPortal.Web
dotnet ef database update --project .\src\FusionPasswordResetPortal.Infrastructure --startup-project .\src\FusionPasswordResetPortal.Web
```

Do not run schema changes on client production databases before DBA approval.

## 9. Replace the Placeholder Authorization Logic

Current status:

- Authorization is scaffolded through `IUserAuthorizationService` and `IUserAuthorizationRepository`.
- The SQL used now is a placeholder assumption.

You must replace it with real logic based on the client schema.

Decide which model applies:

1. SQL table contains approved requesters by email
2. SQL table contains role-to-user mapping
3. SQL table stores allowed AD group names
4. Authorization should come entirely from Entra roles/groups and not SQL

Update:

- `src/FusionPasswordResetPortal.Infrastructure/Repositories/UserAuthorizationRepository.cs`
- `Authorization:SqlQuery` in configuration

Production check:

- unauthorized users cannot search
- unauthorized users cannot reset passwords
- authorized users can complete the flow
- audit logs capture allowed and denied attempts

## 10. Integrate the Real Oracle and APIM Contracts

Current status:

- `OracleApiService` is structured correctly for production-style integration.
- The request and response payloads are placeholders.

You must confirm:

- token endpoint
- grant type
- scope
- APIM subscription key/header name
- user search API route
- password reset API route
- request JSON schema
- response JSON schema
- error response schema
- timeout expectations
- retry-safe behavior

Files to update:

- `src/FusionPasswordResetPortal.Infrastructure/Services/OracleApiService.cs`
- `src/FusionPasswordResetPortal.Infrastructure/Options/OracleApiOptions.cs`
- DTOs in `src/FusionPasswordResetPortal.Application/DTOs`

Production check:

- token retrieval works
- user search returns real Oracle data
- password reset works against non-production environment
- failed API calls log useful diagnostic details without exposing secrets

## 11. Configure Azure Key Vault and Managed Identity

Never keep production secrets in `appsettings.json`.

Steps:

1. Create an Azure Key Vault for the client environment.
2. Enable managed identity on the App Service.
3. Grant the managed identity permission to read secrets.
4. Store all secret values in Key Vault.
5. Remove real secrets from config files.
6. Use environment-specific secret names.

Expected secrets:

- APIM subscription key
- Oracle client ID
- Oracle client secret
- SQL connection string if not using integrated identity
- Application Insights connection string if required by policy

Files involved:

- `src/FusionPasswordResetPortal.Web/Program.cs`
- `src/FusionPasswordResetPortal.Infrastructure/Services/SecretProvider.cs`

## 12. Update Environment Configuration Properly

You should maintain separate values for:

- `Development`
- `UAT`
- `Production`

Config files:

- `src/FusionPasswordResetPortal.Web/appsettings.json`
- `src/FusionPasswordResetPortal.Web/appsettings.Development.json`
- `src/FusionPasswordResetPortal.Web/appsettings.Production.json`

Best practice:

- keep only non-secret defaults in source control
- use Azure App Service configuration or Key Vault references for real values

## 13. Harden Security Before Any Client Demo

The scaffold already includes some security middleware, but production hardening still needs confirmation.

Review and finalize:

- Content Security Policy
- cookie settings
- session timeout
- anti-forgery enforcement
- secure headers
- request validation
- password masking and non-logging of secrets
- error handling behavior
- restricted account rules
- audit data sensitivity

Files to review:

- `src/FusionPasswordResetPortal.Web/Middleware/SecurityHeadersMiddleware.cs`
- `src/FusionPasswordResetPortal.Web/Middleware/GlobalExceptionHandlingMiddleware.cs`
- `src/FusionPasswordResetPortal.Application/Services/ValidationService.cs`

## 14. Complete Logging, Tracing, and Auditing

This must be proven before production release.

Validate:

- correlation ID appears on each request
- request tracing is visible in logs
- login attempts are logged
- user searches are logged
- password reset attempts are logged
- failures are logged
- security denials are logged
- secrets and passwords are never logged

Files to review:

- `src/FusionPasswordResetPortal.Web/Program.cs`
- `src/FusionPasswordResetPortal.Web/Middleware/CorrelationIdMiddleware.cs`
- `src/FusionPasswordResetPortal.Application/Services/AuditService.cs`

## 15. Validate the User Experience

Before UAT, test all screens:

1. Login page
2. Dashboard
3. Oracle instance selection
4. User search
5. Password reset
6. Success page
7. Error page
8. Access denied page

Confirm:

- responsive layout works
- validation messages are clear
- failed requests are understandable
- buttons do not double-submit
- sign-out works

## 16. Run Local Build and Test Validation

When the machine has `.NET 8 runtime` installed, use this sequence:

```powershell
dotnet restore
dotnet build
dotnet test
```

Then validate manually:

1. Start the web application
2. Sign in with a test Entra account
3. Confirm dashboard access
4. Search a real test Oracle user
5. Reset a password in a non-production Oracle environment
6. Review audit records
7. Review logs and App Insights traces

## 17. Prepare Azure Deployment

Recommended Azure components:

- Azure App Service
- Azure Key Vault
- Azure SQL Database or SQL Server
- Application Insights
- Log Analytics
- Managed Identity
- APIM

Deployment strategy:

1. Deploy to `DEV`
2. Validate application startup
3. Validate authentication
4. Validate database access
5. Validate Key Vault resolution
6. Validate Oracle integration
7. Promote to `UAT`
8. Run client testing
9. Promote to `PROD`

## 18. Add CI/CD Before Production Release

Minimum pipeline stages:

1. Restore
2. Build
3. Unit tests
4. Integration tests
5. Security/dependency scan
6. Publish artifacts
7. Deploy to DEV
8. Manual approval
9. Deploy to UAT
10. Manual approval
11. Deploy to PROD

Pipeline should also:

- block vulnerable dependencies
- block failed tests
- block direct production deployment without approval

## 19. UAT Checklist

Before go-live, confirm all of the following:

- Entra sign-in works with real client tenant
- only approved users can access reset features
- all intended Fusion instances appear correctly
- user search returns valid Oracle users
- password reset works for allowed accounts
- restricted accounts are blocked
- all actions are audited
- logs are visible in monitoring tools
- no secrets appear in logs or responses
- error pages are safe and user-friendly
- rollback path is documented

## 20. Production Go-Live Checklist

Only go live after:

1. client signs off UAT
2. security review is complete
3. DBA approves schema and permissions
4. production secrets are stored in Key Vault
5. production redirect URIs are configured
6. monitoring and alerts are active
7. support team has runbook access
8. rollback plan is ready

## 21. Immediate Next Steps For This Repository

This is the exact practical sequence I recommend next:

1. Install `.NET 8 runtime`
2. create the GitHub repository
3. push this source
4. collect real client values and API samples
5. update entities and SQL authorization logic
6. create EF migrations
7. implement real Oracle request and response contracts
8. configure Key Vault and Entra values
9. run build and tests
10. deploy to a non-production Azure environment

## 22. Important Reality Check

If you push this repo to GitHub today and only replace placeholder values, that does not guarantee production success.

It will work properly only after:

- real client schema is implemented
- real Oracle/APIM integration is implemented
- authentication and authorization are validated with the client tenant
- secrets are moved to Key Vault
- runtime testing passes in a real environment

That is normal. This repo is a solid production-style foundation, and this README is the procedure to turn it into a real production deployment safely.
