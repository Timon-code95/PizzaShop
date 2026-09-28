using PizzaShop.Domain;

namespace PizzaShop.Infrastructure.InMemory;

/// <summary>
/// Fake implementation of <see cref="IToppingRepository"/> that returns a hardcoded in-memory catalog,
/// instead of reading it from a real database. It exists purely to let the domain and its consumers
/// (console app, BDD tests) work against the repository abstraction today, so that a real database-backed
/// implementation can later replace this one without touching any other project.
/// </summary>
public sealed class InMemoryToppingRepository : IToppingRepository
{
    private static readonly Dictionary<string, Topping> Toppings = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Mozzarella"] = new Topping("Mozzarella", 1.00m),
        ["Funghi"] = new Topping("Funghi", 1.20m),
        ["Pepperoni"] = new Topping("Pepperoni", 1.50m),
        ["Prosciutto"] = new Topping("Prosciutto", 1.30m),
        ["Olive"] = new Topping("Olive", 0.80m),
    };

    public Task<IReadOnlyCollection<Topping>> GetAllAsync() =>
        Task.FromResult<IReadOnlyCollection<Topping>>(Toppings.Values.ToList());

    public Task<Topping> GetByNameAsync(string name)
    {
        if (!Toppings.TryGetValue(name, out var topping))
        {
            throw new KeyNotFoundException($"Topping '{name}' is not available in the catalog.");
        }

        return Task.FromResult(topping);
    }
}
