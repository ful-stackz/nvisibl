using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nvisibl.Business.Interfaces;
using Nvisibl.Business.Models.Users;
using Nvisibl.Cloud.Authentication;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _identityUserManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsersManager _chatUserManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<IdentityUser> identityUserManager,
            SignInManager<IdentityUser> signInManager,
            IUsersManager chatUserManager,
            ILogger<AuthController> logger)
        {
            _identityUserManager = identityUserManager ?? throw new ArgumentNullException(nameof(identityUserManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _chatUserManager = chatUserManager ?? throw new ArgumentNullException(nameof(chatUserManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] AuthRegisterRequest request)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = request.Username,
                };

                var identityRegistration = await _identityUserManager.CreateAsync(identityUser, request.Password);
                if (!identityRegistration.Succeeded)
                {
                    return BadRequest(identityRegistration.Errors.Select(e => e.Description));
                }

                var chatUser = await _chatUserManager.CreateUserAsync(new CreateUserModel
                {
                    Username = request.Username,
                });
                if (chatUser is null)
                {
                    await _identityUserManager.DeleteAsync(identityUser);
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration failed.");
                return BadRequest("Could not complete registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] AuthLoginRequest request,
            [FromServices] JwtConfiguration jwtConfiguration)
        {
            try
            {
                var identityUser = await _identityUserManager.FindByNameAsync(request.Username);
                if (identityUser is null)
                {
                    return NotFound();
                }

                var signInResult = await _signInManager.PasswordSignInAsync(
                    identityUser,
                    request.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);
                if (!signInResult.Succeeded)
                {
                    return BadRequest();
                }

                var chatUser = await _chatUserManager.GetUserAsync(request.Username);

                var jwtConfig = jwtConfiguration.GetSchemeConfig(JwtSchemes.User);
                var jwtClaims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                };
                var signingCredentials = new SigningCredentials(
                    key: jwtConfig.SecurityKey,
                    algorithm: JwtConfiguration.SecurityAlgorithm
                );
                var createdAt = DateTime.UtcNow;
                var validBefore = createdAt.AddMinutes(5);
                var token = new JwtSecurityToken(
                    issuer: jwtConfig.Issuer,
                    audience: jwtConfig.Audience,
                    claims: jwtClaims,
                    notBefore: createdAt,
                    expires: validBefore,
                    signingCredentials: signingCredentials);

                return new JsonResult(new AuthLoginResponse(
                    userId: chatUser!.Id,
                    auth: new AuthTokenResponse(
                        accessToken: new JwtSecurityTokenHandler().WriteToken(token),
                        createdAt: createdAt.ToString("o"),
                        validBefore: validBefore.ToString("o"))));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Login failed.");
                return BadRequest("Could not complete login.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost("renew-token")]
        public IActionResult RenewToken(
            [FromBody] AuthRenewTokenRequest request,
            [FromServices] JwtConfiguration jwtConfiguration)
        {
            try
            {
                var jwtConfig = jwtConfiguration.GetSchemeConfig(JwtSchemes.User);
                var jwtClaims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, request.Username),
                };
                var signingCredentials = new SigningCredentials(
                    key: jwtConfig.SecurityKey,
                    algorithm: JwtConfiguration.SecurityAlgorithm
                );
                var createdAt = DateTime.UtcNow;
                var validBefore = createdAt.AddMinutes(30);
                var token = new JwtSecurityToken(
                    issuer: jwtConfig.Issuer,
                    audience: jwtConfig.Audience,
                    claims: jwtClaims,
                    notBefore: createdAt,
                    expires: validBefore,
                    signingCredentials: signingCredentials);

                return new JsonResult(new AuthTokenResponse(
                    accessToken: new JwtSecurityTokenHandler().WriteToken(token),
                    createdAt: createdAt.ToString("o"),
                    validBefore: validBefore.ToString("o")));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not issue a renewed token.");
                return BadRequest();
            }
        }
    }
}
