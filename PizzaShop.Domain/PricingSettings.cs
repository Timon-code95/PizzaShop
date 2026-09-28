namespace PizzaShop.Domain;

/// <summary>
/// Business rules configurable by the pizzeria owner: how many extra toppings can be added to a
/// single pizza, the minimum order total (after toppings, before delivery fee) that qualifies for
/// free delivery, and the delivery fee charged when the order does not qualify.
/// </summary>
public sealed record PricingSettings(int MaxToppingsPerPizza, decimal FreeDeliveryThreshold, decimal StandardDeliveryFee);
