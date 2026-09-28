using PizzaShop.Domain;
using PizzaShop.Infrastructure.InMemory;
using System.Globalization;

var culture = CultureInfo.InvariantCulture;

Console.WriteLine("=== PizzaShop - Demo ordine ===");
Console.WriteLine();

// Composizione delle dipendenze: oggi le implementazioni sono in-memory (dati hardcoded), ma essendo
// PizzaSize.cs/ToppingCatalog.cs/Order.cs scritti contro le sole interfacce, in futuro basterà sostituire
// queste tre righe con implementazioni vere basate su database, senza toccare il resto del codice.
IPizzaSizeRepository pizzaSizeRepository = new InMemoryPizzaSizeRepository();
var toppingCatalog = new ToppingCatalog(new InMemoryToppingRepository());
IPricingSettingsRepository pricingSettingsRepository = new InMemoryPricingSettingsRepository();

var settings = await pricingSettingsRepository.GetAsync();

var order = new Order("Mario Rossi");

var margheritaBasePrice = await pizzaSizeRepository.GetBasePriceAsync(PizzaSize.Medium);
var margherita = new Pizza(PizzaSize.Medium, margheritaBasePrice);
margherita.AddTopping(await toppingCatalog.GetAsync("Mozzarella"), settings);
order.AddPizza(margherita);

var capricciosaBasePrice = await pizzaSizeRepository.GetBasePriceAsync(PizzaSize.Large);
var capricciosa = new Pizza(PizzaSize.Large, capricciosaBasePrice);
capricciosa.AddTopping(await toppingCatalog.GetAsync("Prosciutto"), settings);
capricciosa.AddTopping(await toppingCatalog.GetAsync("Funghi"), settings);
capricciosa.AddTopping(await toppingCatalog.GetAsync("Olive"), settings);
order.AddPizza(capricciosa);

foreach (var pizza in order.Pizzas)
{
    var toppings = pizza.Toppings.Count == 0
        ? "nessun ingrediente extra"
        : string.Join(", ", pizza.Toppings.Select(t => t.Name));

    Console.WriteLine($"- Pizza {pizza.Size} ({toppings}): {pizza.CalculatePrice().ToString("F2", culture)} EUR");
}

Console.WriteLine();
Console.WriteLine($"Subtotale:         {order.Subtotal().ToString("F2", culture)} EUR");
Console.WriteLine($"Spesa di consegna: {order.DeliveryFee(settings).ToString("F2", culture)} EUR");
Console.WriteLine($"Totale finale:     {order.GrandTotal(settings).ToString("F2", culture)} EUR");
