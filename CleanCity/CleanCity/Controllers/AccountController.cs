using CleanCity.Data;
using CleanCity.Models.ViewModels;
using CleanCity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanCity.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration) {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginVM model) {
            model.Email = model.Email.ToLower().Trim();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null) {
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                    return BadRequest("Wrong password");
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles) {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = userRoles.Last()
                });
            }
            return Unauthorized($"User '{model.Email}' hasn't found");
        }

        [HttpPost("/RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterVM model) {
            return await Register(model, RoleService.UserRole);
        }

        [HttpPost("/RegisterAdmin")]
        [Authorize(Roles = RoleService.AdminRole)]
        public async Task<IActionResult> RegisterAdmin(RegisterVM model) {
            return await Register(model, RoleService.AdminRole);
        }

        private async Task<IActionResult> Register(RegisterVM model, string role){
            model.Email = model.Email.ToLower().Trim();
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseVM { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new() {
                UserName = model.Nickname,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
            else
                await _userManager.AddToRoleAsync(user, role);
            return Ok(new ResponseVM { Status = "Success", Message = "User created successfully!" });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims) {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
