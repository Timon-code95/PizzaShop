namespace PizzaShop.Domain;

/// <summary>
/// A customer order made of one or more pizzas. Computes subtotal, delivery fee and grand total.
/// </summary>
public sealed class Order(string customerName)
{
    /// <summary>
    /// Delivery fee charged when the order does not qualify for free delivery.
    /// </summary>
    public const decimal StandardDeliveryFee = 3.50m;

    private readonly List<Pizza> _pizzas = [];

    public string CustomerName { get; } = customerName;

    public IReadOnlyList<Pizza> Pizzas => _pizzas;

    public void AddPizza(Pizza pizza) => _pizzas.Add(pizza);

    /// <summary>
    /// Sum of the price of every pizza in the order.
    /// </summary>
    public decimal Subtotal() => _pizzas.Sum(p => p.CalculatePrice());

    /// <summary>
    /// True when the order qualifies for free delivery, according to <paramref name="settings"/>.
    /// </summary>
    public bool HasFreeDelivery(PricingSettings settings) => Subtotal() >= settings.FreeDeliveryThreshold;

    /// <summary>
    /// Delivery fee for this order: zero if it qualifies for free delivery, <see cref="StandardDeliveryFee"/> otherwise.
    /// </summary>
    public decimal DeliveryFee(PricingSettings settings) => HasFreeDelivery(settings) ? 0m : StandardDeliveryFee;

    /// <summary>
    /// Grand total: subtotal plus delivery fee.
    /// </summary>
    public decimal GrandTotal(PricingSettings settings) => Subtotal() + DeliveryFee(settings);
}
