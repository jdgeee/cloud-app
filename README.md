Cloud Task Manager - Błażej Wasiński 

  

Projekt natywnej aplikacji chmurowej realizowany w architekturze 3-warstwowej. 

  

## Deklaracja Architektury (Mapowanie Azure) 
Ten projekt został zaplanowany z myślą o usługach PaaS (Platform as a Service) w chmurze Azure. 

  

| Warstwa | Komponent Lokalny | Usługa Azure | 
| :--- | :--- | :--- | 
| **Presentation** | React 19 (Vite) | Azure Static Web Apps | 
| **Application** | API (.NET 9 / Node 24) | Azure App Service | 
| **Data** | SQL Server (Dev) | Azure SQL Database (Serverless) | 

  

## 🏗 Status Projektu i Dokumentacja 
* [x] **Artefakt 1:** Zaplanowano strukturę folderów i diagram C4 (dostępny w `/docs`). 
* [x] **Artefakt 2:** Konfiguracja środowiska Docker (w trakcie...). 
* [x] **Artefakt 3:** Działająca warstwa prezentacji (React + Vite w Docker). 
* [x] **Artefakt 4:** Działające wersja logiki backendu. 
* [x] **Artefakt 5:** Trwałość danych i profesjonalny  kontrakt API(EF Migrations + DTO + UI Form). 
* [x] **Artefakt 6:** Wdrożenie aplikacji w chmurze Azure (Azure SQL + App Service backend/frontend + CORS).

  

> **Informacja:** Ten plik będzie ewoluował. W kolejnych etapach dodamy tutaj sekcje 'Quick Start', opis zmiennych środowiskowych oraz instrukcję wdrożenia (CI/CD). 

 