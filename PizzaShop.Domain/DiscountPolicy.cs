namespace PizzaShop.Domain;

/// <summary>
/// Encapsulates the discount rule applied to an order based on its subtotal.
/// </summary>
public static class DiscountPolicy
{
    /// <summary>
    /// Orders whose subtotal exceeds this threshold get a discount.
    /// </summary>
    public const decimal DiscountThreshold = 30.00m;

    /// <summary>
    /// Discount rate applied when the subtotal exceeds <see cref="DiscountThreshold"/>.
    /// </summary>
    public const decimal DiscountRate = 0.10m;

    /// <summary>
    /// Computes the discount amount (in currency) for the given order subtotal.
    /// </summary>
    public static decimal CalculateDiscount(decimal subtotal) =>
        subtotal > DiscountThreshold ? subtotal * DiscountRate : 0m;
}
