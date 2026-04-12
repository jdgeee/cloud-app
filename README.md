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
* [x] **Artefakt 7:** Skonfigurowana aplikacja z KeyVaultem.
* [x] **Artefakt 8:** Dokumentacja techniczna API (Swagger UI) dostępna publicznie.

## 🚀 Adres aplikacji

| Komponent | URL |
| :--- | :--- |
| **Frontend (React)** | https://cloud-task-manager-frontend-96593-b5hte0dbgkdvadgt.polandcentral-01.azurewebsites.net |
| **Backend API (Swagger UI)** | https://cloud-task-manager-api-96593-hhgbfabsg3fmhdbe.polandcentral-01.azurewebsites.net/swagger |

---

## 9.5. Profesjonalna dokumentacja w chmurze

### 1. Architektura Systemu (High-Level Design)

Projekt realizuje architekturę 3-warstwową wdrożoną w całości na platformie Microsoft Azure:

| Warstwa | Technologia | Usługa Azure |
| :--- | :--- | :--- |
| **Prezentacji** | React 19 (Vite) | Azure App Service (Web) |
| **Logiki** | .NET 9 Web API | Azure App Service (API) |
| **Danych** | Azure SQL Database | Azure SQL (Serverless, General Purpose) |

### 2. Infrastruktura i Bezpieczeństwo Sieciowe

Oto jak zależności między komponentami są zabezpieczone:

- **Managed Identity:** Kluczowy element architektury — eliminuje hasła w plikach konfiguracyjnych przez co tracimy bezpieczeństwo naszej aplikacji Azure. Backend komunikuje się z Key Vault bez żadnych zakodowanych sekretów.
- **Key Vault:** Miejsce, w którym przechowujemy parametry połączenia (`Connection String`), do których tylko nasz Backend ma dostęp.
- **CORS:** Backend akceptuje zapytania wyłącznie z domeny frontendu.

### 3. Dokumentacja Wdrożenia (DevOps)

Opis procesu automatyzacji, który stosujemy:

- **CI/CD:** Każdy push uruchamia pipeline w **GitHub Actions** (`.github/workflows/`), który buduje kod i publikuje go do Azure App Service.
- **IaC (Infrastructure as Code):** Docelowo całe środowisko powinno być zdefiniowane w pliku Bicep (np. `infrastructure/main.bicep`), który odwzorowuje całe środowisko jako kod, co pozwala na jego pełną odtwarzalność.
- **Monitoring (Observability):** Azure Application Insights / Log Analytics zbiera logi z obu warstw aplikacji.

### 4. Instrukcja Utrzymania (co robić, gdy coś nie działa)

- **Logowanie:** Wszystkie błędy z Frontendu i Backendu trafiają do konsoli Azure Log Analytics — tam szukamy błędów w pierwszej kolejności.
- **Self-Healing:** Opisujemy mechanizm `EvaluateOnFailure`/`Polly`, który automatycznie "budzi" bazę danych w modelu Serverless i ponawia żądanie użytkownika bez jego wiedzy.
- **Restart usługi:** Azure Portal → App Service → Restart.

### 5. Słownik Zasobów (Resource Inventory)

Aby łatwo zarządzać kosztami i audytem, poniższa tabela opisuje wszystkie zasoby w projekcie:

| Zasób (Service) | Rola w projekcie | Skala (Tier/SKU) |
| :--- | :--- | :--- |
| **Azure SQL** | Przechowywanie zadań użytkownika | Serverless (General Purpose) |
| **App Service (API)** | Backend aplikacji i logika biznesowa | Free / Basic B1 |
| **App Service (Web)** | Interfejs użytkownika (React) | Free / Basic B1 |
| **Key Vault** | Bezpieczny magazyn sekretów i haseł | Standard |
