using PizzaShop.Domain;
using PizzaShop.Infrastructure.InMemory;
using Reqnroll;
using Reqnroll.BoDi;

namespace PizzaShop.Bdd.Tests.Support;

/// <summary>
/// Composes the dependencies needed by the domain (repositories + <see cref="ToppingCatalog"/>) before
/// each scenario runs, using Reqnroll's built-in dependency injection (context injection). Today the
/// repositories are the in-memory fakes from <c>PizzaShop.Infrastructure.InMemory</c>; a real
/// database-backed implementation would be registered here instead, without touching the step
/// definitions, since they only depend on the domain's interfaces.
/// </summary>
[Binding]
public class DependencyRegistrationHooks
{
    [BeforeScenario]
    public void RegisterDependencies(ObjectContainer container)
    {
        var pizzaSizeRepository = new InMemoryPizzaSizeRepository();
        var toppingRepository = new InMemoryToppingRepository();
        var pricingSettingsRepository = new InMemoryPricingSettingsRepository();

        container.RegisterInstanceAs<IPizzaSizeRepository>(pizzaSizeRepository);
        container.RegisterInstanceAs<IToppingRepository>(toppingRepository);
        container.RegisterInstanceAs<IPricingSettingsRepository>(pricingSettingsRepository);
        container.RegisterInstanceAs(new ToppingCatalog(toppingRepository));
    }
}
