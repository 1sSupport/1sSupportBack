using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult CreateToken(string username = "admin", string password = "admin")
        {
            IActionResult response = Unauthorized();
            if (username.Equals(password))
            {
                var token = JwtTokenBuilder();
                response = Ok(new { access_token = token });
            }
            return response;
        }

        private string JwtTokenBuilder()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credantials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _configuration["JWT:issuer"], audience: _configuration["JWT:audience"], signingCredentials: credantials, expires: DateTime.Now.AddDays(3));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}