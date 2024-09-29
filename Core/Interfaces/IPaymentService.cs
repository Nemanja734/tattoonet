using System;
using Core.Entities;

namespace Core.Interfaces;

// interface for payment intent
public interface IPaymentService
{
    // method returns task of shoppingcart
    Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
}
