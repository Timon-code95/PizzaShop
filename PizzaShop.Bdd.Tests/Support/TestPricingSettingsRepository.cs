using PizzaShop.Domain;

namespace PizzaShop.Bdd.Tests.Support;

/// <summary>
/// Test double for <see cref="IPricingSettingsRepository"/>: has no settings until a scenario declares
/// them via "Given che il limite di ingredienti extra per pizza è ...", "Given che la soglia di
/// consegna gratuita è ... euro" or "Given che la spesa di consegna standard è ... euro" steps (see
/// <see cref="PizzaShop.Bdd.Tests.Steps.TestDataSteps"/>).
/// Unlike the demo implementation in <c>PizzaShop.Infrastructure.InMemory</c>, no value is assumed
/// unless the scenario declares it explicitly: this keeps every scenario self-contained and independent
/// from whatever a real database might contain at any given time.
/// </summary>
public sealed class TestPricingSettingsRepository : IPricingSettingsRepository
{
    private int _maxToppingsPerPizza = 5;
    private decimal _freeDeliveryThreshold = 25.00m;
    private decimal _standardDeliveryFee = 3.50m;

    public void SetMaxToppingsPerPizza(int value) => _maxToppingsPerPizza = value;

    public void SetFreeDeliveryThreshold(decimal value) => _freeDeliveryThreshold = value;

    public void SetStandardDeliveryFee(decimal value) => _standardDeliveryFee = value;

    public Task<PricingSettings> GetAsync() =>
        Task.FromResult(new PricingSettings(_maxToppingsPerPizza, _freeDeliveryThreshold, _standardDeliveryFee));
}
