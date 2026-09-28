namespace PizzaShop.Domain;

/// <summary>
/// Reads the extra toppings offered by the pizzeria, with their price, from the topping catalog.
/// </summary>
public interface IToppingRepository
{
    /// <summary>
    /// Returns all the toppings currently available in the catalog.
    /// </summary>
    Task<IReadOnlyCollection<Topping>> GetAllAsync();

    /// <summary>
    /// Finds a topping by name (case-insensitive).
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the topping is not part of the catalog.</exception>
    Task<Topping> GetByNameAsync(string name);
}
