namespace PizzaShop.Domain;

/// <summary>
/// A customer order made of one or more pizzas. Computes subtotal, discount, delivery fee and grand total.
/// </summary>
public sealed class Order(string customerName)
{
    /// <summary>
    /// Minimum total (after discount) to qualify for free delivery.
    /// </summary>
    public const decimal FreeDeliveryThreshold = 25.00m;

    /// <summary>
    /// Delivery fee charged when the order does not qualify for free delivery.
    /// </summary>
    public const decimal StandardDeliveryFee = 3.50m;

    private readonly List<Pizza> _pizzas = [];

    public string CustomerName { get; } = customerName;

    public IReadOnlyList<Pizza> Pizzas => _pizzas;

    public void AddPizza(Pizza pizza) => _pizzas.Add(pizza);

    /// <summary>
    /// Sum of the price of every pizza in the order, before any discount.
    /// </summary>
    public decimal Subtotal() => _pizzas.Sum(p => p.CalculatePrice());

    /// <summary>
    /// Discount amount applied to this order, according to <see cref="DiscountPolicy"/>.
    /// </summary>
    public decimal Discount() => DiscountPolicy.CalculateDiscount(Subtotal());

    /// <summary>
    /// Order total after discount, before delivery fee.
    /// </summary>
    public decimal TotalAfterDiscount() => Subtotal() - Discount();

    /// <summary>
    /// True when the order qualifies for free delivery.
    /// </summary>
    public bool HasFreeDelivery() => TotalAfterDiscount() >= FreeDeliveryThreshold;

    /// <summary>
    /// Delivery fee for this order: zero if it qualifies for free delivery, <see cref="StandardDeliveryFee"/> otherwise.
    /// </summary>
    public decimal DeliveryFee() => HasFreeDelivery() ? 0m : StandardDeliveryFee;

    /// <summary>
    /// Grand total: total after discount plus delivery fee.
    /// </summary>
    public decimal GrandTotal() => TotalAfterDiscount() + DeliveryFee();
}
