namespace PizzaShop.Domain;

/// <summary>
/// Available pizza sizes. The base price for each size (before any topping) is no longer a hardcoded
/// constant: it is configurable by the pizzeria owner and read through <see cref="IPizzaSizeRepository"/>.
/// </summary>
public enum PizzaSize
{
    Small,
    Medium,
    Large
}
