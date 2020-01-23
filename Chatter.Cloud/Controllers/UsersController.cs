using System;
using System.Threading.Tasks;
using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManagerService _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManagerService userManagerService, ILogger<UsersController> logger)
        {
            _userManager = userManagerService ?? throw new ArgumentNullException(nameof(userManagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return new JsonResult(await _userManager.GetUsersAsync(offset, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    exception: ex,
                    $"Could not retrieve users. {nameof(offset)} = {offset}; {nameof(pageSize)} = {pageSize}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserAsync(int id)
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(id);
                return user is { }
                    ? new JsonResult(user)
                    : (ActionResult)NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve user with id {id}.");
                return BadRequest();
            }
        }

        [HttpGet("{id}/friends")]
        public async Task<ActionResult> GetUserFriendsAsync(int id)
        {
            try
            {
                return new JsonResult(await _userManager.GetUserWithFriendsByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve user with id ({id}) and friend list.");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(
            [FromBody] CreateUserModel userRequest)
        {
            try
            {
                return new JsonResult(await _userManager.CreateUserAsync(userRequest));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not create new user record. {userRequest.Username}");
                return BadRequest();
            }
        }

        [HttpPost("{id}/friends")]
        public async Task<ActionResult> AddUserFriendAsync(
            int id,
            [FromBody] UserModel friend)
        {
            try
            {
                await _userManager.MakeFriendsAsync(id, friend);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not add user ({friend.Id}) as friend to user ({id}).");
                return BadRequest();
            }
        }
    }
}