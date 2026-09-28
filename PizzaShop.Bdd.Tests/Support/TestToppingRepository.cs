using PizzaShop.Domain;

namespace PizzaShop.Bdd.Tests.Support;

/// <summary>
/// Test double for <see cref="IToppingRepository"/>: starts empty and is populated per-scenario by
/// "Given che il topping ... costa ... euro" steps (see
/// <see cref="PizzaShop.Bdd.Tests.Steps.TestDataSteps"/>). Unlike the demo implementation in
/// <c>PizzaShop.Infrastructure.InMemory</c>, no topping is assumed unless the scenario declares it
/// explicitly: this keeps every scenario self-contained and independent from whatever a real database
/// might contain at any given time.
/// </summary>
public sealed class TestToppingRepository : IToppingRepository
{
    private readonly Dictionary<string, Topping> _toppings = new(StringComparer.OrdinalIgnoreCase);

    public void SetTopping(string name, decimal price) => _toppings[name] = new Topping(name, price);

    public Task<IReadOnlyCollection<Topping>> GetAllAsync() =>
        Task.FromResult<IReadOnlyCollection<Topping>>(_toppings.Values.ToList());

    public Task<Topping> GetByNameAsync(string name)
    {
        if (!_toppings.TryGetValue(name, out var topping))
        {
            throw new KeyNotFoundException($"Topping '{name}' is not available in the catalog.");
        }

        return Task.FromResult(topping);
    }
}
