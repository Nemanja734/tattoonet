using System;

namespace Core.Entities.OrderAggregate;

// this property will be owned by our order and will also be in the same table
public class PaymentSummary
{
    public int Last4 { get; set; }
    public required string Brand { get; set; }
    public int ExpMonth { get; set; }
    public int ExpYear { get; set; }
}
