namespace Lab01.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Lab01.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> orders = Enumerable.Range(1, 3)
            .SelectMany(customerId => Enumerable.Range(1, 2).Select(orderIndex => new Order
            {
                Id = (customerId - 1) * 2 + orderIndex,
                Amount = (customerId + orderIndex) * 10
            }))
            .ToList();

        [HttpGet("list_orders")]
        public ActionResult<IEnumerable<Order>> ListOrders()
        {
            return Ok(orders);
        }
    }
}
