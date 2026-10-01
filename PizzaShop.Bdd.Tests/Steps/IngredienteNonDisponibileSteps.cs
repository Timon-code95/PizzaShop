using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for IngredienteNonDisponibile.feature.
/// Il passo "Dato che ordino una pizza di formato ..." è riusato da CommonPizzaSteps:
/// qui serve aggiungere solo il When e il Then specifici di questo scenario.
/// </summary>
[Binding]
public class IngredienteNonDisponibileSteps(PizzaOrderContext context, IToppingRepository toppingRepository)
{
    [When(@"provo ad aggiungere un ingrediente extra non disponibile ""(.*)""")]
    public async Task QuandoProvoAdAggiungereUnIngredienteExtraNonDisponibile(string nomeIngrediente)
    {
        context.LastError = await Record.ExceptionAsync(() => toppingRepository.GetByNameAsync(nomeIngrediente));
    }

    [Then(@"dovrebbe essere sollevato un errore che segnala che l'ingrediente non è disponibile nel catalogo")]
    public void AlloraDovrebbeEssereSollevatoUnErroreIngredienteNonDisponibile()
    {
        Assert.NotNull(context.LastError);
        Assert.IsType<KeyNotFoundException>(context.LastError);
    }
}
