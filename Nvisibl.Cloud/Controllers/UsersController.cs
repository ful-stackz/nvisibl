using System;
using System.Threading.Tasks;
using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Services.Interfaces;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagerService _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserManagerService userManagerService, ILogger<UsersController> logger)
        {
            _userManager = userManagerService ?? throw new ArgumentNullException(nameof(userManagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return new JsonResult(await _userManager.GetUsersAsync(page, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    exception: ex,
                    $"Could not retrieve users. {nameof(page)} = {page}; {nameof(pageSize)} = {pageSize}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
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
        public async Task<ActionResult> GetFriendsAsync(int id)
        {
            try
            {
                return new JsonResult(await _userManager.GetFriendsAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not friends of user with id ({id}).");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(
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
        public async Task<ActionResult> AddFriendAsync(
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