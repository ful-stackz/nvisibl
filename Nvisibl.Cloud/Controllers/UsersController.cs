using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Business.Interfaces;
using Nvisibl.Cloud.Models.Users;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersManager _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersManager userManagerService, ILogger<UsersController> logger)
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
        public async Task<ActionResult> GetByIdAsync(
            int id,
            [FromQuery] bool includeFriends = false)
        {
            try
            {
                var user = await _userManager.GetUserAsync(id);
                if (user is null)
                {
                    return NotFound();
                }

                return new JsonResult(new
                {
                    user.Id,
                    user.Username,
                    Friends = includeFriends
                        ? await _userManager.GetUserFriendsAsync(id)
                        : Array.Empty<Business.Models.Users.UserModel>(),
                });
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
                return new JsonResult(await _userManager.GetUserFriendsAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not friends of user with id ({id}).");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateUserModel createUserModel)
        {
            try
            {
                return new JsonResult(
                    await _userManager.CreateUserAsync(
                        new Business.Models.Users.CreateUserModel
                        {
                            Username = createUserModel.Username,
                        }));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not create new user record. {createUserModel.Username}");
                return BadRequest();
            }
        }

        [HttpPost("{id}/friends")]
        public async Task<ActionResult> AddFriendAsync(
            int id,
            [FromBody] AddUserFriendModel addUserFriendModel)
        {
            try
            {
                await _userManager.AddUserFriendAsync(new Business.Models.Users.AddUserFriendModel
                {
                    FriendId = addUserFriendModel.UserId,
                    UserId = id,
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not add user ({addUserFriendModel.UserId}) as friend to user ({id}).");
                return BadRequest();
            }
        }
    }
}