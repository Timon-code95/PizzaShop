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
    IPizzaSizeRepository pizzaSizeRepository,
    ToppingCatalog toppingCatalog,
    IPricingSettingsRepository pricingSettingsRepository)
{
    [Given(@"che il cliente ""(.*)"" ha ordinato le seguenti pizze")]
    public async Task DatoCheIlClienteHaOrdinatoLeSeguentiPizze(string cliente, Table tabellaPizze)
    {
        var settings = await pricingSettingsRepository.GetAsync();
        context.Settings = settings;

        var order = new Order(cliente);

        foreach (var row in tabellaPizze.Rows)
        {
            var size = Enum.Parse<PizzaSize>(row["Formato"], ignoreCase: true);
            var basePrice = await pizzaSizeRepository.GetBasePriceAsync(size);
            var pizza = new Pizza(size, basePrice);

            foreach (var nome in CommonPizzaSteps.SplitIngredienti(row["Ingredienti"]))
            {
                var topping = await toppingCatalog.GetAsync(nome);
                pizza.AddTopping(topping, settings);
            }

            order.AddPizza(pizza);
        }

        context.CurrentOrder = order;
    }

    [When(@"calcolo il totale dell'ordine")]
    public void QuandoCalcoloIlTotaleDellOrdine()
    {
        // Nessuna azione esplicita necessaria: Order calcola i valori "on demand" negli step Then successivi.
    }

    [Then(@"il subtotale dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlSubtotaleDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.Subtotal());

    [Then(@"la spesa di consegna dovrebbe essere ""(.*)"" euro")]
    public void AlloraLaSpesaDiConsegnaDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.DeliveryFee(context.Settings!));

    [Then(@"il totale finale dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlTotaleFinaleDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.GrandTotal(context.Settings!));

    private static decimal Parse(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
