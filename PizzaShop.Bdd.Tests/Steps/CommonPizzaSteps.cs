using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions shared by every feature that needs to build a pizza (format + extra toppings).
/// Kept in a single binding class so the same Gherkin phrasing is never duplicated across step classes,
/// which would otherwise cause Reqnroll to report an "ambiguous step" error.
/// </summary>
[Binding]
public class CommonPizzaSteps(PizzaOrderContext context)
{
    [Given(@"che ordino una pizza di formato ""(.*)""")]
    public void DatoCheOrdinoUnaPizzaDiFormato(string formato)
    {
        context.CurrentPizza = new Pizza(Enum.Parse<PizzaSize>(formato, ignoreCase: true));
    }

    [When(@"aggiungo i seguenti ingredienti extra ""(.*)""")]
    public void QuandoAggiungoISeguentiIngredientiExtra(string ingredienti)
    {
        foreach (var nome in SplitIngredienti(ingredienti))
        {
            context.CurrentPizza!.AddTopping(ToppingCatalog.Get(nome));
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
