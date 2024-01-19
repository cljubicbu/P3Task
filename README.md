# P3Task

Requirements:
- .NET 8
- MSSQL DB (Docker or other)

Entity framework migrations should be run on the database after the connection string to the database has been set in appsettings.json

If running docker database create image with following command and then connection string can stay the same:
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Sasasa%123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

Migrations:
Run following command inside P3Task.DocumentManagement.Infrastructure:
dotnet ef --startup-project ../P3Task.DocumentManagement.Grpc/ database update

Project can now be run in VS/Rider

