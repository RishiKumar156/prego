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
        private readonly IMongoCollection<JwtCollection> _jwtCollection;
        public ReserveTableController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("pregopantry");
            _reservations =  database.GetCollection<ReserveTable>("reservetable");
            _jwtCollection = database.GetCollection<JwtCollection>("JwtTokens");
        }

        [HttpPost("MakeReservation")]
        public  IActionResult CreateReservation(ReserveTable reserveTable, string Jwt)
        {
            var Validate = _jwtCollection.Find( c => c.JwtToken == Jwt ).FirstOrDefault();
            if ( Validate != null)
            {
                if (reserveTable != null)
                {
                    _reservations.InsertOne(reserveTable);
                    return Ok(reserveTable);
                }
            }else
            {
                return Unauthorized();
            }
            return BadRequest("Data not found");
        }
    }
}
