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
- Služba už nepočítá `TotalLifetimeCredits` pomocí pevné hodnoty `45`.

Proč: poměr minut za jeden kredit si uživatel nastavuje v prohlížeči, takže výpočet kreditů nemá být natvrdo v databázové službě.

### `Services/ExamService.cs`

- Seznam termínů nevrací termíny archivovaných předmětů.

Proč: Planner nemá po archivaci předmětu dál zobrazovat jeho termíny jako běžné aktivní záznamy.

### `Services/StudySessionService.cs`

- Přidány metody `UpdateAsync` a `DeleteAsync` pro úpravu a mazání jednotlivých studijních session.

Proč: další krok je umožnit opravy nebo mazání chybně zadaných studijních záznamů.

### `Components/Pages/Subjects.razor`

- Seznam studijních session už nepoužívá `Take(10)` a zobrazí celou historii předmětu.
- U každé session jsou tlačítka `Upravit` a `Smazat`.
- Stávající formulář pro ruční záznam studia se používá i pro úpravu existující session.
- Přibyl stav `_isEditingSession`, `SessionFormModel.Id` a malé metody `StartEditSession`, `DeleteSessionAsync`, `CancelSessionEdit`.

Proč: uživatel může opravit nebo smazat chybně zadanou studijní session bez nové stránky a bez snapshotů.

### `Components/Pages/Planner.razor`

- Formulář pro přidání termínu už není pořád vidět na stránce.
- Formulář se zobrazí až po kliknutí na `Přidat termín` nebo při úpravě existujícího termínu.
- Když formulář není otevřený, seznam termínů využije celou šířku stránky.

Proč: zadání požadovalo, aby přidání nového záznamu bylo spíš on demand a aby se místo využilo pro zobrazení celého seznamu.

### `Components/Pages/Stats.razor`

- Karta `Kredity` byla přejmenována na `Získané kredity celkem`.
- Výpočet získaných kreditů používá nastavení `userRewardRatio` z localStorage.
- Hodnota se předává i do `RewardShop`.

Proč: tato hodnota ukazuje všechny kredity získané studiem a musí respektovat nastavení `1 kredit za X minut`.

### `Components/Shared/RewardShop.razor`

- Zůstatek v reward shopu je nově označený jako `Aktuálně dostupné kredity`.

Proč: v reward shopu se zobrazuje použitelný počet kreditů po odečtení kreditů utracených za odměny.

### `Components/Pages/Dashboard.razor`

- `FocusTimer` dostává jen nearchivované předměty.

Proč: archivovaný předmět už nemá jít vybrat pro nové měření studia.

## Kdyby zbyl čas

- Přidat potvrzovací dialog před archivací předmětu a před smazáním studijní session.

Proč: aktuální řešení funguje, ale potvrzení by snížilo riziko omylem kliknutého smazání.

## Poznámka

Tahle varianta nepřidává snapshoty do `StudySession`. Je jednodušší: předmět zůstává v databázi, jen se skryje z aktivního UI.
