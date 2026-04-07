# Student OS → Blazor starter

Tohle je první použitelný základ migrace z `student-os` (Next.js) do **Blazor Web App (.NET 8)**.

## Co už je připravené

- Blazor Web App skeleton
- EF Core + SQLite modely
- základní dashboard service
- layout + navigace + theme toggle
- dashboard page
- localStorage wrapper přes JS interop
- Tailwind setup, aby šlo držet stejný vzhled jako v původním projektu

## Co je zatím jen starter / phase 1

- Planner / Subjects / Stats jsou založené a mají základní tok
- část komponent je přepsaná jako první verze, ne finální pixel-perfect kopie
- pro 1:1 finální přepis bude potřeba dopracovat všechny formuláře a lokální widgety

## Jak to spustit

```bash
dotnet restore
npm install
npm run css:build
dotnet ef database update
dotnet run
```

Pro vývoj stylů:

```bash
npm run css:watch
```

## Doporučené další kroky

1. převést seed data z Prisma do EF Core seed / initializeru
2. dopřepsat PlannerExamForm, SubjectForm, StudySessionForm
3. napojit Stats page na finální agregace
4. doladit timer, reward shop, hydration tracker a quick notes do 1:1 chování
