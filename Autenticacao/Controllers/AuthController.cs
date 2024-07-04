using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Autenticacao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("/login")]
        public IActionResult Login(string username, string password)
        {
            if(IsValidUser(username, password))
            {
                var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new [] { 
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Role, "admin"),
                            new Claim(ClaimTypes.Role, "manager"),
                        },
                        BearerTokenDefaults.AuthenticationScheme
                    )
                );

                return SignIn(claimsPrincipal);
            }

            return Unauthorized();
        }

        private bool IsValidUser(string username, string password) => username == "user" && password == "1234";

        [HttpGet("/user")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUser()
        {
            var user = User;
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return Ok();
            }

            return Unauthorized();
        }
    }
}
