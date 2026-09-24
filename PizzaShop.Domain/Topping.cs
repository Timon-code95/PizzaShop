namespace PizzaShop.Domain;

/// <summary>
/// An extra ingredient that can be added on top of a pizza, with its own additional price.
/// </summary>
public sealed record Topping(string Name, decimal Price);
