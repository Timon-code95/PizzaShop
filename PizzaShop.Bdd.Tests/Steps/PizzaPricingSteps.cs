using System.Globalization;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for CalcoloPrezzoPizza.feature.
/// </summary>
[Binding]
public class PizzaPricingSteps(PizzaOrderContext context)
{
    [Then(@"il prezzo della pizza dovrebbe essere ""(.*)"" euro")]
    public void AlloraIlPrezzoDellaPizzaDovrebbeEssere(string prezzoAtteso)
    {
        var atteso = decimal.Parse(prezzoAtteso, CultureInfo.InvariantCulture);
        Assert.Equal(atteso, context.CurrentPizza!.CalculatePrice());
    }
}
