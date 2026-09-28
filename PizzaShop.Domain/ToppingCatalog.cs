namespace PizzaShop.Domain;

/// <summary>
/// The set of extra ingredients (toppings) offered by the pizzeria, with their price. Reads through
/// <see cref="IToppingRepository"/>, which is the abstraction owned by the Data Access Component.
/// </summary>
public sealed class ToppingCatalog(IToppingRepository repository)
{
    /// <summary>
    /// All the toppings currently available in the catalog.
    /// </summary>
    public Task<IReadOnlyCollection<Topping>> GetAllAsync() => repository.GetAllAsync();

    /// <summary>
    /// Finds a topping by name (case-insensitive).
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the topping is not part of the catalog.</exception>
    public Task<Topping> GetAsync(string name) => repository.GetByNameAsync(name);
}
