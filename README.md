# GherkinCucumberDemo — PizzaShop

Progetto demo (volutamente semplice) per studiare tre argomenti collegati:

- **[C4 Model](docs/c4-model/README.md)**: come documentare un'architettura software su 4 livelli di dettaglio.
- **[Gherkin & Cucumber](docs/gherkin-cucumber/README.md)**: come scrivere comportamenti/regole di business in
  linguaggio naturale strutturato e trasformarli in test automatici (in .NET, tramite **Reqnroll**).

Il dominio scelto per l'esempio è volutamente semplice: un piccolo servizio di **ordini di pizza** (formati,
ingredienti extra, sconti, consegna), abbastanza ricco da avere regole di business sensate da documentare e
testare, ma senza la complessità di un sistema reale.

## Struttura della solution
```
GherkinCucumberDemo.slnx
GherkinCucumberDemo.csproj / Program.cs   → console app "demo runner": esegue un esempio end-to-end di ordine
PizzaShop.Domain/                         → libreria di dominio: pizze, topping, sconti, consegna
PizzaShop.Bdd.Tests/                      → test BDD: feature Gherkin (.feature) + step definition (Reqnroll + xUnit)
  Features/                               → gli scenari, scritti in italiano
  Steps/                                  → il codice C# che li esegue come test
docs/
  c4-model/                               → documentazione architetturale C4 (diagrammi Mermaid)
  gherkin-cucumber/                       → spiegazione di Gherkin/Cucumber/Reqnroll
```

## Prerequisiti
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Comandi utili
```powershell
# Compilare tutta la solution
dotnet build GherkinCucumberDemo.slnx

# Eseguire la demo console (crea un ordine di esempio e ne stampa il riepilogo)
dotnet run --project GherkinCucumberDemo.csproj

# Eseguire tutti gli scenari Gherkin come test automatici
dotnet test PizzaShop.Bdd.Tests/PizzaShop.Bdd.Tests.csproj
```

## Regole di business del dominio (PizzaShop.Domain)
- Ogni formato di pizza (`Small`, `Medium`, `Large`) ha un prezzo base; ogni ingrediente extra aggiunge un costo.
- Una pizza non può avere più di **5 ingredienti extra**.
- Un ordine riceve uno **sconto del 10%** se il subtotale supera i **30 €**.
- La consegna è **gratuita** se il totale (dopo sconto) è **almeno 25 €**, altrimenti costa **3,50 €**.

Queste stesse regole sono descritte come scenari Gherkin in `PizzaShop.Bdd.Tests/Features/` ed eseguite come test
automatici da Reqnroll — vedi [docs/gherkin-cucumber/README.md](docs/gherkin-cucumber/README.md) per i dettagli.

## Documentazione architetturale
L'architettura (per quanto semplice) di questo progetto è documentata con il C4 Model in
[docs/c4-model/](docs/c4-model/README.md), a partire dalla vista di contesto fino ad arrivare ai componenti della
libreria di dominio.
