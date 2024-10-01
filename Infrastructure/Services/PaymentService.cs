using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

// IConfiguration for stripe key
// ICartService to see whats in the cart (we won't trust the price of cart)
// IGenericRepository to get the price of products and delivery method
public class PaymentService(IConfiguration config, ICartService cartService, IUnitOfWork unit) : IPaymentService
{
    // validate cart, validate delivery information
    // and create payment intent so we can send it to stripe
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

        var cart = await cartService.GetCartAsync(cartId);

        if (cart == null) return null;

        var shippingPrice = 0m;

        // check if delivery method has value and add delivery price
        if (cart.DeliveryMethodId.HasValue)
        {
            var DeliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync((int)cart.DeliveryMethodId);     // cast from int? to int

            if (DeliveryMethod == null) return null;

            shippingPrice = DeliveryMethod.Price;
        }

        // validate and update items in the cart
        foreach (var item in cart.Items)
        {
            var productItem = await unit.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId);

            if (productItem == null) return null;

            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;

        // check if we have a PaymentIntentId
        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                // convert decimals into longtype, as that is what stripe uses for the amount
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100,
                Currency = "eur",
                PaymentMethodTypes = ["card"]
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else        // if PaymentIntentId is given, update intent
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100
            };
            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }
        await cartService.SetCartAsync(cart);

        return cart;
    }
}
