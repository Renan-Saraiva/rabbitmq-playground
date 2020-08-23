using Domain;
using System;

namespace Repositories.Orders
{
    public interface IOrdersRepository
    {
        public void Add(Order order);
        public void AddConsumer(Action<Order> action);
    }
}
