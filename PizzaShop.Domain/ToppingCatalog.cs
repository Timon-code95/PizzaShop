namespace PizzaShop.Domain;

/// <summary>
/// The set of extra ingredients (toppings) offered by the pizzeria, with their price.
/// </summary>
public static class ToppingCatalog
{
    private static readonly Dictionary<string, Topping> Toppings = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Mozzarella"] = new Topping("Mozzarella", 1.00m),
        ["Funghi"] = new Topping("Funghi", 1.20m),
        ["Pepperoni"] = new Topping("Pepperoni", 1.50m),
        ["Prosciutto"] = new Topping("Prosciutto", 1.30m),
        ["Olive"] = new Topping("Olive", 0.80m),
    };

    /// <summary>
    /// All the toppings currently available in the catalog.
    /// </summary>
    public static IReadOnlyCollection<Topping> All => Toppings.Values;

    /// <summary>
    /// Finds a topping by name (case-insensitive).
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the topping is not part of the catalog.</exception>
    public static Topping Get(string name)
    {
        if (!Toppings.TryGetValue(name, out var topping))
        {
            throw new KeyNotFoundException($"Topping '{name}' is not available in the catalog.");
        }

        return topping;
    }
}
