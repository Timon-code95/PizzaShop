using System.Globalization;
using PizzaShop.Bdd.Tests.Support;
using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions that let each feature declare, explicitly and self-sufficiently, which menu prices
/// and business settings it depends on (base price per size, topping prices, max toppings per pizza,
/// free delivery threshold, standard delivery fee). This avoids scenarios relying implicitly on whatever
/// data happens to be hardcoded in an infrastructure project: when a real database-backed repository
/// eventually replaces the current test doubles, these steps still configure isolated, known values for
/// every scenario, so the tests keep passing (and stay understandable) regardless of what the real
/// database contains at any given time.
/// </summary>
[Binding]
public class TestDataSteps(
    TestPizzaSizeRepository pizzaSizeRepository,
    TestToppingRepository toppingRepository,
    TestPricingSettingsRepository pricingSettingsRepository)
{
    [Given(@"che il prezzo base per il formato ""(.*)"" è ""(.*)"" euro")]
    public void DatoCheIlPrezzoBasePerIlFormatoE(string formato, string prezzo)
    {
        var size = Enum.Parse<PizzaSize>(formato, ignoreCase: true);
        pizzaSizeRepository.SetBasePrice(size, Parse(prezzo));
    }

    [Given(@"che sono configurati i seguenti prezzi base")]
    public void DatoCheSonoConfiguratiISeguentiPrezziBase(Table tabellaPrezzi)
    {
        foreach (var row in tabellaPrezzi.Rows)
        {
            var size = Enum.Parse<PizzaSize>(row["Formato"], ignoreCase: true);
            pizzaSizeRepository.SetBasePrice(size, Parse(row["PrezzoBase"]));
        }
    }

    [Given(@"che il topping ""(.*)"" costa ""(.*)"" euro")]
    public void DatoCheIlToppingCosta(string nome, string prezzo) =>
        toppingRepository.SetTopping(nome, Parse(prezzo));

    [Given(@"che sono disponibili i seguenti topping")]
    public void DatoCheSonoDisponibiliISeguentiTopping(Table tabellaTopping)
    {
        foreach (var row in tabellaTopping.Rows)
        {
            toppingRepository.SetTopping(row["Nome"], Parse(row["Prezzo"]));
        }
    }

    [Given(@"che il limite di ingredienti extra per pizza è (\d+)")]
    public void DatoCheIlLimiteDiIngredientiExtraPerPizzaE(int limite) =>
        pricingSettingsRepository.SetMaxToppingsPerPizza(limite);

    [Given(@"che la soglia di consegna gratuita è ""(.*)"" euro")]
    public void DatoCheLaSogliaDiConsegnaGratuitaE(string soglia) =>
        pricingSettingsRepository.SetFreeDeliveryThreshold(Parse(soglia));

    [Given(@"che la spesa di consegna standard è ""(.*)"" euro")]
    public void DatoCheLaSpesaDiConsegnaStandardE(string spesa) =>
        pricingSettingsRepository.SetStandardDeliveryFee(Parse(spesa));

    private static decimal Parse(string value) => decimal.Parse(value, CultureInfo.InvariantCulture);
}
