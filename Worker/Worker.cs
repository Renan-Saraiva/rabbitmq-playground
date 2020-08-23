using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories.Orders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOrdersRepository _ordersRepository;

        public Worker(ILogger<Worker> logger, IOrdersRepository ordersRepository)
        {
            this._logger = logger;
            this._ordersRepository = ordersRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._ordersRepository.AddConsumer(OrderReceveid);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }

        private void OrderReceveid(Order order) 
        {
            Console.WriteLine(JsonSerializer.Serialize(order));
        }
    }
}
