namespace PizzaShop.Domain;

/// <summary>
/// Reads the base price (before any topping) for a given pizza size from the pricing catalog.
/// </summary>
public interface IPizzaSizeRepository
{
    /// <summary>
    /// Returns the base price for the given pizza size.
    /// </summary>
    Task<decimal> GetBasePriceAsync(PizzaSize size);
}
