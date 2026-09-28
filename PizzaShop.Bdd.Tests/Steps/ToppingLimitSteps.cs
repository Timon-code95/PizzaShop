using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for LimiteIngredienti.feature.
/// </summary>
[Binding]
public class ToppingLimitSteps(
    PizzaOrderContext context,
    ToppingCatalog toppingCatalog,
    IPricingSettingsRepository pricingSettingsRepository)
{
    [Given(@"che la pizza ha già 5 ingredienti extra")]
    public async Task DatoCheLaPizzaHaGiaCinqueIngredientiExtra()
    {
        var settings = await pricingSettingsRepository.GetAsync();
        var toppings = await toppingCatalog.GetAllAsync();

        foreach (var topping in toppings.Take(settings.MaxToppingsPerPizza))
        {
            context.CurrentPizza!.AddTopping(topping, settings);
        }
    }

    [When(@"provo ad aggiungere un ulteriore ingrediente extra ""(.*)""")]
    public async Task QuandoProvoAdAggiungereUnUlterioreIngredienteExtra(string nomeIngrediente)
    {
        var settings = await pricingSettingsRepository.GetAsync();
        context.LastError = await Record.ExceptionAsync(async () =>
        {
            var topping = await toppingCatalog.GetAsync(nomeIngrediente);
            context.CurrentPizza!.AddTopping(topping, settings);
        });
    }

    [Then(@"l'aggiunta degli ingredienti extra dovrebbe andare a buon fine")]
    public async Task AlloraLAggiuntaDegliIngredientiExtraDovrebbeAndareABuonFine()
    {
        var settings = await pricingSettingsRepository.GetAsync();
        Assert.Equal(settings.MaxToppingsPerPizza, context.CurrentPizza!.Toppings.Count);
    }

    [Then(@"dovrebbe essere sollevato un errore che segnala il superamento del limite di ingredienti extra")]
    public void AlloraDovrebbeEssereSollevatoUnErrore()
    {
        Assert.NotNull(context.LastError);
        Assert.IsType<InvalidOperationException>(context.LastError);
    }
}
