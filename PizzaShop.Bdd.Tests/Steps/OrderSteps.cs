using System.Globalization;
using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for ScontoEConsegna.feature.
/// </summary>
[Binding]
public class OrderSteps(
    PizzaOrderContext context,
    OrderCompositionService orderComposer)
{
    [Given(@"che il cliente ""(.*)"" ha ordinato le seguenti pizze")]
    public async Task DatoCheIlClienteHaOrdinatoLeSeguentiPizze(string cliente, Table tabellaPizze)
    {
        var pizze = tabellaPizze.Rows.Select(row => new PizzaOrderRequest(
            Enum.Parse<PizzaSize>(row["Formato"], ignoreCase: true),
            CommonPizzaSteps.SplitIngredienti(row["Ingredienti"]).ToList()));

        context.CurrentOrder = await orderComposer.CreateOrderAsync(cliente, pizze);
    }

    [When(@"calcolo il totale dell'ordine")]
    public async Task QuandoCalcoloIlTotaleDellOrdine()
    {
        context.Totals = await orderComposer.CalculateTotalsAsync(context.CurrentOrder!);
    }

    [Then(@"il subtotale dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlSubtotaleDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.Totals!.Subtotal);

    [Then(@"la spesa di consegna dovrebbe essere ""(.*)"" euro")]
    public void AlloraLaSpesaDiConsegnaDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.Totals!.DeliveryFee);

    [Then(@"il totale finale dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlTotaleFinaleDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.Totals!.GrandTotal);

    private static decimal Parse(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
