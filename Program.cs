using PizzaShop.Domain;
using System.Globalization;

var culture = CultureInfo.InvariantCulture;

Console.WriteLine("=== PizzaShop - Demo ordine ===");
Console.WriteLine();

var order = new Order("Mario Rossi");

var margherita = new Pizza(PizzaSize.Medium);
margherita.AddTopping(ToppingCatalog.Get("Mozzarella"));
order.AddPizza(margherita);

var capricciosa = new Pizza(PizzaSize.Large);
capricciosa.AddTopping(ToppingCatalog.Get("Prosciutto"));
capricciosa.AddTopping(ToppingCatalog.Get("Funghi"));
capricciosa.AddTopping(ToppingCatalog.Get("Olive"));
order.AddPizza(capricciosa);

foreach (var pizza in order.Pizzas)
{
    var toppings = pizza.Toppings.Count == 0
        ? "nessun ingrediente extra"
        : string.Join(", ", pizza.Toppings.Select(t => t.Name));

    Console.WriteLine($"- Pizza {pizza.Size} ({toppings}): {pizza.CalculatePrice().ToString("F2", culture)} EUR");
}

Console.WriteLine();
Console.WriteLine($"Subtotale:        {order.Subtotal().ToString("F2", culture)} EUR");
Console.WriteLine($"Sconto:           {order.Discount().ToString("F2", culture)} EUR");
Console.WriteLine($"Spesa di consegna: {order.DeliveryFee().ToString("F2", culture)} EUR");
Console.WriteLine($"Totale finale:    {order.GrandTotal().ToString("F2", culture)} EUR");
