using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveTableController : ControllerBase
    {
        private readonly IMongoCollection<ReserveTable> _reservations;
        private readonly IMongoCollection<GoogleRegister> _googleRegister;
        public ReserveTableController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("pregopantry");
            _reservations =  database.GetCollection<ReserveTable>("reservetable");
            _googleRegister = database.GetCollection<GoogleRegister>("googlelogin");
        }

        [HttpPost("MakeReservation")]
        public  IActionResult CreateReservation(ReserveTable reserveTable)
        {
            if( reserveTable != null)
            {
                _googleRegister.Find(x => x.Jwt == reserveTable.Jwt);
                _reservations.InsertOne(reserveTable);
                return Ok(reserveTable);
            }
            return BadRequest("Data not found");
        }
    }
}
