# Student OS

Student OS je webová aplikace vytvořená jako semestrální práce pro předmět KIV/UUR.

Aplikace slouží k plánování termínů zkoušek, evidenci studia a zobrazení základních statistik studia.

Autor: Václav Chuchlík

## Hlavní části aplikace

Aplikace obsahuje čtyři hlavní pohledy:

- Dashboard – denní přehled, focus timer, nejbližší termíny a týdenní graf
- Planner – správa termínů zkoušek a zápočtů
- Subjects – správa předmětů a ruční evidence studia
- Stats – statistiky podle týdnů a předmětů, kredity a odměny

## Použité technologie

- Blazor Web App
- .NET 10
- C#
- Entity Framework Core
- SQLite
- Tailwind CSS
- localStorage

## Požadavky pro spuštění

Pro spuštění aplikace je potřeba mít nainstalované:

- .NET SDK 10
- Node.js
- npm

## Spuštění aplikace

V kořenové složce projektu spusťte následující příkazy:

```bash
dotnet restore
```

```bash
npm install
```

```bash
npm run css:build
```

```bash
dotnet run
```

Po spuštění aplikace se v terminálu zobrazí lokální adresa, na které je aplikace dostupná.

Například:

```text
http://localhost:5000
```

## Databáze

Aplikace používá lokální SQLite databázi:

```text
student-os.db
```

Databáze se vytvoří automaticky při prvním spuštění aplikace.

## Struktura projektu

```text
Components/Pages      hlavní stránky aplikace
Components/Shared     sdílené komponenty
Services              aplikační logika a práce s daty
Data                  datové modely a databázový kontext
Styles                zdrojové styly
wwwroot/js            JavaScript helpery
```
