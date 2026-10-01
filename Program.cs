using PizzaShop.Domain;
using PizzaShop.Infrastructure.InMemory;
using System.Globalization;

var culture = CultureInfo.InvariantCulture;

Console.WriteLine("=== PizzaShop - Demo ordine ===");
Console.WriteLine();

// Composizione delle dipendenze: oggi le implementazioni sono in-memory (dati hardcoded), ma essendo
// PizzaSize.cs/Order.cs scritti contro le sole interfacce, in futuro basterà sostituire
// queste implementazioni con implementazioni vere basate su database, senza toccare il resto del codice.
// OrderCompositionService è il punto unico che parla con i repository (IPizzaSizeRepository,
// IPricingSettingsRepository, IToppingRepository) e l'unico che crea e assembla l'Order: Pizza e Order
// restano entità di dominio pure.
var orderComposer = new OrderCompositionService(
    new InMemoryPizzaSizeRepository(),
    new InMemoryPricingSettingsRepository(),
    new InMemoryToppingRepository());

var order = await orderComposer.CreateOrderAsync(
    "Mario Rossi",
    [
        new PizzaOrderRequest(PizzaSize.Medium, ["Mozzarella"]),
        new PizzaOrderRequest(PizzaSize.Large, ["Prosciutto", "Funghi", "Olive"]),
    ]);

foreach (var pizza in order.Pizzas)
{
    var toppings = pizza.Toppings.Count == 0
        ? "nessun ingrediente extra"
        : string.Join(", ", pizza.Toppings.Select(t => t.Name));

    Console.WriteLine($"- Pizza {pizza.Size} ({toppings}): {pizza.CalculatePrice().ToString("F2", culture)} EUR");
}

var totali = await orderComposer.CalculateTotalsAsync(order);

Console.WriteLine();
Console.WriteLine($"Subtotale:         {totali.Subtotal.ToString("F2", culture)} EUR");
Console.WriteLine($"Spesa di consegna: {totali.DeliveryFee.ToString("F2", culture)} EUR");
Console.WriteLine($"Totale finale:     {totali.GrandTotal.ToString("F2", culture)} EUR");
