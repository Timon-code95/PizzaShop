namespace PizzaShop.Domain;

/// <summary>
/// A single pizza: a size plus a set of extra toppings. Encapsulates the "max toppings" business rule
/// and knows how to compute its own price.
/// </summary>
public sealed class Pizza(PizzaSize size)
{
    /// <summary>
    /// Maximum number of extra toppings allowed on a single pizza.
    /// </summary>
    public const int MaxToppings = 5;

    private readonly List<Topping> _toppings = [];

    public PizzaSize Size { get; } = size;

    public IReadOnlyList<Topping> Toppings => _toppings;

    /// <summary>
    /// Adds an extra topping to the pizza.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the pizza already has the maximum allowed number of toppings.</exception>
    public void AddTopping(Topping topping)
    {
        if (_toppings.Count >= MaxToppings)
        {
            throw new InvalidOperationException(
                $"Cannot add more than {MaxToppings} toppings to a single pizza.");
        }

        _toppings.Add(topping);
    }

    /// <summary>
    /// Total price of this pizza: base price for its size, plus the price of every topping added.
    /// </summary>
    public decimal CalculatePrice() => Size.BasePrice() + _toppings.Sum(t => t.Price);
}
