namespace PizzaShop.Domain;

/// <summary>
/// Available pizza sizes. Each size has its own base price, before any topping is added.
/// </summary>
public enum PizzaSize
{
    Small,
    Medium,
    Large
}

public static class PizzaSizeExtensions
{
    /// <summary>
    /// Returns the base price (without toppings) for the given pizza size.
    /// </summary>
    public static decimal BasePrice(this PizzaSize size) => size switch
    {
        PizzaSize.Small => 5.00m,
        PizzaSize.Medium => 7.50m,
        PizzaSize.Large => 10.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, "Unknown pizza size.")
    };
}
