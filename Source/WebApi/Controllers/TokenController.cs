using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet("{inn:maxlength(12):minlength(12)?}/{login?}")]
        [ProducesResponseType(404)]
        public IActionResult CreateToken(string inn, string login = "")
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login))
            {
                return NoContent();
            }

            IActionResult response = Unauthorized();
            if (inn.Equals(inn))
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