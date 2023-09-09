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
        private readonly IMongoCollection<JwtCollection> _JwtCollection;
        public GoogleController(IMongoClient mongoClient )
        {
            var database = mongoClient.GetDatabase("pregopantry");
            _GoogleRegisterCcollection = database.GetCollection<GoogleRegister>("googlelogin");
            _JwtCollection = database.GetCollection<JwtCollection>("JwtTokens");
        }

        [HttpPost("newuser")]
        public IActionResult RegisterUser(GoogleRegister googleRegister)
        {
            var UserExists = _GoogleRegisterCcollection.Find(c => c.GmailId == googleRegister.GmailId).FirstOrDefault();
            if(UserExists == null)
            {
                _GoogleRegisterCcollection.InsertOne(googleRegister);
                var JWT = CreateToken(googleRegister);
                var AddJwt = new JwtCollection
                {
                    Username = googleRegister.Gusername,
                    JwtToken = JWT
                };
                _JwtCollection.InsertOne(AddJwt);
                return Ok(AddJwt);
            }
            return BadRequest();
        }

        [HttpPost("Glogin")]
        public IActionResult LoginUser(GoogleRegister googleRegister)
        {
            var FindUser = _GoogleRegisterCcollection.Find( c => c.GmailId == googleRegister.GmailId).FirstOrDefault();
            if(FindUser != null)
            {
                var Jwt = _JwtCollection.Find(c => c.Username == FindUser.Gusername).FirstOrDefault();
                var LoginCredentials = new GoogleLoginDTO
                {
                    Gid = FindUser.Gid,
                    GmailId = FindUser.GmailId,
                    Gusername = FindUser.Gusername,
                    Id = FindUser.Id,
                    Picture = FindUser.Picture,
                    Jwt = Jwt.JwtToken
                };
                return Ok(LoginCredentials);
            }
            return BadRequest("User Not Found Register First");
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
