using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Business.Interfaces;
using Nvisibl.Business.Models.Users;
using Nvisibl.Cloud.Authentication;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;

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
                var users = await _userManager.GetUsersAsync(page, pageSize);
                return new JsonResult(
                    users.Select(user => new BasicUserResponse
                    {
                        Id = user.Id,
                        Username = user.Username,
                    }));
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

                var friends = includeFriends
                    ? (await _userManager.GetUserFriendsAsync(id)).Select(f => new BasicUserResponse
                    {
                        Id = f.Id,
                        Username = f.Username,
                    }).ToList()
                    : Enumerable.Empty<BasicUserResponse>().ToList();

                return new JsonResult(new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Friends = friends,
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
                var friends = await _userManager.GetUserFriendsAsync(id);
                return new JsonResult(friends.Select(user => new BasicUserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                }));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not friends of user with id ({id}).");
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin + "," + JwtSchemes.User)]
        [HttpGet("{id}/chatrooms")]
        public async Task<IActionResult> GetChatroomsAsync(
            int id,
            [FromServices] IChatroomsManager chatroomsManager)
        {
            try
            {
                var user = await _userManager.GetUserAsync(id);
                if (user is null)
                {
                    return NotFound();
                }

                var chatrooms = await chatroomsManager.GetUserChatroomsAsync(user);
                var responseChatrooms = new List<ChatroomResponse>();
                foreach (var chatroom in chatrooms)
                {
                    var chatroomUsers = (await chatroomsManager.GetChatroomUsersAsync(chatroom.Id))
                        .Select(user => new BasicUserResponse { Id = user.Id, Username = user.Username })
                        .ToList();
                    responseChatrooms.Add(new ChatroomResponse
                    {
                        Id = chatroom.Id,
                        Name = chatroom.Name,
                        Users = chatroomUsers,
                    });
                }

                return new JsonResult(new UserChatroomsResponse { Chatrooms = responseChatrooms });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve the chatrooms of user with id ({id}).");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateUserRequest request)
        {
            try
            {
                var user = await _userManager.CreateUserAsync(new CreateUserModel
                {
                    Username = request.Username,
                });
                return new JsonResult(new BasicUserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not create new user record. {request.Username}");
                return BadRequest();
            }
        }

        [HttpPost("{id}/friends")]
        public async Task<ActionResult> AddFriendAsync(
            int id,
            [FromBody] AddFriendRequest request)
        {
            try
            {
                await _userManager.AddUserFriendAsync(new AddUserFriendModel
                {
                    FriendId = request.UserId,
                    UserId = id,
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not add user ({request.UserId}) as friend to user ({id}).");
                return BadRequest();
            }
        }
    }
}