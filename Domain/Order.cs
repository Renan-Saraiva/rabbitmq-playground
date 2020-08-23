using System;

namespace Domain
{
    public class Order
    {
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public DateTime BuyedOn { get; set; }
    }
}
