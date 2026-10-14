using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for LimiteIngredienti.feature.
/// </summary>
[Binding]
public class ToppingLimitSteps(
    PizzaOrderContext context,
    IToppingRepository toppingRepository,
    IPricingSettingsRepository pricingSettingsRepository)
{
    [Given(@"che la pizza ha già (\d+) ingredienti extra")]
    public async Task DatoCheLaPizzaHaGiaNIngredientiExtra(int numeroIngredienti)
    {
        var settings = await pricingSettingsRepository.GetAsync();
        var toppings = await toppingRepository.GetAllAsync();

        foreach (var topping in toppings.Take(numeroIngredienti))
        {
            context.CurrentPizza!.AddTopping(topping, settings);
        }
    }

    [Then(@"dovrebbe essere sollevato un errore che segnala il superamento del limite di ingredienti extra")]
    public void AlloraDovrebbeEssereSollevatoUnErrore()
    {
        Assert.NotNull(context.LastError);
        Assert.IsType<InvalidOperationException>(context.LastError);
    }
}
