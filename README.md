# Projekt_ASP

## Opis projektu

Projekt_ASP to aplikacja webowa napisana w **ASP.NET Core 8**, pozwalająca na zarządzanie ogłoszeniami w różnych kategoriach (motoryzacja, nieruchomości, praca, elektronika itd.).  
Projekt wykorzystuje **SQLite** jako bazę danych lokalną i wspiera **autentykację użytkowników**.

Projekt został skonfigurowany pod **CI/CD** z użyciem **GitHub Actions** i **Azure App Service**, dzięki czemu aplikacja jest automatycznie wdrażana po każdym pushu do gałęzi `master`.

---

## Technologie

- ASP.NET Core 8
- Entity Framework Core z SQLite
- Razor Pages / MVC
- GitHub Actions (CI/CD)
- Azure App Service (CD)

---

## Wymagania

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 / VS Code
- Git
- Konto Azure (opcjonalnie, do wdrożenia)

---

## Instrukcja uruchomienia lokalnie

1. **Sklonuj repozytorium:**

```bash
git clone https://github.com/Martivial/Projekt_ASP.git
cd Projekt_ASP/Projekt_ASP
