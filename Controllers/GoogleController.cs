using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IMongoCollection<GoogleRegister> _GoogleRegisterCcollection;
        public GoogleController(IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase("pregopantry");
            _GoogleRegisterCcollection = database.GetCollection<GoogleRegister>("googlelogin");
        }

        [HttpPost("newuser")]
        public IActionResult RegisterUser(GoogleRegister googleRegister)
        {
            var UserExists = _GoogleRegisterCcollection.Find(c => c.GmailId == googleRegister.GmailId).FirstOrDefault();
            if(UserExists != null)
            {
                var JWT = CreateToken(UserExists);
                UserExists.Jwt = JWT;
                _GoogleRegisterCcollection.InsertOne(googleRegister);
                return Ok(UserExists);
            }
            return Unauthorized("Sing-up failed");
        }

        private string CreateToken(GoogleRegister userRegister)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , userRegister.Gusername)
            };

            var key = new Byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            var symmetrickey = new SymmetricSecurityKey(key);
            var cred = new SigningCredentials(symmetrickey, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
