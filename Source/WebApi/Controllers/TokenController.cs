using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
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
        private readonly IEmailService _emailService;

        public TokenController(IConfiguration config, IEmailService emailService)
        {
            _configuration = config;
            _emailService = emailService;
        }

        [HttpGet()]
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

        //[HttpGet("{email}")]
        //public async Task<IActionResult> SendEmail(string email)
        //{
        //    //var list = new List<string>() { "stepmolostov@gmail.com", "dartweder7@gmail.com", "krumih@mail.ru", "Krentorr@gmail.com", "ibigcall@gmail.com", "mkruglov239@gmail.com", "vilkovaliza@gmail.com", "gteliaeva@gmail.com", "Bigcall9287006@gmail.com", govjadkoilja@yandex.ru };

        //    //foreach (var em in list)
        //    //{
        //    //    await _emailService.SendAsync(em, "Возникли проблемы с вашим заказом!", @"Необходимо подтвердить корректность выполнения на сайте https://trello.com/b/keauAEbE/%D0%B7%D0%B0%D0%B4%D0%B0%D1%87%D0%B8");
        //    //}

        //    //return Ok();
        //}

        private string JwtTokenBuilder()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credantials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _configuration["JWT:issuer"], audience: _configuration["JWT:audience"], signingCredentials: credantials, expires: DateTime.Now.AddDays(3));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}