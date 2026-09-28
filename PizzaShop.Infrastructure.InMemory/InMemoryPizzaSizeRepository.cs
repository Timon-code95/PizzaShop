using PizzaShop.Domain;

namespace PizzaShop.Infrastructure.InMemory;

/// <summary>
/// Fake implementation of <see cref="IPizzaSizeRepository"/> that returns hardcoded base prices,
/// instead of reading them from a real database. It exists purely to let the domain and its
/// consumers (console app, BDD tests) work against the repository abstraction today, so that a
/// real database-backed implementation can later replace this one without touching any other project.
/// </summary>
public sealed class InMemoryPizzaSizeRepository : IPizzaSizeRepository
{
    private static readonly Dictionary<PizzaSize, decimal> BasePrices = new()
    {
        [PizzaSize.Small] = 5.00m,
        [PizzaSize.Medium] = 7.50m,
        [PizzaSize.Large] = 10.00m,
    };

    public Task<decimal> GetBasePriceAsync(PizzaSize size)
    {
        if (!BasePrices.TryGetValue(size, out var basePrice))
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Unknown pizza size.");
        }

        return Task.FromResult(basePrice);
    }
}
