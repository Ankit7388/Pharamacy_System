using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace YourApp.Controllers.Api
{
    
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
        private readonly IConfiguration _config;
        private readonly WebApiContext _dbContext;

        public UserController(IConfiguration config,WebApiContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string pass = _passwordHasher.HashPassword(null, model.Password);
            var user = new Users
            {
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                Password = pass
            };

            var result = await _dbContext.Users.AddAsync(user);

            if (result.State != EntityState.Added)
            {
                ModelState.AddModelError(string.Empty, "Unable to register user.");
                return BadRequest(ModelState);
            }
            _dbContext.SaveChanges();
            

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == model.UserEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
            var result = _passwordHasher.VerifyHashedPassword(null, user.Password, model.Password);
            bool req = false;
            if (result == PasswordVerificationResult.Success)
            {
                req = true;
            }

            if (!await _dbContext.Users.AnyAsync(u => u.UserEmail == model.UserEmail && req))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Users user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id",user.UserId.ToString()),
                new Claim("UserName",user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim("Password",user.Password)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

