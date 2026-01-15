using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Models.DTOs;
using NZwalks.API.Repository;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ItokenRepository tokenrepository;

        public AuthController(UserManager<IdentityUser> userManager, ItokenRepository tokenrepository)
        {
            this.userManager = userManager;
            this.tokenrepository = tokenrepository;
        }

        //api/auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
            {
                var roleResult = await userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
            }

            return Ok("User registered successfully.");
        }

        //api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            // 1. Find the user by Email (Assuming loginRequestDTO.UserName contains the email)
            var user = await userManager.FindByEmailAsync(loginRequestDTO.UserName);

            if (user != null)
            {
                // 2. Validate the password
                var checkedPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (checkedPasswordResult)
                {
                    // 3. Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // 4. Create Token using the repository
                        var jwtToken = tokenrepository.CreateJWTToken(user, roles.ToList());

                        // 5. Return the token in the response
                        return Ok(new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        });
                    }
                }
            }

            return BadRequest("UserName or Password incorrect");
        }
    }
}