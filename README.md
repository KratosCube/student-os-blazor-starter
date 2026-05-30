# Student OS

Student OS je webová aplikace pro plánování zkoušek, sledování studia a motivaci během semestru.

Autor: Václav Chuchlík  
Semestrální práce do předmětu KIV/UUR

## Co aplikace umí

Aplikace obsahuje čtyři hlavní části:

Dashboard  
Rychlý přehled dne, focus timer, nejbližší termíny, týdenní graf, poznámky, denní cíl a hydratace.

Planner  
Správa termínů zkoušek a zápočtů. U termínu lze nastavit předmět, datum, čas, typ, délku, stav a poznámku.

Subjects  
Správa předmětů a ruční zapisování studijních session.

Stats  
Statistiky studia podle týdnů a podle předmětů, kredity a reward shop.

## Technologie

Projekt používá:

- Blazor Web App
- .NET 10
- C#
- Entity Framework Core
- SQLite
- localStorage
- Tailwind CSS

## Spuštění aplikace

Nejdřív obnov .NET balíčky:

```bash
dotnet restore
```

Nainstaluj frontend závislosti:

```bash
npm install
```

Sestav CSS:

```bash
npm run css:build
```

Spusť aplikaci:

```bash
dotnet run
```

Po spuštění se v terminálu vypíše adresa aplikace, například:

```text
http://localhost:5000
```

nebo:

```text
https://localhost:5001
```

## Vývoj CSS

Pokud upravíš styly v `Styles/input.css`, znovu spusť:

```bash
npm run css:build
```

Pro průběžné sledování změn lze použít:

```bash
npm run css:watch
```

## Databáze

Aplikace používá lokální SQLite databázi:

```text
student-os.db
```

Databáze se vytvoří automaticky při prvním spuštění aplikace a naplní se ukázkovými daty.

Pokud chceš začít s čistou databází, smaž soubory:

```text
student-os.db
student-os.db-shm
student-os.db-wal
```

a spusť aplikaci znovu.

## Struktura projektu

```text
Components/Pages      hlavní stránky aplikace
Components/Shared     sdílené komponenty
Services              práce s daty a aplikační logika
Data                  databázový kontext a modely
Styles                zdrojové CSS/Tailwind styly
wwwroot/js            JavaScript helpery
```

## Poznámka

Projekt je lokální studentská aplikace. Hlavní data se ukládají do SQLite databáze, menší uživatelská nastavení a stav některých komponent se ukládají do `localStorage`.
