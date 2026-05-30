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
