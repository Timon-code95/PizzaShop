# Livello 4 — Code (nota)

Il livello **Code** è il più dettagliato del C4 Model: tipicamente un class diagram UML con classi, attributi e
metodi. Nella pratica **è il livello meno usato**, per un motivo semplice: cambia ad ogni refactoring, quindi
mantenerlo a mano manualmente è costoso e il diagramma "invecchia" (diventa disallineato dal codice reale) molto
in fretta. La raccomandazione comune (anche di Simon Brown, l'autore del C4 Model) è **generarlo al bisogno**
direttamente dall'IDE o da strumenti di analisi statica, piuttosto che tenerlo come documento "vivo" nel repository.

A puro titolo illustrativo, ecco come apparirebbe per le classi principali dell'**Order Management Component**
(vedi [Component Diagram](03-component-diagram.md)), il cui nome tecnico nel codice è il namespace/libreria
`PizzaShop.Domain` — è per questo che il riquadro nel diagramma è etichettato `PizzaShop.Domain`: rappresenta
lo stesso componente, solo visto "dal lato codice" invece che "dal lato architettura". Il diagramma è scritto
anch'esso come diagramma-as-code, stavolta con la sintassi `classDiagram` di Mermaid, pensata proprio per le
classi:

```mermaid
---
title: "Code View: PizzaShop — Backend — Order Management Component"
---
classDiagram
	namespace PizzaShopDomain["PizzaShop.Domain"] {
		class Pizza {
			+const int MaxToppings = 5
			+PizzaSize Size
			+IReadOnlyList~Topping~ Toppings
			+AddTopping(Topping topping) void
			+CalculatePrice() decimal
		}

		class Order {
			+const decimal FreeDeliveryThreshold = 25.00
			+const decimal StandardDeliveryFee = 3.50
			+string CustomerName
			+IReadOnlyList~Pizza~ Pizzas
			+AddPizza(Pizza pizza) void
			+Subtotal() decimal
			+Discount() decimal
			+TotalAfterDiscount() decimal
			+HasFreeDelivery() bool
			+DeliveryFee() decimal
			+GrandTotal() decimal
		}

		class PizzaSize {
			<<enumeration>>
			Small
			Medium
			Large
		}

		class Topping {
			<<record>>
			+string Name
			+decimal Price
		}

		class ToppingCatalog {
			<<static>>
			+All IReadOnlyCollection~Topping~
			+Get(string name) Topping
		}

		class DiscountPolicy {
			<<static>>
			+const decimal DiscountThreshold = 30.00
			+const decimal DiscountRate = 0.10
			+CalculateDiscount(decimal subtotal) decimal
		}
	}

	Order "1" o-- "many" Pizza : Pizzas
	Pizza "1" o-- "0..many" Topping : Toppings
	Pizza --> PizzaSize : Size
	Pizza ..> ToppingCatalog : usa
	Order ..> DiscountPolicy : usa
```

> Il titolo `Code View: PizzaShop — Backend — Order Management Component` è ora parte del diagramma stesso
> (definito nel frontmatter `title:` del blocco Mermaid), non solo di questo testo: resta quindi visibile
> anche se il diagramma viene esportato o incollato altrove come immagine isolata (vedi
> [Component Diagram](03-component-diagram.md) per il contesto architetturale completo).

## Da tenere a mente
Questo file è pensato come **esempio didattico**, non come documentazione da tenere sincronizzata manualmente ad
ogni modifica del codice. Se in futuro serve un class diagram aggiornato, la via consigliata è generarlo
automaticamente (es. dagli strumenti di class diagram integrati nell'IDE) invece di aggiornare a mano
questo file.
