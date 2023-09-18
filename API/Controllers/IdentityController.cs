using Internal.Contracts.DTOs.Identity;
using Internal.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _um;
        private readonly IConfiguration _config;

        public IdentityController(UserManager<User> um, IConfiguration config)
        {
            _um = um;
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _um.FindByNameAsync(login.Username);

            if (user == null || !await _um.CheckPasswordAsync(user, login.Password))
                throw new Exception("Netacni kredencijali.");

            return Ok(new IdentityResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Role = (await _um.GetRolesAsync(user))[0],
                Token = await GenerateToken(user)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var check = await _um.FindByEmailAsync(register.Email);
            if (check != null) throw new Exception("Vec postoji korisnik sa unetim mejlom.");

            var checkU = await _um.FindByNameAsync(register.Username);
            if (checkU != null) throw new Exception("Vec postoji korisnik sa unetim korisnickim imenom.");

            var user = new User
            {
                UserName = register.Username,
                Email = register.Email,
            };

            var result = await _um.CreateAsync(user, register.Password);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _um.AddToRoleAsync(user, "User");

            return Ok(new IdentityResponse
            {
                Username = user.UserName,
                Role = (await _um.GetRolesAsync(user))[0],
                Token = await GenerateToken(user),
                Id = user.Id
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _um.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}