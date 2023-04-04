@echo off

for /f "tokens=10 delims==;" %%a in ('type Ticketsystem\appsettings.json ^| find /i "PostgreSQLContextConnection" ^| find /i "Password"') do (
    set "PGPASSWORD=%%~a"
)

for /f "delims=" %%i in ('where /r "C:\Program Files\PostgreSQL" "dropdb.exe" 2^>nul') do (
    set "DROPDB=%%i"
)

"%DROPDB%" -U postgres --if-exists ticketsystem

cd Ticketsystem

rmdir /q /s Migrations

dotnet ef migrations add InitialCreate

dotnet ef database update

cd ..