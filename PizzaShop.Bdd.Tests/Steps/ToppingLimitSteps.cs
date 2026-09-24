using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for LimiteIngredienti.feature.
/// </summary>
[Binding]
public class ToppingLimitSteps(PizzaOrderContext context)
{
    [Given(@"che la pizza ha già 5 ingredienti extra")]
    public void DatoCheLaPizzaHaGiaCinqueIngredientiExtra()
    {
        foreach (var topping in ToppingCatalog.All.Take(Pizza.MaxToppings))
        {
            context.CurrentPizza!.AddTopping(topping);
        }
    }

    [When(@"provo ad aggiungere un ulteriore ingrediente extra ""(.*)""")]
    public void QuandoProvoAdAggiungereUnUlterioreIngredienteExtra(string nomeIngrediente)
    {
        context.LastError = Record.Exception(() => context.CurrentPizza!.AddTopping(ToppingCatalog.Get(nomeIngrediente)));
    }

    [Then(@"l'aggiunta degli ingredienti extra dovrebbe andare a buon fine")]
    public void AlloraLAggiuntaDegliIngredientiExtraDovrebbeAndareABuonFine()
    {
        Assert.Equal(Pizza.MaxToppings, context.CurrentPizza!.Toppings.Count);
    }

    [Then(@"dovrebbe essere sollevato un errore che segnala il superamento del limite di ingredienti extra")]
    public void AlloraDovrebbeEssereSollevatoUnErrore()
    {
        Assert.NotNull(context.LastError);
        Assert.IsType<InvalidOperationException>(context.LastError);
    }
}
