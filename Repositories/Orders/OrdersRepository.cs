using Domain;
using System;
using Queue.Management;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Repositories.Orders
{
    public class OrdersRepository : IOrdersRepository
    {
        private const string __QueueName = "Queue.Orders";
        private readonly IQueueManagement _queueManagement;
        private readonly List<Action<Order>> _actions;

        public OrdersRepository(IQueueManagement queueManagement)
        {
            _actions = new List<Action<Order>>();
            _queueManagement = queueManagement;
            _queueManagement.CreateQueue(__QueueName);
            _queueManagement.SetQueueConsumer<Order>(__QueueName, OrderReceived);
        }
        
        public void Add(Order order)
        {
            this._queueManagement.Produce<Order>(order, __QueueName);
        }

        public void AddConsumer(Action<Order> action)
        {
            _actions.Add(action);
        }

        private void OrderReceived(Order order)
        {
            _actions.ForEach(action => action.Invoke(order));
        }
    }
}
