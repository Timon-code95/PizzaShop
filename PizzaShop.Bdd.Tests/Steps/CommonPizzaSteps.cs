using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions shared by every feature that needs to build a pizza (format + extra toppings).
/// Kept in a single binding class so the same Gherkin phrasing is never duplicated across step classes,
/// which would otherwise cause Reqnroll to report an "ambiguous step" error.
/// </summary>
[Binding]
public class CommonPizzaSteps(
    PizzaOrderContext context,
    IPizzaSizeRepository pizzaSizeRepository,
    ToppingCatalog toppingCatalog,
    IPricingSettingsRepository pricingSettingsRepository)
{
    [Given(@"che ordino una pizza di formato ""(.*)""")]
    public async Task DatoCheOrdinoUnaPizzaDiFormato(string formato)
    {
        var size = Enum.Parse<PizzaSize>(formato, ignoreCase: true);
        var basePrice = await pizzaSizeRepository.GetBasePriceAsync(size);
        context.CurrentPizza = new Pizza(size, basePrice);
    }

    [When(@"aggiungo i seguenti ingredienti extra ""(.*)""")]
    public async Task QuandoAggiungoISeguentiIngredientiExtra(string ingredienti)
    {
        var settings = await pricingSettingsRepository.GetAsync();

        foreach (var nome in SplitIngredienti(ingredienti))
        {
            var topping = await toppingCatalog.GetAsync(nome);
            context.CurrentPizza!.AddTopping(topping, settings);
        }
    }

    /// <summary>
    /// Splits a comma-separated list of topping names coming from a Gherkin step/table cell.
    /// </summary>
    internal static IEnumerable<string> SplitIngredienti(string ingredienti) =>
        string.IsNullOrWhiteSpace(ingredienti)
            ? []
            : ingredienti.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
