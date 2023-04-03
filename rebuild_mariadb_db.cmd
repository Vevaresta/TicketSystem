@echo off

for /f "tokens=8 delims==;" %%a in ('type Ticketsystem\appsettings.json ^| find /i "MariaDbContextConnection"') do (
    set "PASSWORD=%%~a"
)

for /f "delims=" %%i in ('where /r "C:\Program Files" "mysql.exe" 2^>nul') do (
    set "MYSQL=%%i"
)

"%MYSQL%" -u root --password="%PASSWORD%" -e "DROP DATABASE ticketsystem;"


cd Ticketsystem

rmdir /q /s Migrations

dotnet ef migrations add InitialCreate

dotnet ef database update

cd ..