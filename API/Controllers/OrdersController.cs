using Microsoft.AspNetCore.Mvc;
using Domain;
using System;
using Microsoft.Extensions.Logging;
using Queue.Management;
using Repositories.Orders;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersRepository _ordersManagement;

        public OrdersController(ILogger<OrdersController> logger, IOrdersRepository ordersManagement)
        {
            this._logger = logger;
            this._ordersManagement = ordersManagement;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            try
            {
                this._ordersManagement.Add(order);
                return Accepted(order);
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, title: "Error on create order");
            }
        }
    }
}
