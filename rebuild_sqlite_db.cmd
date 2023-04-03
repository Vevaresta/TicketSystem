@echo off

cd Ticketsystem

rmdir /q /s Migrations
del /q /s *.db

dotnet ef migrations add InitialCreate

dotnet ef database update

cd ..