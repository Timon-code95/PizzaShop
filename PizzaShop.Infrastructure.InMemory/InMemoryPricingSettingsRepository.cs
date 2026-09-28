using PizzaShop.Domain;

namespace PizzaShop.Infrastructure.InMemory;

/// <summary>
/// Fake implementation of <see cref="IPricingSettingsRepository"/> that returns a hardcoded, fixed set
/// of settings, instead of reading them from a real database. It exists purely to let the domain and
/// its consumers (console app, BDD tests) work against the repository abstraction today, so that a
/// real database-backed implementation (where the pizzeria owner can edit these values) can later
/// replace this one without touching any other project.
/// </summary>
public sealed class InMemoryPricingSettingsRepository : IPricingSettingsRepository
{
    private static readonly PricingSettings Settings = new(
        MaxToppingsPerPizza: 5,
        FreeDeliveryThreshold: 25.00m,
        StandardDeliveryFee: 3.50m);

    public Task<PricingSettings> GetAsync() => Task.FromResult(Settings);
}
