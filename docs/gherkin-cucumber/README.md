# Gherkin, Cucumber e Reqnroll — guida rapida con l'esempio PizzaShop

## Cos'è Gherkin
**Gherkin** è un linguaggio (una sintassi) per descrivere comportamenti/regole di business in linguaggio naturale
ma strutturato, leggibile sia da persone non tecniche (product owner, QA, cliente) sia da un tool che lo esegue
come test automatico. È alla base della pratica chiamata **BDD (Behavior-Driven Development)**.

Un file Gherkin si chiama **feature file** (estensione `.feature`) ed è composto da:

- `Feature` (in italiano `Funzionalità`): il titolo e una breve descrizione della funzionalità documentata.
- `Scenario`: un caso concreto, con una sequenza di passi.
- `Given` / `When` / `Then` (in italiano `Dato che` / `Quando` / `Allora`): rispettivamente la **premessa** (stato
  iniziale), l'**azione** compiuta e il **risultato atteso**. `And`/`But` (`E`/`Ma`) concatenano più passi dello
  stesso tipo.
- `Scenario Outline` (in italiano `Schema dello scenario`) + `Examples` (`Esempi`): permette di ripetere lo stesso
  scenario con più combinazioni di dati, evitando di duplicare gli scenari a mano.
- `# language: it`: un commento speciale in testa al file che dice al parser di usare le parole chiave italiane
  invece di quelle inglesi di default. Gherkin supporta [decine di lingue](https://cucumber.io/docs/gherkin/languages/).

Esempio tratto dal progetto (vedi [`LimiteIngredienti.feature`](../../PizzaShop.Bdd.Tests/Features/LimiteIngredienti.feature)):

```gherkin
# language: it
Funzionalità: Limite massimo di ingredienti extra su una pizza

  Scenario: Rifiuto di un ingrediente oltre il limite consentito
	Dato che ordino una pizza di formato "Large"
	E che la pizza ha già 5 ingredienti extra
	Quando provo ad aggiungere un ulteriore ingrediente extra "Mozzarella"
	Allora dovrebbe essere sollevato un errore che segnala il superamento del limite di ingredienti extra
```

## Cos'è Cucumber (e come si collega a Gherkin)
**Cucumber** è la famiglia di strumenti open source (nata per Ruby, oggi disponibile per Java, JavaScript, ecc.)
che **legge i file `.feature`** scritti in Gherkin e **li esegue come test automatici**, abbinando ogni singola
riga (`Given`/`When`/`Then`) a un pezzo di codice scritto dallo sviluppatore, chiamato **step definition**.

Quindi la comprensione di partenza era corretta: **Cucumber serve esattamente a questo** — trasformare gli
scenari scritti in linguaggio naturale (Gherkin) in test automatici eseguibili, fungendo da ponte tra:
- la **documentazione vivente** (leggibile da chiunque, sempre aggiornata perché è anche il test), e
- i **test di regressione automatici** (eseguibili in CI/CD, con `dotnet test` come in questo progetto).

## E Reqnroll dov'è in tutto questo?
**Reqnroll** è l'implementazione **.NET-native** di questo stesso paradigma. Non è "Cucumber" in senso stretto
(che è nato per altri linguaggi), ma segue la stessa specifica Gherkin ed ha lo stesso ruolo:

1. Legge i file `.feature` (qui in `PizzaShop.Bdd.Tests/Features/`).
2. A build-time genera automaticamente il codice C# che trasforma ogni scenario in un test xUnit
   (`*.feature.cs`, generati e non da modificare a mano).
3. A runtime, ogni riga dello scenario viene abbinata (tramite espressioni regolari) a un metodo C# annotato con
   `[Given]`, `[When]` o `[Then]`, chiamato **step definition** (qui in `PizzaShop.Bdd.Tests/Steps/`).
4. `dotnet test` esegue quei test esattamente come farebbe con un test xUnit scritto a mano.

Reqnroll è il **successore attivamente mantenuto** di SpecFlow (che è stato lo strumento storico di riferimento
per il BDD in .NET, ma è ormai deprecato): la sintassi degli attributi (`[Binding]`, `[Given]`, `[When]`,
`[Then]`) è quasi identica, per rendere la migrazione semplice.

## Come sono organizzati gli scenari in questo progetto
| Feature file | Cosa verifica |
|---|---|
| [`CalcoloPrezzoPizza.feature`](../../PizzaShop.Bdd.Tests/Features/CalcoloPrezzoPizza.feature) | Il prezzo di una pizza in base a formato e ingredienti extra (`Schema dello scenario` con una tabella di `Esempi`) |
| [`LimiteIngredienti.feature`](../../PizzaShop.Bdd.Tests/Features/LimiteIngredienti.feature) | Il limite massimo di 5 ingredienti extra per pizza |
| [`ScontoEConsegna.feature`](../../PizzaShop.Bdd.Tests/Features/ScontoEConsegna.feature) | Lo sconto sull'ordine e la soglia di consegna gratuita |

Le step definition corrispondenti si trovano in `PizzaShop.Bdd.Tests/Steps/`:
- `CommonPizzaSteps.cs`: passi condivisi tra più feature (creare una pizza, aggiungere ingredienti) — tenuti in
  un'unica classe per evitare che Reqnroll segnali un "ambiguous step" quando lo stesso testo compare in più
  feature file.
- `PizzaPricingSteps.cs`, `ToppingLimitSteps.cs`, `OrderSteps.cs`: le verifiche (`Then`) specifiche di ciascuna
  feature.
- `PizzaOrderContext.cs`: un piccolo oggetto condiviso (tramite dependency injection nativa di Reqnroll, la
  cosiddetta *context injection*) usato per passare stato (la pizza/ordine in costruzione) tra le diverse classi
  di step definition all'interno dello stesso scenario, senza duplicare i passi.

## Come eseguire i test
```powershell
dotnet test PizzaShop.Bdd.Tests/PizzaShop.Bdd.Tests.csproj
```
Ogni riga `Scenario`/ogni riga della tabella `Esempi` diventa un test a sé stante, visibile anche nel Test
Explorer di Visual Studio con il testo dello scenario come nome del test.
