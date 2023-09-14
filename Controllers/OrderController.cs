using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IMongoCollection<Order> _Orders;
        public OrderController(IMongoClient mongoClient)
        {
            var Database = mongoClient.GetDatabase("pregopantry");
            _Orders = Database.GetCollection<Order>("Orders");
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders(string jwt)
        {
            var order = _Orders.Find( c => c.Jwt == jwt).ToList();
            if(order != null)
            {
                return Ok(order);
            }
            return BadRequest();
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(Order order)
        {
            _Orders.InsertOne(order);
            return Ok(order);
        }
    }
}
