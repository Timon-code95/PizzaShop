using PizzaShop.Domain;
using Reqnroll;
using Reqnroll.BoDi;

namespace PizzaShop.Bdd.Tests.Support;

/// <summary>
/// Composes the dependencies needed by the domain (repositories + <see cref="ToppingCatalog"/>) before
/// each scenario runs, using Reqnroll's built-in dependency injection (context injection). Each scenario
/// gets its own empty <see cref="TestPizzaSizeRepository"/>/<see cref="TestToppingRepository"/>/
/// <see cref="TestPricingSettingsRepository"/> instance: prices and settings are populated explicitly by
/// the scenario itself (see <see cref="Steps.TestDataSteps"/>), instead of relying on whatever a shared
/// implementation happens to hardcode. This keeps every scenario self-contained and readable without
/// having to look at infrastructure code to know which numbers to expect, and it will keep working
/// unchanged even once a real database-backed repository is introduced for production use: only that
/// production wiring (e.g. in the console app's composition root) would point at the real database,
/// while these tests would keep using their own isolated, declared values.
/// </summary>
[Binding]
public class DependencyRegistrationHooks
{
    [BeforeScenario]
    public void RegisterDependencies(ObjectContainer container)
    {
        var pizzaSizeRepository = new TestPizzaSizeRepository();
        var toppingRepository = new TestToppingRepository();
        var pricingSettingsRepository = new TestPricingSettingsRepository();

        container.RegisterInstanceAs(pizzaSizeRepository);
        container.RegisterInstanceAs(toppingRepository);
        container.RegisterInstanceAs(pricingSettingsRepository);
        container.RegisterInstanceAs<IPizzaSizeRepository>(pizzaSizeRepository);
        container.RegisterInstanceAs<IToppingRepository>(toppingRepository);
        container.RegisterInstanceAs<IPricingSettingsRepository>(pricingSettingsRepository);
        container.RegisterInstanceAs(new ToppingCatalog(toppingRepository));
    }
}
