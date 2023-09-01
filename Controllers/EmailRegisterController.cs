using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailRegisterController : ControllerBase
    {
        private readonly IMongoCollection<Email> _mongoCollection;
        
        public EmailRegisterController(IMongoClient mongoClient)
        {
            var Database = mongoClient.GetDatabase("pregopantry");
            _mongoCollection = Database.GetCollection<Email>("emailregister");
        }

        [HttpPost("EmailRegister")]
        public IActionResult RegisterUser(EmailDTO email)
        {
            var Haspassword = BCrypt.Net.BCrypt.HashPassword(email.Password);
            var Register = new Email();
            Register.UserName = email.UserName;
            Register.Password = Haspassword;
            if(Register == null)
            {
                return BadRequest();
            }
            _mongoCollection.InsertOne(Register);
            return Ok(Register);
        }

        [HttpPost("EmailLogin")]
        public IActionResult LoginUser(EmailDTO email)
        {
            var user = _mongoCollection.Find(c => c.UserName == email.UserName).FirstOrDefault();
            if(!BCrypt.Net.BCrypt.Verify(email.Password , user.Password))
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
