namespace PizzaShop.Domain;

/// <summary>
/// Reads the current <see cref="PricingSettings"/> configured by the pizzeria owner.
/// </summary>
public interface IPricingSettingsRepository
{
    /// <summary>
    /// Returns the current pricing settings.
    /// </summary>
    Task<PricingSettings> GetAsync();
}
