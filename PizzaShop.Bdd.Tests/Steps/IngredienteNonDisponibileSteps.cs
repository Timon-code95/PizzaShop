using PizzaShop.Domain;
using Reqnroll;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Step definitions for IngredienteNonDisponibile.feature.
/// I passi "Dato che ordino una pizza di formato ..." e "provo ad aggiungere i seguenti ingredienti extra ..."
/// sono riusati da CommonPizzaSteps: qui servono solo i Then specifici di questo scenario.
/// </summary>
[Binding]
public class IngredienteNonDisponibileSteps(PizzaOrderContext context)
{
    [Then(@"dovrebbe essere sollevato un errore che segnala che l'ingrediente non è disponibile nel catalogo")]
    public void AlloraDovrebbeEssereSollevatoUnErroreIngredienteNonDisponibile()
    {
        Assert.NotNull(context.LastError);
        Assert.IsType<KeyNotFoundException>(context.LastError);
    }

    [Then(@"il messaggio di errore dovrebbe citare l'ingrediente ""(.*)""")]
    public void AlloraIlMessaggioDiErroreDovrebbeCitareLIngrediente(string nomeIngrediente)
    {
        Assert.NotNull(context.LastError);
        Assert.Contains(nomeIngrediente, context.LastError!.Message);
    }
}
