using System.Globalization;
using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for ScontoEConsegna.feature.
/// </summary>
[Binding]
public class OrderSteps(PizzaOrderContext context)
{
    [Given(@"che il cliente ""(.*)"" ha ordinato le seguenti pizze")]
    public void DatoCheIlClienteHaOrdinatoLeSeguentiPizze(string cliente, Table tabellaPizze)
    {
        var order = new Order(cliente);

        foreach (var row in tabellaPizze.Rows)
        {
            var pizza = new Pizza(Enum.Parse<PizzaSize>(row["Formato"], ignoreCase: true));

            foreach (var nome in CommonPizzaSteps.SplitIngredienti(row["Ingredienti"]))
            {
                pizza.AddTopping(ToppingCatalog.Get(nome));
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

    [Then(@"lo sconto applicato dovrebbe essere ""(.*)"" euro")]
    public void AlloraLoScontoApplicatoDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.Discount());

    [Then(@"la spesa di consegna dovrebbe essere ""(.*)"" euro")]
    public void AlloraLaSpesaDiConsegnaDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.DeliveryFee());

    [Then(@"il totale finale dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlTotaleFinaleDovrebbeEssere(string valoreAtteso) =>
        Assert.Equal(Parse(valoreAtteso), context.CurrentOrder!.GrandTotal());

    private static decimal Parse(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
