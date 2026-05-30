# Student OS

Student OS je webová aplikace pro plánování zkoušek, sledování studia a motivaci během semestru nebo zkouškového období.

Aplikace pomáhá studentovi mít na jednom místě:

- přehled předmětů,
- termíny zkoušek a zápočtů,
- záznamy studijních session,
- focus timer,
- denní a týdenní statistiky,
- rychlé poznámky,
- jednoduchý reward systém.

Projekt vznikl jako seminární práce do předmětu UUR.

---

## Hlavní funkce

### Dashboard

Dashboard slouží jako hlavní přehled dne.

Obsahuje:

- dnešní odstudovaný čas,
- celkový čas za poslední týden,
- celkový odstudovaný čas,
- focus timer,
- denní goal tracker,
- hydration tracker,
- graf studia za poslední týden,
- nejbližší aktivní termíny na následujících 14 dní,
- rychlé poznámky.

Dashboard je záměrně jednodušší a slouží hlavně pro rychlou orientaci.

---

### Planner

Planner slouží ke správě zkoušek a termínů.

Uživatel může:

- přidat nový termín,
- vybrat předmět,
- nastavit datum a čas,
- zvolit typ termínu,
- nastavit délku,
- přidat poznámku k termínu,
- upravit existující termín,
- přepnout stav termínu,
- smazat termín.

Poznámka k termínu je určená například pro téma zápočtu, rozsah učiva, místnost nebo další doplňující informace.

---

### Subjects

Stránka Subjects slouží ke správě předmětů a ručnímu záznamu studia.

Uživatel může:

- vytvořit předmět,
- zvolit barvu předmětu,
- upravit nebo smazat předmět,
- ručně přidat studijní session,
- zobrazit hierarchický přehled dat podle předmětů.

Každý předmět obsahuje své termíny a studijní záznamy.

---

### Stats

Stats stránka slouží k detailnějšímu vyhodnocení studia.

Obsahuje:

- souhrn vybraného týdne,
- celkový odstudovaný čas,
- počet kreditů,
- graf podle týdne,
- navigaci mezi týdny pomocí šipek,
- graf podle předmětů,
- reward shop.

Dashboard ukazuje jen jednoduchý aktuální přehled, zatímco Stats slouží pro hlubší analýzu.

---

### Focus timer

Focus timer slouží k měření soustředěného studia.

Funguje jako jednoduchý Pomodoro systém:

- pracovní session,
- pauza,
- automatické přepnutí práce/pauza,
- uložení studijní session po dokončení práce,
- zvukové pípnutí po doběhnutí timeru,
- uložení stavu timeru do prohlížeče.

Timer neodečítá pouze lokální počítadlo, ale počítá podle cílového času. Díky tomu zůstává přesný i při drobném zpoždění překreslení UI.

---

### Reward shop

Reward shop převádí odstudovaný čas na kredity.

Uživatel může:

- sledovat celkový počet kreditů,
- kupovat odměny,
- vytvářet vlastní odměny,
- mazat vlastní odměny.

Výchozí poměr je 1 kredit za 45 minut studia.

---

## Technologický stack

- Blazor Web App
- .NET 10.0
- C#
- Entity Framework Core
- SQLite
- JavaScript interop
- Browser localStorage
- Tailwind CSS

---

## Architektura projektu

Projekt je rozdělený na několik hlavních částí.

### Components/Pages

Obsahuje hlavní stránky aplikace:

- `Dashboard.razor`
- `Planner.razor`
- `Subjects.razor`
- `Stats.razor`

Tyto komponenty řeší hlavně zobrazení dat a reakce na uživatelské akce.

---

### Components/Shared

Obsahuje sdílené UI komponenty:

- `FocusTimer.razor`
- `GoalTracker.razor`
- `HydrationTracker.razor`
- `QuickNotes.razor`
- `RewardShop.razor`
- `StudyBarChart.razor`
- `StudyChartSwitcher.razor`
- `ThemeToggle.razor`

Sdílené komponenty umožňují znovupoužít části UI na více místech aplikace.

---

### Services

Obsahuje aplikační logiku a práci s daty:

- `DashboardService`
- `SubjectService`
- `ExamService`
- `StudySessionService`
- `BrowserStorageService`
- `ThemeService`

Služby oddělují práci s databází a browser storage od Razor komponent.

---

### Data

Obsahuje databázový kontext a datové modely:

- `AppDbContext`
- `Subject`
- `Exam`
- `StudySession`

Data jsou ukládána do SQLite databáze `student-os.db`.

---

## Datový model

### Subject

Předmět obsahuje:

- `Id`
- `Name`
- `Color`
- seznam zkoušek,
- seznam studijních session.

Jeden předmět může mít více zkoušek a více studijních záznamů.

---

### Exam

Termín obsahuje:

- `Id`
- `Date`
- `Type`
- `Duration`
- `Note`
- `IsDone`
- `SubjectId`

Poznámka je volitelná a může mít maximálně 500 znaků.

---

### StudySession

Studijní session obsahuje:

- `Id`
- `Duration`
- `CreatedAt`
- `SubjectId`

Studijní session vzniká buď ručně na stránce Subjects, nebo automaticky po dokončení focus timeru.

---

## Ukládání dat

Aplikace používá dva způsoby ukládání dat.

### SQLite databáze

Do databáze se ukládají hlavní aplikační data:

- předměty,
- termíny,
- studijní session.

Databáze se vytváří automaticky při startu aplikace pomocí `EnsureCreatedAsync()`.

### localStorage

Do `localStorage` se ukládají drobná uživatelská nastavení:

- aktuální theme,
- rychlé poznámky,
- nastavení goal trackeru,
- stav hydration trackeru,
- stav focus timeru,
- vlastní odměny,
- utracené kredity.

---

## Spuštění projektu

### 1. Obnovení .NET balíčků

```bash
dotnet restore
