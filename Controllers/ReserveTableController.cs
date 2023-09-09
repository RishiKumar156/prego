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
        public  IActionResult CreateReservation(ReservationTableDTO reserveTable)
        {
            var Validate = _jwtCollection.Find( c => c.JwtToken == reserveTable.Jwt ).FirstOrDefault();
            if ( Validate != null)
            {
                var MakeReservation = new ReserveTable
                {
                    Jwt = reserveTable.Jwt,
                    OrderDesc = reserveTable.OrderDesc,
                    OrderName = reserveTable.OrderName,
                    PhoneNo = reserveTable.PhoneNo,
                };
                _reservations.InsertOne(MakeReservation);
                return Ok(reserveTable);
            }
            return Unauthorized();
        }
    }
}
