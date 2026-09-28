using PizzaShop.Domain;

namespace PizzaShop.Bdd.Tests.Support;

/// <summary>
/// Test double for <see cref="IPizzaSizeRepository"/>: starts empty and is populated per-scenario by
/// "Given che il prezzo base per il formato ... è ... euro" steps (see
/// <see cref="PizzaShop.Bdd.Tests.Steps.TestDataSteps"/>). Unlike the demo implementation in
/// <c>PizzaShop.Infrastructure.InMemory</c>, no price is assumed unless the scenario declares it
/// explicitly: this keeps every scenario self-contained and independent from whatever a real database
/// might contain at any given time.
/// </summary>
public sealed class TestPizzaSizeRepository : IPizzaSizeRepository
{
    private readonly Dictionary<PizzaSize, decimal> _basePrices = [];

    public void SetBasePrice(PizzaSize size, decimal basePrice) => _basePrices[size] = basePrice;

    public Task<decimal> GetBasePriceAsync(PizzaSize size)
    {
        if (!_basePrices.TryGetValue(size, out var basePrice))
        {
            throw new InvalidOperationException(
                $"No base price was declared for size '{size}' in this scenario. Add a step like " +
                $"'Given che il prezzo base per il formato \"{size}\" è \"...\" euro'.");
        }

        return Task.FromResult(basePrice);
    }
}
