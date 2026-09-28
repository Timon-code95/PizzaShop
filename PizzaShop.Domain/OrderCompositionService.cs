namespace PizzaShop.Domain;

/// <summary>
/// Describes a single pizza to add to an order being composed via
/// <see cref="OrderCompositionService.CreateOrderAsync"/>: the desired size, plus the names of the
/// extra toppings to add (resolved by name against the topping catalog).
/// </summary>
public sealed record PizzaOrderRequest(PizzaSize Size, IReadOnlyCollection<string> ToppingNames);

/// <summary>
/// Result of <see cref="OrderCompositionService.CalculateTotalsAsync"/>: the subtotal, delivery fee
/// and grand total of an <see cref="Order"/>, computed against the current <see cref="PricingSettings"/>.
/// </summary>
public sealed record OrderTotals(decimal Subtotal, decimal DeliveryFee, decimal GrandTotal);

/// <summary>
/// Application-level service that orchestrates the composition of an order: resolves menu data
/// (base price per size, pricing settings) from the Data Access Component and hands it, already
/// resolved, to the pure domain entities (<see cref="Pizza"/>, <see cref="Order"/>). This is the
/// class that actually calls <see cref="IPizzaSizeRepository"/> and
/// <see cref="IPricingSettingsRepository"/>: <see cref="Pizza"/> and <see cref="Order"/> never call
/// them directly, so that they can stay simple, pure domain entities (see the class diagram notes
/// for why). It is also the only class that creates and assembles an <see cref="Order"/>: the
/// caller never calls <c>new Order(...)</c> or <see cref="Order.AddPizza"/> directly.
/// </summary>
/// <remarks>
/// In the current demo this class is used directly by <c>Program.cs</c> (the console demo) and by
/// the BDD step definitions. In a real deployment, an Order API component would call this service
/// instead of talking to the repositories itself.
/// </remarks>
public sealed class OrderCompositionService(
    IPizzaSizeRepository pizzaSizeRepository,
    IPricingSettingsRepository pricingSettingsRepository,
    ToppingCatalog toppingCatalog)
{
    /// <summary>
    /// Creates a new, empty <see cref="Pizza"/> of the given size, with its base price already
    /// resolved from <see cref="IPizzaSizeRepository"/>.
    /// </summary>
    public async Task<Pizza> CreatePizzaAsync(PizzaSize size)
    {
        var basePrice = await pizzaSizeRepository.GetBasePriceAsync(size);
        return new Pizza(size, basePrice);
    }

    /// <summary>
    /// Reads the current pricing settings (max toppings per pizza, free delivery threshold, standard
    /// delivery fee) from <see cref="IPricingSettingsRepository"/>.
    /// </summary>
    public Task<PricingSettings> GetPricingSettingsAsync() => pricingSettingsRepository.GetAsync();

    /// <summary>
    /// Finds a topping by name (case-insensitive), delegating to <see cref="ToppingCatalog"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when the topping is not part of the catalog.</exception>
    public Task<Topping> GetToppingAsync(string name) => toppingCatalog.GetAsync(name);

    /// <summary>
    /// Creates a full <see cref="Order"/> for <paramref name="customerName"/>, composed of the given
    /// pizzas: resolves the base price of each size, resolves every requested topping by name, and
    /// assembles everything into the returned <see cref="Order"/>. This is the only place where an
    /// <see cref="Order"/> is created and populated: callers never call <c>new Order(...)</c> or
    /// <see cref="Order.AddPizza"/> themselves.
    /// </summary>
    public async Task<Order> CreateOrderAsync(string customerName, IEnumerable<PizzaOrderRequest> pizzas)
    {
        var settings = await GetPricingSettingsAsync();
        var order = new Order(customerName);

        foreach (var request in pizzas)
        {
            var pizza = await CreatePizzaAsync(request.Size);

            foreach (var toppingName in request.ToppingNames)
            {
                var topping = await GetToppingAsync(toppingName);
                pizza.AddTopping(topping, settings);
            }

            order.AddPizza(pizza);
        }

        return order;
    }

    /// <summary>
    /// Computes subtotal, delivery fee and grand total for <paramref name="order"/>, against the
    /// current <see cref="PricingSettings"/>.
    /// </summary>
    public async Task<OrderTotals> CalculateTotalsAsync(Order order)
    {
        var settings = await GetPricingSettingsAsync();
        return new OrderTotals(
            order.Subtotal(),
            order.DeliveryFee(settings),
            order.GrandTotal(settings));
    }
}
