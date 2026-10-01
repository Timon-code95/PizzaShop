# Guida di lettura — Class Diagram (Livello 4)

> Questo documento spiega **per filo e per segno** il class diagram in [04-code-level-note.md](04-code-level-note.md),
> simbolo per simbolo. È pensato per chi ha dimestichezza limitata con la notazione UML.

## Indice
- [1. Cos'è un class diagram](#1-cosè-un-class-diagram)
- [2. Il box del componente (namespace)](#2-il-box-del-componente-namespace)
- [3. Anatomia di una classe nel diagramma](#3-anatomia-di-una-classe-nel-diagramma)
- [4. Classe per classe](#4-classe-per-classe)
- [5. Le relazioni tra le classi](#5-le-relazioni-tra-le-classi)
- [6. Diagramma completo commentato](#6-diagramma-completo-commentato)

> **Aggiornamento**: `OrderCompositionService` (sezione [4.9](#49-ordercompositionservice)) è stata introdotta
> per essere il consumer esplicito e visibile di `IPizzaSizeRepository`/`IPricingSettingsRepository`: prima di
> questa modifica il diagramma citava un "livello applicativo interno all'Order Management Component" senza
> che una classe concreta lo rappresentasse — un'imprecisione ora corretta. In un secondo passaggio, lo stesso
> servizio è stato esteso con `CreateOrderAsync` e `CalculateTotalsAsync`: prima creava solo `Pizza`, mentre la
> creazione dell'`Order` e il calcolo dei totali restavano a carico del chiamante — un'altra imprecisione, dato
> che il nome della classe suggeriva già che dovesse occuparsene lei. Ora `OrderCompositionService` è l'unico
> punto che crea e assembla un `Order` completo. In un terzo passaggio è stato rimosso il wrapper
> `ToppingCatalog`, che era un semplice pass-through su `IToppingRepository`: `OrderCompositionService` ora
> dipende direttamente da `IToppingRepository`, esattamente come già faceva per `IPizzaSizeRepository` e
> `IPricingSettingsRepository` — i tre repository sono quindi trattati in modo simmetrico (sezione
> [4.5](#45-itoppingrepository)).

> Le sezioni 4 e 5 mostrano gli snippet delle singole classi/relazioni senza il riquadro `namespace` per
> restare più leggibili in isolamento; il riquadro compare per intero solo nel diagramma reale ([04-code-level-note.md](04-code-level-note.md)) e nel riepilogo finale della sezione 6.

---

## 1. Cos'è un class diagram

Un class diagram UML rappresenta la **struttura statica** del codice: quali classi esistono, quali dati
contengono (attributi), quali operazioni espongono (metodi) e come sono collegate tra loro (relazioni).

Importante: un class diagram **non mostra comportamento** (non mostra *quando* o *in che ordine* i metodi
vengono chiamati — quello è compito del [sequence diagram](05-sequence-diagram.md)). Mostra solo *cosa esiste*
e *come è collegato*.

Nel nostro caso il diagramma descrive le classi del progetto [`PizzaShop.Domain`](../../PizzaShop.Domain/),
cioè l'**Order Management Component** del [Component Diagram](03-component-diagram.md).

---

## 2. Il box del componente (namespace)

Nell'esempio ufficiale del C4 Model (il "Core Banking System Adapter" di Simon Brown), tutte le classi sono
disegnate dentro un unico grande riquadro etichettato con il nome del package Java
(`com.bigbank.ib.component.corebankingsystem`) — a indicare visivamente che quel class diagram descrive **un
singolo componente**, non l'intero sistema. In fondo alla pagina compare anche una didascalia
("Code View: Internet Banking System - Backend - Core Banking System Adapter") che rende esplicito lo stesso
concetto in forma testuale.

Il nostro diagramma fa la stessa cosa, usando il costrutto `namespace` della sintassi `classDiagram` di
Mermaid:

```mermaid
classDiagram
	namespace PizzaShopDomain["PizzaShop.Domain"] {
		class Pizza { }
		class Order { }
	}
```

`namespace NomeNamespace { ... }` disegna un riquadro tratteggiato che racchiude tutte le classi al suo
interno ed è etichettato col nome indicato — esattamente il tipo di box "contenitore" dell'esempio ufficiale.

> **Attenzione alla dot notation**: la prima versione di questo diagramma usava direttamente
> `namespace PizzaShop.Domain { ... }`. Nella sintassi Mermaid, però, il punto in un nome di namespace viene
> interpretato come **separatore gerarchico** (crea automaticamente namespace annidati, come una dot notation
> "Company.Engineering.Backend"): il risultato era **due riquadri annidati**, uno esterno `PizzaShop` e uno
> interno `Domain`, che dava l'impressione errata che `PizzaShop` (l'intero sistema) fosse il nome del
> componente. Per ottenere un **unico riquadro** con il nome tecnico completo si usa invece la sintassi con
> etichetta esplicita `namespace Id["Label"]`: l'`Id` (`PizzaShopDomain`, senza punti) è solo l'identificatore
> interno usato da Mermaid, mentre la `"Label"` (`PizzaShop.Domain`) è il testo effettivamente mostrato nel
> riquadro — e può contenere il punto senza essere interpretata come annidamento.

Il nome mostrato è `PizzaShop.Domain`, che non è scelto a caso: è **il namespace C# reale** in cui
sono dichiarate tutte queste classi (vedi l'intestazione `namespace PizzaShop.Domain;` in cima a ogni file, es.
[`Pizza.cs`](../../PizzaShop.Domain/Pizza.cs)), ed è anche il nome tecnico già associato all'**Order Management
Component** nella tabella di corrispondenza del [Component Diagram](03-component-diagram.md#corrispondenza-con-il-codice).

Il diagramma reale in [04-code-level-note.md](04-code-level-note.md) porta la stessa didascalia in stile
"Code View" dell'esempio ufficiale, ma con un accorgimento in più: non è solo testo Markdown scritto *sotto*
il blocco Mermaid (che sparirebbe se il diagramma venisse esportato o incollato altrove come immagine
isolata), bensì è definita nel **frontmatter** del diagramma stesso (`title: "..."` prima di `classDiagram`),
quindi Mermaid la disegna *dentro* l'immagine renderizzata, sopra il riquadro delle classi — esattamente come
la didascalia dell'esempio ufficiale è parte integrante della sua immagine.

---

## 3. Anatomia di una classe nel diagramma

Ogni classe è disegnata così:

```mermaid
classDiagram
	class Pizza {
		+const int MaxToppings = 5
		+PizzaSize Size
		+IReadOnlyList~Topping~ Toppings
		+AddTopping(Topping topping) void
		+CalculatePrice() decimal
	}
```

Il rettangolo `class NomeClasse { ... }` contiene, riga per riga, i **membri** della classe. Ogni riga si legge
con queste regole:

| Simbolo/sintassi | Significato | Equivalente C# |
|---|---|---|
| `+` a inizio riga | Visibilità **pubblica** (`public`) | `public` |
| `-` a inizio riga (non presente qui) | Visibilità **privata** (`private`) | `private` |
| `#` a inizio riga (non presente qui) | Visibilità **protetta** (`protected`) | `protected` |
| `const int MaxToppings = 5` | Campo costante con valore | `public const int MaxToppings = 5;` |
| `PizzaSize Size` | Attributo/proprietà: `Tipo NomeProprietà` | `public PizzaSize Size { get; }` |
| `IReadOnlyList~Topping~` | Generico: la tilde `~...~` sostituisce `<...>` (Mermaid non può usare `<>` perché sono riservati per altre notazioni UML) | `IReadOnlyList<Topping>` |
| `AddTopping(Topping topping) void` | Metodo: `NomeMetodo(TipoParam nomeParam) TipoRitorno` | `public void AddTopping(Topping topping)` |
| `CalculatePrice() decimal` | Metodo senza parametri che ritorna un valore | `public decimal CalculatePrice()` |

Nessuna riga inizia con `-` o `#` in questo diagramma perché **tutti i membri disegnati sono pubblici**: i
campi privati di implementazione (es. `_toppings`, `_pizzas` nel codice reale) sono stati **volutamente omessi**
perché un class diagram di norma mostra solo la parte rilevante all'esterno (l'API pubblica), non i dettagli
di implementazione interna.

### Gli stereotipi `<<...>>`

Alcune classi hanno una riga tra doppie parentesi angolari, es. `<<record>>` o `<<static>>`. Sono **stereotipi
UML**: etichette che aggiungono un'informazione extra sul "tipo" della classe, che UML puro non prevede ma che
è utile specificare quando si documenta codice C#:

- `<<record>>` → la classe è in realtà un `record` C# (tipo immutabile con uguaglianza per valore).
- `<<static>>` → la classe è `static` (non istanziabile, contiene solo membri statici).
- `<<enumeration>>` → la classe è in realtà un `enum` C#.

---

## 4. Classe per classe

### 4.1 `Pizza`

```mermaid
classDiagram
	class Pizza {
		-decimal _basePrice
		+PizzaSize Size
		+IReadOnlyList~Topping~ Toppings
		+AddTopping(Topping topping, PricingSettings settings) void
		+CalculatePrice() decimal
	}
```

Corrisponde a [`Pizza.cs`](../../PizzaShop.Domain/Pizza.cs). Rappresenta una singola pizza:

- `_basePrice`: prezzo base per il formato scelto, ricevuto dal costruttore (vedi nota sotto) e conservato
  internamente — non è un parametro passato a ogni chiamata, ma uno stato della pizza fissato alla creazione.
- `Size`: il formato della pizza (vedi [`PizzaSize`](#43-pizzasize)).
- `Toppings`: la lista (di sola lettura dall'esterno) degli ingredienti extra aggiunti finora.
- `AddTopping(Topping topping, PricingSettings settings)`: aggiunge un ingrediente extra (lancia un'eccezione
  se si supera `settings.MaxToppingsPerPizza`, ma questo dettaglio *comportamentale* non compare qui — è nel
  codice e nel sequence diagram).
- `CalculatePrice()`: calcola il prezzo totale della pizza (`_basePrice` + ingredienti).

> **Nota**: il limite di ingredienti extra non è più una costante interna a `Pizza` (in passato era
> `MaxToppings = 5`): è configurabile dal proprietario della pizzeria (letto da un database) e arriva
> dall'esterno tramite il parametro `settings` (vedi [`PricingSettings`](#47-pricingsettings)). `Pizza`
> resta comunque un'entità "pura": non sa nulla di database o repository, riceve solo il valore già
> pronto — vedi la sezione 5.3 per il perché di questa scelta.
>
> Lo stesso vale per il prezzo base: `CalculatePrice()` non prende parametri, ma il prezzo base non è più
> calcolato da un metodo hardcoded. Il prezzo base viene risolto da `OrderCompositionService.CreatePizzaAsync`
> (sezione [4.9](#49-ordercompositionservice)), tramite
> [`IPizzaSizeRepository`](#43bis-ipizzasizerepository), e passato al **costruttore** di `Pizza`
> (`new Pizza(size, basePrice)`), che lo salva in `_basePrice`. Per questo `CalculatePrice()` non ha bisogno di
> alcun parametro: il valore è già disponibile internamente, esattamente come avviene per i `Topping` già
> aggiunti — `Pizza` continua a non sapere nulla di database o repository.

### 4.2 `Order`

```mermaid
classDiagram
	class Order {
		+string CustomerName
		+IReadOnlyList~Pizza~ Pizzas
		+AddPizza(Pizza pizza) void
		+Subtotal() decimal
		+HasFreeDelivery(PricingSettings settings) bool
		+DeliveryFee(PricingSettings settings) decimal
		+GrandTotal(PricingSettings settings) decimal
	}
```

Corrisponde a [`Order.cs`](../../PizzaShop.Domain/Order.cs). Rappresenta l'ordine del cliente, con una o più
pizze:

- `CustomerName`: nome del cliente che ha fatto l'ordine.
- `Pizzas`: la lista delle pizze incluse nell'ordine.
- `AddPizza(...)`: aggiunge una pizza all'ordine.
- `Subtotal()`: somma dei prezzi di tutte le pizze.
- `HasFreeDelivery(PricingSettings settings)`: `true` se il subtotale supera `settings.FreeDeliveryThreshold`.
- `DeliveryFee(PricingSettings settings)`: `0` se `HasFreeDelivery(settings)`, altrimenti `settings.StandardDeliveryFee`.
- `GrandTotal(PricingSettings settings)`: totale finale (`Subtotal() + DeliveryFee(settings)`).

> **Nota**: la logica di sconto è stata **rimossa del tutto** dal codice per semplificare il modello — vedi la
> nota nella sezione 4.6 per il perché. Inoltre né `FreeDeliveryThreshold` né `StandardDeliveryFee` sono più
> costanti interne a `Order`, ma arrivano dall'esterno tramite il parametro `settings`
> (vedi [`PricingSettings`](#47-pricingsettings)), perché entrambi sono pensati come configurabili dal
> proprietario della pizzeria.

### 4.3 `PizzaSize`

```mermaid
classDiagram
	class PizzaSize {
		<<enumeration>>
		Small
		Medium
		Large
	}
```

Corrisponde all'enum in [`PizzaSize.cs`](../../PizzaShop.Domain/PizzaSize.cs). È un semplice elenco di valori
possibili (`Small`, `Medium`, `Large`) — non ha visibilità `+` perché i valori di un `enum` sono per natura
pubblici e non sono né campi né metodi in senso classico.

> **Nota**: il prezzo base per formato non è più calcolato da un metodo con valori hardcoded (`Small` = 5.00,
> `Medium` = 7.50, `Large` = 10.00 restano gli stessi valori, ma ora vengono letti tramite un repository). Il
> diagramma lo tratta come un dato di menu configurabile dal proprietario della pizzeria, esattamente come i
> topping — vedi la sezione seguente ([`IPizzaSizeRepository`](#43bis-ipizzasizerepository)).

### 4.3bis `IPizzaSizeRepository`

```mermaid
classDiagram
	class IPizzaSizeRepository {
		<<interface>>
		+GetBasePriceAsync(PizzaSize size) Task~decimal~
	}
```

È il confine (contratto) verso il **Data Access Component**, lo stesso ruolo che `IToppingRepository` ha per i
topping: dato un `PizzaSize`, restituisce il prezzo base configurato per quel formato, letto da un database in
modo asincrono (`Task<decimal>`). Come per gli altri repository, l'implementazione concreta non compare in
questo diagramma perché appartiene a un altro componente.

> **Perché non è `Pizza` a dipendere direttamente da `IPizzaSizeRepository`?** Per lo stesso motivo per cui
> `Pizza`/`Order` non dipendono direttamente da `IPricingSettingsRepository` (vedi la nota nella sezione
> [4.8](#48-ipricingsettingsrepository)): `Pizza` è un'entità di dominio e deve restare facile da istanziare e
> testare, senza sapere da dove arrivano i dati. Il prezzo base viene quindi risolto **prima** di creare la
> pizza da `OrderCompositionService.CreatePizzaAsync(size)` (sezione [4.9](#49-ordercompositionservice)),
> che chiama `GetBasePriceAsync(size)` e passa il risultato al **costruttore** di
> `Pizza` (`new Pizza(size, basePrice)`), che lo conserva in `_basePrice`. `CalculatePrice()` lo userà poi
> senza bisogno di riceverlo di nuovo come parametro a ogni chiamata.

### 4.4 `Topping`

```mermaid
classDiagram
	class Topping {
		<<record>>
		+string Name
		+decimal Price
	}
```

Corrisponde a [`Topping.cs`](../../PizzaShop.Domain/Topping.cs): un ingrediente extra, con nome e prezzo. È uno
stereotipo `<<record>>` perché nel codice è dichiarato come `public sealed record Topping(string Name, decimal Price)`
— un record C#, tipicamente immutabile e con uguaglianza per valore (due `Topping` con stesso nome e prezzo sono
considerati uguali).

### 4.5 `IToppingRepository`

```mermaid
classDiagram
	class IToppingRepository {
		<<interface>>
		+GetAllAsync() Task~IReadOnlyCollection~Topping~~
		+GetByNameAsync(string name) Task~Topping~
	}
```

È il confine (contratto) tra l'**Order Management Component** e il **Data Access Component**, esattamente
come `IPizzaSizeRepository` (sezione [4.3bis](#43bis-ipizzasizerepository)) e `IPricingSettingsRepository`
(sezione [4.8](#48-ipricingsettingsrepository)): incapsula la lettura di topping e prezzi da un database in
modo asincrono (`Task<...>`), senza esporre dettagli di SQL, stringhe di connessione o ORM usati per
implementarla. L'implementazione concreta (es. `SqlToppingRepository`) non compare in questo diagramma perché
appartiene a un altro componente — esattamente come, nell'esempio ufficiale del C4 Model,
`CoreBankingSystemConnection` incapsula i dettagli di rete senza esporli a chi la usa. Lo stereotipo
`<<interface>>` segnala che si tratta di un contratto (`public interface IToppingRepository` in C#), non di
una classe concreta istanziabile.

- `GetAllAsync()`: tutti gli ingredienti disponibili, letti dal database.
- `GetByNameAsync(string name)`: cerca un ingrediente per nome, letto dal database (lancia
  `KeyNotFoundException` se non esiste).

> **Nota**: in una versione precedente di questo diagramma esisteva anche un wrapper `ToppingCatalog`, una
> classe applicativa con un campo privato `_repository` che si limitava a inoltrare le chiamate a
> `IToppingRepository`. Era però un semplice pass-through senza logica propria, ed era l'unico dei tre
> repository ad avere un livello di indirezione in più: è stato quindi rimosso, e `OrderCompositionService`
> (sezione [4.9](#49-ordercompositionservice)) ora dipende direttamente da `IToppingRepository`, come già
> faceva per `IPizzaSizeRepository` e `IPricingSettingsRepository`.

### 4.6 Perché non c'è più `DiscountPolicy`

In una versione precedente del codice esisteva una classe `DiscountPolicy.cs`, con una soglia
(`DiscountThreshold = 30.00`) e una percentuale (`DiscountRate = 0.10`) fisse, usata da `Order` per
calcolare `Discount()` e `TotalAfterDiscount()`. La logica di sconto è stata **rimossa del
tutto**, per semplificare il modello: niente classe `DiscountPolicy`, niente `Discount()`/`TotalAfterDiscount()`
su `Order` (vedi [sezione 4.2](#42-order)). Il totale finale si calcola quindi direttamente da `Subtotal()` più
l'eventuale costo di consegna.

### 4.7 `PricingSettings`

```mermaid
classDiagram
	class PricingSettings {
		<<record>>
		+int MaxToppingsPerPizza
		+decimal FreeDeliveryThreshold
		+decimal StandardDeliveryFee
	}
```

È un semplice contenitore dati (per questo è uno stereotipo `<<record>>`, come `Topping`): raggruppa le regole di
business che il proprietario della pizzeria può modificare — il numero massimo di topping per pizza, la soglia
di consegna gratuita e il costo di consegna standard. Non contiene logica, solo valori: chi la usa (`Pizza`,
`Order`) la riceve già pronta come parametro, senza sapere da dove arriva.

> **Nota**: né `FreeDeliveryThreshold` né `StandardDeliveryFee` sono più costanti dichiarate dentro `Order`
> (in passato valevano `25.00` e `3.50`). Ora sono **configurabili dal proprietario della pizzeria** (lette da
> un database), esattamente come il limite di topping di `Pizza` — vedi la sezione 5.3 per il perché `Order`
> resta comunque un'entità "pura" invece di dipendere direttamente da un repository.

### 4.8 `IPricingSettingsRepository`

```mermaid
classDiagram
	class IPricingSettingsRepository {
		<<interface>>
		+GetAsync() Task~PricingSettings~
	}
```

È il confine (contratto) verso il **Data Access Component**, lo stesso ruolo che `IToppingRepository` ha per i
topping: incapsula la lettura delle impostazioni da un database, restituendole come `PricingSettings` tramite
un metodo asincrono (`Task<...>`), coerentemente col fatto che leggere da un DB è un'operazione di I/O.
L'implementazione concreta non compare in questo diagramma, perché appartiene a un altro componente.

> **Perché non è `Pizza`/`Order` a dipendere direttamente da `IPricingSettingsRepository`?** `Pizza` e `Order`
> rappresentano entità di dominio: farle dipendere da un repository le renderebbe più difficili da istanziare
> e testare, e mescolerebbe "cosa sono" con "da dove arrivano i dati". Per questo qui si è scelto un livello
> di indirezione in più: `OrderCompositionService` (sezione [4.9](#49-ordercompositionservice)), un servizio
> applicativo interno all'**Order Management Component** stesso e ora esplicitamente presente nel diagramma,
> chiama `IPricingSettingsRepository.GetAsync()` una volta e passa il risultato (`PricingSettings`) a valle,
> come semplice parametro. **Non** è l'Order API a farlo direttamente: l'Order API inoltra la richiesta
> all'Order Management Component (vedi [Component Diagram](03-component-diagram.md)) e resta un livello
> sottile, senza conoscere i dettagli di come vengono risolti prezzi e impostazioni.

### 4.9 `OrderCompositionService`

```mermaid
classDiagram
	class PizzaOrderRequest {
		<<record>>
		+PizzaSize Size
		+IReadOnlyCollection~string~ ToppingNames
	}

	class OrderTotals {
		<<record>>
		+decimal Subtotal
		+decimal DeliveryFee
		+decimal GrandTotal
	}

	class OrderWithTotals {
		<<record>>
		+Order Order
		+OrderTotals Totals
	}

	class OrderCompositionService {
		-IPizzaSizeRepository _pizzaSizeRepository
		-IPricingSettingsRepository _pricingSettingsRepository
		-IToppingRepository _toppingRepository
		+CreatePizzaAsync(PizzaSize size) Task~Pizza~
		+GetPricingSettingsAsync() Task~PricingSettings~
		+GetToppingAsync(string name) Task~Topping~
		+CreateOrderAsync(string customerName, IEnumerable~PizzaOrderRequest~ pizzas) Task~Order~
		+CalculateTotalsAsync(Order order) Task~OrderTotals~
		+CreateOrderWithTotalsAsync(string customerName, IEnumerable~PizzaOrderRequest~ pizzas) Task~OrderWithTotals~
	}
```

Corrisponde a [`OrderCompositionService.cs`](../../PizzaShop.Domain/OrderCompositionService.cs): è il servizio
applicativo che **orchestra la composizione dell'ordine**, ed è il **consumer esplicito** di
`IPizzaSizeRepository`, `IPricingSettingsRepository` e `IToppingRepository` che nelle versioni precedenti di
questo diagramma era solo citato a parole ("un livello applicativo non mostrato") senza comparire come classe
reale.

- `_pizzaSizeRepository`, `_pricingSettingsRepository`, `_toppingRepository`: dipendenze private iniettate nel
  costruttore, tenute per tutta la vita del servizio, gestite in modo simmetrico.
- `CreatePizzaAsync(PizzaSize size)`: risolve il prezzo base tramite `IPizzaSizeRepository.GetBasePriceAsync(size)`
  e costruisce un `Pizza` già pronto (`new Pizza(size, basePrice)`).
- `GetPricingSettingsAsync()`: delega a `IPricingSettingsRepository.GetAsync()`.
- `GetToppingAsync(string name)`: delega a `IToppingRepository.GetByNameAsync(name)`.
- `CreateOrderAsync(string customerName, IEnumerable<PizzaOrderRequest> pizzas)`: crea un `Order` vuoto per
  `customerName`, poi per ogni `PizzaOrderRequest` (formato + nomi dei topping desiderati) crea la pizza tramite
  `CreatePizzaAsync`, applica ogni topping risolto tramite `GetToppingAsync` e `pizza.AddTopping(...)`, e infine fa
  `order.AddPizza(pizza)`. Restituisce l'`Order` completo. **È l'unico punto in cui viene creato un `Order`**: il
  chiamante non fa mai `new Order(...)` né `order.AddPizza(...)` direttamente.
- `CalculateTotalsAsync(Order order)`: risolve le impostazioni correnti tramite `GetPricingSettingsAsync()` e le
  usa per calcolare `order.Subtotal()`, `order.DeliveryFee(settings)` e `order.GrandTotal(settings)`, restituendo
  il tutto racchiuso in un `OrderTotals`. Il chiamante non richiama mai questi tre metodi di `Order` da solo.
- `CreateOrderWithTotalsAsync(string customerName, IEnumerable<PizzaOrderRequest> pizzas)`: metodo di
  convenienza a **chiamata singola** che risolve `GetPricingSettingsAsync()` **una sola volta** e la passa ai due
  metodi privati condivisi `ComposeOrderAsync` e `ComputeTotals` (gli stessi usati internamente da
  `CreateOrderAsync` e `CalculateTotalsAsync`), restituendo entrambi i risultati in un `OrderWithTotals`. Questo
  evita una seconda interrogazione ridondante al repository delle impostazioni di prezzo quando composizione e
  totale sono richiesti nella stessa chiamata. Serve per i casi in cui il chiamante vuole solo comporre l'ordine
  e conoscerne subito il prezzo finale, senza dover gestire due round trip separati (es. un vero controller API
  che riceve una singola richiesta HTTP dal cliente). `CreateOrderAsync` e `CalculateTotalsAsync` restano
  comunque disponibili separatamente (ciascuno risolve le proprie impostazioni al bisogno), perché utili quando
  composizione e calcolo del totale vanno verificati come passi distinti (è il caso dei test BDD, dove un passo
  Given compone l'ordine e un passo When successivo ne calcola il totale).
- `PizzaOrderRequest`, `OrderTotals` e `OrderWithTotals` sono tre `record` di supporto: il primo descrive "una
  pizza da comporre" (input di `CreateOrderAsync`), il secondo il risultato del calcolo dei totali (output di
  `CalculateTotalsAsync`), il terzo combina un `Order` già composto con il suo `OrderTotals` (output di
  `CreateOrderWithTotalsAsync`). Nessuno dei tre contiene logica: sono semplici contenitori di dati, come
  `PricingSettings` o `Topping`.

È questa classe — non `Pizza`, non `Order` — a essere usata direttamente da `Program.cs` (il composition root
della console demo) e dagli step definitions BDD (`CommonPizzaSteps`, `OrderSteps`): loro chiamano
`OrderCompositionService`, mai i repository direttamente, e non creano mai `Order` o `Pizza` con `new` al di
fuori dei metodi di questo servizio. In un'eventuale Order API reale, sarebbe `OrderCompositionService` (o un
servizio equivalente all'interno dell'Order Management Component) a essere invocato dall'API stessa: il
controller si limiterebbe a tradurre la richiesta HTTP in una chiamata a `CreateOrderAsync`/`CalculateTotalsAsync`,
senza conoscere `Pizza`, `Order` o i repository.

---

## 5. Le relazioni tra le classi

Dopo le classi, il diagramma dichiara come sono collegate tra loro:

```mermaid
classDiagram
	Order "1" o-- "many" Pizza : Pizzas
	Pizza "1" o-- "0..many" Topping : Toppings
	Pizza --> PizzaSize : Size
	Pizza ..> PricingSettings : usa
	Order ..> PricingSettings : usa
	IPricingSettingsRepository ..> PricingSettings : restituisce
	IPizzaSizeRepository ..> PizzaSize : usa
	OrderCompositionService --> IPizzaSizeRepository : usa
	OrderCompositionService --> IPricingSettingsRepository : usa
	OrderCompositionService --> IToppingRepository : usa
	OrderCompositionService ..> Pizza : crea
	OrderCompositionService ..> Order : crea
	OrderCompositionService ..> PizzaOrderRequest : riceve
	OrderCompositionService ..> OrderTotals : restituisce
```

Ci sono **tre tipi diversi** di relazione, ognuna con un significato UML preciso.

### 5.1 Aggregazione (`o--`) — "ha un/ha molti" con cardinalità

```
Order "1" o-- "many" Pizza : Pizzas
Pizza "1" o-- "0..many" Topping : Toppings
```

Il simbolo `o--` disegna una linea continua con un **cerchio vuoto** (○) dal lato della classe "contenitore"
(qui `Order` e `Pizza`). Significa: "questa classe contiene una collezione dell'altra classe come parte del suo
stato".

- `Order "1" o-- "many" Pizza`: **1** `Order` contiene **molte** `Pizza` (nell'attributo `Pizzas`).
- `Pizza "1" o-- "0..many" Topping`: **1** `Pizza` contiene **da 0 a molti** `Topping` (nell'attributo `Toppings`
  — una pizza può anche non avere ingredienti extra).

Le etichette `"1"`, `"many"`, `"0..many"` sono le **cardinalità/moltiplicità**: si leggono sempre dal lato della
classe più vicina all'etichetta. L'etichetta finale (`: Pizzas`, `: Toppings`) indica il **nome della proprietà**
nel codice che realizza quella relazione.

> **Perché aggregazione (○) e non composizione (●)?** In UML la composizione (cerchio pieno) indicherebbe che
> l'oggetto "contenuto" non ha senso di esistere senza il "contenitore" (se elimini l'`Order`, le sue `Pizza`
> smettono di esistere logicamente). Qui è stata scelta la semantica più leggera (aggregazione) perché a questo
> livello di dettaglio didattico non è un punto critico da forzare, anche se concettualmente si potrebbe
> discutere che sia più corretta la composizione.

### 5.2 Associazione diretta (`-->`) — riferimento a un tipo

```
Pizza --> PizzaSize : Size
OrderCompositionService --> IPizzaSizeRepository : usa
OrderCompositionService --> IPricingSettingsRepository : usa
OrderCompositionService --> IToppingRepository : usa
```

> `IPricingSettingsRepository ..> PricingSettings : restituisce` usa invece una dipendenza tratteggiata
> (sezione 5.3), non un'associazione: l'interfaccia non "possiede" un `PricingSettings`, lo produce e lo
> restituisce come valore di ritorno del metodo `GetAsync()`.

La freccia continua **piena** (`-->`) indica che la classe di partenza ha un riferimento diretto a quella di
arrivo, tenuto come campo/proprietà. È una relazione più "debole" dell'aggregazione: non è una collezione, è
un singolo valore che la classe usa come proprio attributo tipizzato.

- `Pizza --> PizzaSize : Size`: `Pizza` ha un riferimento diretto a `PizzaSize` tramite la proprietà `Size`.
- `OrderCompositionService --> IPizzaSizeRepository/IPricingSettingsRepository/IToppingRepository : usa`:
  `OrderCompositionService` tiene tutte e tre come campi privati iniettati nel costruttore (sezione
  [4.9](#49-ordercompositionservice)) — dipendenze stabili per tutta la vita dell'oggetto, non
  create/scartate a ogni chiamata come farebbe una dipendenza (`..>`).

### 5.3 Dipendenza (`..>`) — "usa, senza possedere"

```
Pizza ..> PricingSettings : usa
Order ..> PricingSettings : usa
IPizzaSizeRepository ..> PizzaSize : usa
OrderCompositionService ..> Pizza : crea
OrderCompositionService ..> Order : crea
OrderCompositionService ..> PizzaOrderRequest : riceve
OrderCompositionService ..> OrderTotals : restituisce
```

La freccia **tratteggiata** (`..>`) indica una **dipendenza**: `Pizza` e `Order` *usano* dati di
`PricingSettings`, ma non li tengono come campo/proprietà — è un uso "di passaggio" (es. il parametro
`settings` viene letto quando serve, non è salvato da nessuna parte dentro `Pizza`).

`Pizza ..> PricingSettings` e `Order ..> PricingSettings` seguono una logica analoga: entrambe ricevono
un `PricingSettings` come **parametro di metodo** (`AddTopping(..., settings)`, `HasFreeDelivery(settings)`,
`DeliveryFee(settings)`, `GrandTotal(settings)`), lo leggono e lo scartano — non lo tengono come campo. È
esattamente questa scelta (dipendenza "di passaggio" invece di un'associazione stabile come
`OrderCompositionService --> IToppingRepository`) che permette a `Pizza` e `Order` di restare entità di
dominio pure, senza mai dipendere direttamente da un repository.

`IPizzaSizeRepository ..> PizzaSize : usa` segue lo stesso principio delle relazioni "di ritorno" viste sopra
per `IPricingSettingsRepository`: l'interfaccia usa `PizzaSize` come chiave di ricerca del proprio metodo
(`GetBasePriceAsync(PizzaSize size)`), senza possederlo come campo.

`OrderCompositionService ..> Pizza : crea` indica invece una dipendenza di **creazione**: `CreatePizzaAsync`
costruisce un nuovo `Pizza` (`new Pizza(size, basePrice)`) e lo restituisce, senza tenerne una copia come
campo — esattamente come una fabbrica non trattiene gli oggetti che produce.

`OrderCompositionService ..> Order : crea` è la stessa relazione di creazione, ma per `Order`:
`CreateOrderAsync` costruisce un nuovo `Order` (`new Order(customerName)`), lo popola con le pizze e lo
restituisce, senza tenerne un riferimento. È la relazione che rende esplicito il fatto, prima assente dal
diagramma, che `OrderCompositionService` non compone solo le singole pizze ma anche l'ordine nel suo insieme.

`OrderCompositionService ..> PizzaOrderRequest : riceve` e `OrderCompositionService ..> OrderTotals : restituisce`
sono dipendenze "di passaggio dati": `CreateOrderAsync` riceve una collezione di `PizzaOrderRequest` come
parametro (senza tenerli come campo), mentre `CalculateTotalsAsync` produce e restituisce un `OrderTotals` —
lo stesso pattern già visto per `IPricingSettingsRepository ..> PricingSettings : restituisce`.

### 5.4 Riepilogo differenze linea/freccia

| Notazione Mermaid | Nome UML | Linea | Significato |
|---|---|---|---|
| `o--` | Aggregazione | Continua, cerchio vuoto | "Ha una collezione di" (contenitore/contenuto) |
| `-->` | Associazione | Continua, freccia piena | "Ha un riferimento a" (singolo valore tipizzato) |
| `..>` | Dipendenza | Tratteggiata, freccia aperta | "Usa temporaneamente", senza possedere |

---

## 6. Diagramma completo commentato

```mermaid
---
title: "Code View: PizzaShop — Backend — Order Management Component"
---
classDiagram
	namespace PizzaShopDomain["PizzaShop.Domain"] {
		class Pizza {
			-decimal _basePrice
			+PizzaSize Size
			+IReadOnlyList~Topping~ Toppings
			+AddTopping(Topping topping, PricingSettings settings) void
			+CalculatePrice() decimal
		}

		class Order {
			+string CustomerName
			+IReadOnlyList~Pizza~ Pizzas
			+AddPizza(Pizza pizza) void
			+Subtotal() decimal
			+HasFreeDelivery(PricingSettings settings) bool
			+DeliveryFee(PricingSettings settings) decimal
			+GrandTotal(PricingSettings settings) decimal
		}

		class PizzaSize {
			<<enumeration>>
			Small
			Medium
			Large
		}

		class IPizzaSizeRepository {
			<<interface>>
			+GetBasePriceAsync(PizzaSize size) Task~decimal~
		}

		class Topping {
			<<record>>
			+string Name
			+decimal Price
		}

		class IToppingRepository {
			<<interface>>
			+GetAllAsync() Task~IReadOnlyCollection~Topping~~
			+GetByNameAsync(string name) Task~Topping~
		}

		class PricingSettings {
			<<record>>
			+int MaxToppingsPerPizza
			+decimal FreeDeliveryThreshold
			+decimal StandardDeliveryFee
		}

		class IPricingSettingsRepository {
			<<interface>>
			+GetAsync() Task~PricingSettings~
		}

		class PizzaOrderRequest {
			<<record>>
			+PizzaSize Size
			+IReadOnlyCollection~string~ ToppingNames
		}

		class OrderTotals {
			<<record>>
			+decimal Subtotal
			+decimal DeliveryFee
			+decimal GrandTotal
		}

		class OrderCompositionService {
			-IPizzaSizeRepository _pizzaSizeRepository
			-IPricingSettingsRepository _pricingSettingsRepository
			-IToppingRepository _toppingRepository
			+CreatePizzaAsync(PizzaSize size) Task~Pizza~
			+GetPricingSettingsAsync() Task~PricingSettings~
			+GetToppingAsync(string name) Task~Topping~
			+CreateOrderAsync(string customerName, IEnumerable~PizzaOrderRequest~ pizzas) Task~Order~
			+CalculateTotalsAsync(Order order) Task~OrderTotals~
		}
	}

	Order "1" o-- "many" Pizza : Pizzas
	Pizza "1" o-- "0..many" Topping : Toppings
	Pizza --> PizzaSize : Size
	Pizza ..> PricingSettings : usa
	Order ..> PricingSettings : usa
	IPricingSettingsRepository ..> PricingSettings : restituisce
	IPizzaSizeRepository ..> PizzaSize : usa
	OrderCompositionService --> IPizzaSizeRepository : usa
	OrderCompositionService --> IPricingSettingsRepository : usa
	OrderCompositionService --> IToppingRepository : usa
	OrderCompositionService ..> Pizza : crea
	OrderCompositionService ..> Order : crea
	OrderCompositionService ..> PizzaOrderRequest : riceve
	OrderCompositionService ..> OrderTotals : restituisce
```

*Come già anticipato nella [sezione 2](#2-il-box-del-componente-namespace), il riquadro `PizzaShop.Domain` e il
titolo `Code View: PizzaShop — Backend — Order Management Component` rendono esplicito, direttamente
nell'immagine renderizzata, che questo è il code-view di un singolo componente — l'Order Management Component
— e non dell'intero sistema PizzaShop.*

**Lettura d'insieme, dall'alto verso il basso:**

1. Un `Order` **aggrega** più `Pizza` (relazione 1→molti).
2. Ogni `Pizza` **aggrega** da 0 a molti `Topping` (relazione 1→0..molti) e **ha un** `PizzaSize`.
3. `OrderCompositionService` **ha un riferimento a** `IPizzaSizeRepository`, `IPricingSettingsRepository` e
   `IToppingRepository` (tutte e tre iniettate nel costruttore, in modo simmetrico), e **crea** ogni `Pizza`
   (`CreatePizzaAsync`) oltre all'`Order` stesso (`CreateOrderAsync`), a partire da una lista di
   `PizzaOrderRequest`. Calcola anche i totali finali (`CalculateTotalsAsync`), restituendo un `OrderTotals`.
   È lei il consumer esplicito di questi repository e l'unico punto che assembla un ordine completo — né
   `Pizza` né `Order` li chiamano mai direttamente, e nessuno all'esterno crea un `Order` con `new` o lo
   popola direttamente.
4. `IToppingRepository` legge topping e prezzi da un database, senza esporne i dettagli implementativi
   (delegati al Data Access Component). Allo stesso modo, `IPizzaSizeRepository` **usa** `PizzaSize` per
   recuperare da un database il prezzo base associato a quel formato.
5. `Pizza` e `Order` **dipendono da** `PricingSettings` (limite topping e soglia di consegna gratuita
   configurabili dal proprietario), ricevuto come parametro invece che tramite un repository iniettato:
   restano così entità di dominio pure, mentre è `OrderCompositionService` (tramite
   `IPricingSettingsRepository`) a occuparsi di produrre quel valore leggendolo da un database. Il prezzo
   base per formato segue una logica simile ma non identica: risolto da `OrderCompositionService` tramite
   `IPizzaSizeRepository` **prima** di creare la pizza e passato al **costruttore**
   (`new Pizza(size, basePrice)`), che lo conserva in `_basePrice` — per questo `CalculatePrice()` non ha
   bisogno di riceverlo a ogni chiamata come fanno invece `AddTopping(..., settings)` o `GrandTotal(settings)`
   con `PricingSettings`.
6. `Topping`, `PricingSettings`, `PizzaOrderRequest` e `OrderTotals` sono record immutabili,
   `IToppingRepository`/`IPricingSettingsRepository`/`IPizzaSizeRepository` sono interfacce, `PizzaSize` è un
   enum, `OrderCompositionService` è una classe applicativa concreta — diverse "nature" di classe, segnalate
   dagli stereotipi.
7. Non compare più alcuna logica di sconto: 
   dall'eventuale costo di consegna (`DeliveryFee(settings)`), senza passare da una classe `DiscountPolicy`.

Per vedere **quando** questi metodi vengono effettivamente chiamati, e in che ordine, consulta il
[sequence diagram](05-sequence-diagram.md), che descrive il flusso del caso d'uso "composizione di un ordine e
calcolo del totale" usando esattamente queste classi.
