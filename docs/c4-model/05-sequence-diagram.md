# Livello 4 — Sequence Diagram

Per il livello 4 (Code) i requisiti di documentazione prevedono, oltre ai 4 livelli standard del C4 Model,
**almeno un sequence diagram e un class diagram** ("UML puro"). Il [class diagram](04-code-level-note.md) è già
documentato; questo file aggiunge il sequence diagram, con la sintassi `sequenceDiagram` di Mermaid.

Il diagramma descrive il **caso d'uso scelto per la demo**: *"Il cliente compone un ordine con una o più pizze,
personalizzate con ingredienti extra entro un limite massimo, e ne viene calcolato il totale finale (spese di
consegna incluse)"* — lo stesso caso d'uso rappresentato nei diagrammi [Context](01-system-context.md),
[Container](02-container-diagram.md) e [Component](03-component-diagram.md).

```mermaid
---
title: "Sequence Diagram: composizione di un ordine e calcolo del totale"
---
sequenceDiagram
	actor Cliente
	participant OrderCompositionService
	participant Order
	participant Pizza
	participant ToppingCatalog
	participant PricingSettingsRepository as IPricingSettingsRepository
	participant PizzaSizeRepository as IPizzaSizeRepository

	Cliente->>OrderCompositionService: CreateOrderAsync(nomeCliente, richiestePizze)
	OrderCompositionService->>PricingSettingsRepository: GetAsync()
	PricingSettingsRepository-->>OrderCompositionService: PricingSettings
	OrderCompositionService->>Order: new Order(nomeCliente)

	loop Per ogni PizzaOrderRequest
		OrderCompositionService->>PizzaSizeRepository: GetBasePriceAsync(formato)
		PizzaSizeRepository-->>OrderCompositionService: prezzo base
		OrderCompositionService->>Pizza: new Pizza(formato, prezzo base)
		loop Per ogni nome di ingrediente extra richiesto
			OrderCompositionService->>ToppingCatalog: GetAsync(nomeIngrediente)
			ToppingCatalog-->>OrderCompositionService: Topping
			OrderCompositionService->>Pizza: AddTopping(topping, settings)
		end
		OrderCompositionService->>Order: AddPizza(pizza)
	end

	OrderCompositionService-->>Cliente: Order

	Cliente->>OrderCompositionService: CalculateTotalsAsync(order)
	OrderCompositionService->>PricingSettingsRepository: GetAsync()
	PricingSettingsRepository-->>OrderCompositionService: PricingSettings
	OrderCompositionService->>Order: Subtotal()
	Order-->>OrderCompositionService: subtotale
	OrderCompositionService->>Order: DeliveryFee(settings)
	Order-->>OrderCompositionService: spesa di consegna
	OrderCompositionService->>Order: GrandTotal(settings)
	Order-->>OrderCompositionService: totale finale

	OrderCompositionService-->>Cliente: OrderTotals
```

> Il titolo `Sequence Diagram: composizione di un ordine e calcolo del totale` è definito nel frontmatter
> `title:` del blocco Mermaid (non solo nel testo Markdown sopra): Mermaid lo disegna *dentro* l'immagine
> renderizzata, sopra il diagramma stesso, esattamente come già fatto per il [class diagram](04-code-level-note.md).
> Questo rende esplicito il caso d'uso rappresentato anche se il diagramma viene esportato o incollato altrove
> come immagine isolata — è la stessa esigenza, ma qui ancora più sentita: un sequence diagram descrive *un solo*
> scenario specifico, quindi sapere a colpo d'occhio quale caso d'uso stiamo guardando è particolarmente utile.

## Note di lettura
- `actor` / `participant`: gli attori/oggetti coinvolti nello scambio di messaggi. Qui `Cliente` è la persona che
  usa il sistema, gli altri sono le classi/interfacce C# reali che compongono l'**Order Management Component**
  del [Component Diagram](03-component-diagram.md), descritte nel dettaglio nel
  [class diagram](04-code-level-note.md). `Cliente` non parla mai direttamente con i repository
  (`IPizzaSizeRepository`, `IPricingSettingsRepository`), con `ToppingCatalog`, né con `Pizza`/`Order`: parla
  solo con `OrderCompositionService`, che è il vero consumer di quelle dipendenze e l'unico che crea/assembla
  un `Order` (vedi [04-code-level-note.md](04-code-level-note.md) e
  [04a-class-diagram-spiegazione.md](04a-class-diagram-spiegazione.md#410-ordercompositionservice)).
- `loop ... end`: rappresenta un'iterazione (qui: più pizze da comporre, e più ingredienti extra per pizza).
- `alt ... else ... end`: non compare più in questo diagramma, perché la diramazione "consegna gratuita o a
  pagamento" è ora incapsulata dentro `Order.DeliveryFee(settings)`, invocato come singola chiamata da
  `OrderCompositionService` — resta comunque descritta nel dettaglio in
  [Order.cs](../../PizzaShop.Domain/Order.cs) e nella sezione 4 del [class diagram](04-code-level-note.md).
- Le frecce continue (`->>`) sono chiamate sincrone; le frecce tratteggiate (`-->>`) sono le risposte/ritorni.
- `Cliente->>OrderCompositionService: CreateOrderAsync(nomeCliente, richiestePizze)` è l'**unica** chiamata che
  il cliente fa per comporre l'intero ordine: gli basta descrivere *cosa* vuole (una collezione di
  `PizzaOrderRequest`, ciascuna con formato e nomi dei topping desiderati). Non chiama mai `new Order(...)`,
  `new Pizza(...)` o `order.AddPizza(...)` direttamente: tutta questa orchestrazione avviene dentro
  `OrderCompositionService`, che risolve le impostazioni di pricing **una sola volta** (`settings`) e le
  riusa per ogni `Pizza.AddTopping(...)` del ciclo, coerentemente con la scelta di design spiegata in
  [04-code-level-note.md](04-code-level-note.md) (`Pizza` e `Order` restano entità "pure", senza dipendere
  direttamente da un repository).
- Il prezzo base per formato viene risolto **prima** di creare ogni `Pizza`, non al momento del calcolo del
  totale: il prezzo base risolto viene passato direttamente al costruttore (`new Pizza(formato, prezzo base)`),
  che lo conserva internamente. Per questo `Pizza.CalculatePrice()` non ha bisogno di alcun parametro —
  restituisce semplicemente `_basePrice + somma dei topping` — ed `Order.Subtotal()` può sommare i prezzi di
  tutte le pizze senza dover conoscere da dove arriva il prezzo base di ciascuna.
- `Cliente->>OrderCompositionService: CalculateTotalsAsync(order)` è la seconda (e ultima) chiamata che il
  cliente fa: passa l'`Order` già composto e riceve indietro un `OrderTotals` con subtotale, spesa di consegna
  e totale finale già calcolati. Anche qui le impostazioni vengono risolte internamente al servizio: il
  cliente non deve mai leggere `PricingSettings` da solo né richiamare `Order.Subtotal()`/`DeliveryFee(...)`/
  `GrandTotal(...)` direttamente.

## Corrispondenza con il codice
Questo flusso rispecchia fedelmente `Program.cs` (la demo console) e i metodi pubblici di
[`OrderCompositionService.cs`](../../PizzaShop.Domain/OrderCompositionService.cs) —
in particolare `CreateOrderAsync` e `CalculateTotalsAsync` — oltre a
[`Order.cs`](../../PizzaShop.Domain/Order.cs), [`Pizza.cs`](../../PizzaShop.Domain/Pizza.cs) e
[`ToppingCatalog.cs`](../../PizzaShop.Domain/ToppingCatalog.cs). È anche lo stesso scenario descritto in
linguaggio naturale nelle feature Gherkin di `PizzaShop.Bdd.Tests/Features/` (gli step definitions chiamano
`OrderCompositionService.CreateOrderAsync`/`CalculateTotalsAsync` esattamente come mostrato qui, invece di
comporre l'ordine o calcolarne i totali da soli).

> **Nota**: questo diagramma rispecchia il codice effettivamente presente: niente più `DiscountPolicy` (la
> logica di sconto è stata rimossa), e `ToppingCatalog`/`PricingSettings`/`IPizzaSizeRepository` sono usati
> nella loro forma realistica (`PizzaShop.Domain` dipende solo dalle interfacce), sempre tramite
> `OrderCompositionService`, che ora è anche l'unico punto che crea e assembla un `Order`. Oggi le uniche
> implementazioni di queste interfacce sono quelle hardcoded in memoria del progetto
> `PizzaShop.Infrastructure.InMemory`, che gioca lo stesso ruolo che avrebbe un vero Data Access Component
> basato su database.
