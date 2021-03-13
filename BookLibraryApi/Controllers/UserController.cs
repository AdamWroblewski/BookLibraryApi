using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookLibraryApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BookLibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterNewUser(UserModel model)
        {
            var user = new IdentityUser {UserName = model.UserName, Email = model.EmailAddress};
            IdentityResult result = null;
            try
            {
                result = await _userManager.CreateAsync(user, model.Password);
            }
            catch (SqlException exception)
            {
                _logger.LogCritical(exception.Message);
            }
            catch (DbUpdateException exception)
            {
                _logger.LogCritical(exception.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKey123secretKey123"));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {    
                new Claim(ClaimTypes.Name, model.UserName),
            };

            var options = new JwtSecurityToken(
                issuer: "https://localhost:44369",
                audience: "https://localhost:44369",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(options);

            return Ok(new {Token = token});
        }
    }
}