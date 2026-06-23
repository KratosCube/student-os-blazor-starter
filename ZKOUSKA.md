# ZKOUSKA.md

## Ochrana historie při mazání předmětu

Cíl změny: smazání předmětu nemá smazat jeho studijní historii, odstudovaný čas ani kredity ze statistik.

## Co bylo změněno

### `Data/Models/Subject.cs`

- Přidán boolean příznak `IsArchived`.

Proč: předmět se nebude fyzicky mazat z databáze. Jen se označí jako archivovaný, takže jeho `StudySession` záznamy a termíny zůstanou pořád navázané.

### `Data/AppDbContext.cs`

- Pro `Subject.IsArchived` je nastavený default `false`.

Proč: nově vytvořené předměty mají být automaticky aktivní.

### `Program.cs`

- Přidán helper `EnsureSubjectArchiveColumnAsync` podobně jako existující helper pro `Exam.Note`.
- Helper při startu zkontroluje tabulku `Subjects`.
- Pokud sloupec `IsArchived` neexistuje, doplní ho přes:

```sql
ALTER TABLE Subjects ADD COLUMN IsArchived INTEGER NOT NULL DEFAULT 0
```

Proč: projekt nepoužívá EF migrace, takže je zachovaný stejný jednoduchý přístup jako u sloupce `Note`.

### `Services/SubjectService.cs`

- `GetAllAsync` vrací jen aktivní/nearchivované předměty.
- `AddAsync` nastavuje nový předmět jako aktivní.
- `UpdateAsync` upravuje jen aktivní předmět.
- `DeleteAsync` už předmět fyzicky nemaže, ale nastaví `IsArchived = true`.

Proč: uživatelsky se předmět chová jako smazaný, ale data v databázi zůstávají zachovaná.

### `Services/DashboardService.cs`

- Statistiky zůstaly řešené původním stylem přes předměty a jejich session.
- Do aktivních termínů se nově nezařazují termíny archivovaných předmětů.

Proč: termíny archivovaného předmětu už nemají být v dashboardu jako aktivní povinnosti.

### `Services/ExamService.cs`

- Seznam termínů nevrací termíny archivovaných předmětů.

Proč: Planner nemá po archivaci předmětu dál zobrazovat jeho termíny jako běžné aktivní záznamy.

### `Components/Pages/Dashboard.razor`

- `FocusTimer` dostává jen nearchivované předměty.

Proč: archivovaný předmět už nemá jít vybrat pro nové měření studia.

## Poznámka

Tahle varianta nepřidává snapshoty do `StudySession`. Je jednodušší: předmět zůstává v databázi, jen se skryje z aktivního UI.
