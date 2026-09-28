namespace PizzaShop.Domain;

/// <summary>
/// A single pizza: a size plus a set of extra toppings. Encapsulates the "max toppings" business rule
/// and knows how to compute its own price.
/// </summary>
/// <remarks>
/// The base price for <paramref name="size"/> is resolved by the caller (via <see cref="IPizzaSizeRepository"/>)
/// before the pizza is created, and passed in already resolved: <see cref="Pizza"/> is a pure domain entity
/// and does not depend on any repository itself.
/// </remarks>
public sealed class Pizza(PizzaSize size, decimal basePrice)
{
    private readonly decimal _basePrice = basePrice;
    private readonly List<Topping> _toppings = [];

    public PizzaSize Size { get; } = size;

    public IReadOnlyList<Topping> Toppings => _toppings;

    /// <summary>
    /// Adds an extra topping to the pizza.
    /// </summary>
    /// <param name="topping">The topping to add.</param>
    /// <param name="settings">The current pricing settings, used to enforce the maximum toppings rule.</param>
    /// <exception cref="InvalidOperationException">Thrown when the pizza already has the maximum allowed number of toppings.</exception>
    public void AddTopping(Topping topping, PricingSettings settings)
    {
        if (_toppings.Count >= settings.MaxToppingsPerPizza)
        {
            throw new InvalidOperationException(
                $"Cannot add more than {settings.MaxToppingsPerPizza} toppings to a single pizza.");
        }

        _toppings.Add(topping);
    }

    /// <summary>
    /// Total price of this pizza: base price for its size, plus the price of every topping added.
    /// </summary>
    public decimal CalculatePrice() => _basePrice + _toppings.Sum(t => t.Price);
}
