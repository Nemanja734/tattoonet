using System;

namespace Core.Entities.OrderAggregate;

// this property will be owned by our order-item and will also be in the same table
public class ProductItemOrdered
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
}
