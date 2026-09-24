using PizzaShop.Domain;

namespace PizzaShop.Bdd.Tests.Steps;

/// <summary>
/// Plain object shared across step definition classes within the same scenario, thanks to Reqnroll's
/// built-in dependency injection (context injection). Keeps state (the pizza/order being built, the
/// last captured error) so different step classes can cooperate without duplicating step definitions.
/// </summary>
public class PizzaOrderContext
{
    public Pizza? CurrentPizza { get; set; }

    public Order? CurrentOrder { get; set; }

    public Exception? LastError { get; set; }
}
