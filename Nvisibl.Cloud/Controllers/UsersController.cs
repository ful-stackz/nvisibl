using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Nvisibl.Business.Interfaces;
using Nvisibl.Business.Models.Users;
using Nvisibl.Cloud.Authentication;
using Nvisibl.Cloud.Models.Data;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;
using Nvisibl.Cloud.Services.Interfaces;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersManager _userManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUsersManager userManagerService,
            INotificationsService notificationsService,
            ILogger<UsersController> logger)
        {
            _userManager = userManagerService ?? throw new ArgumentNullException(nameof(userManagerService));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
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

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpGet("find")]
        public async Task<IActionResult> FindUserAsync(
            [FromQuery] string username)
        {
            try
            {
                var user = await _userManager.GetUserAsync(username);
                return user is null
                    ? (IActionResult)NotFound()
                    : new JsonResult(new BasicUserResponse { Id = user.Id, Username = user.Username });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not find user with username {username}.");
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

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpGet("{id}/friend-requests")]
        public async Task<IActionResult> GetFriendRequestsAsync(int id)
        {
            try
            {
                var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
                if (subClaim is null ||
                    !int.TryParse(subClaim.Value, out int idFromToken) ||
                    idFromToken != id)
                {
                    return Unauthorized();
                }

                var friendRequests = await _userManager.GetUserFriendRequestsAsync(id);
                var friendRequestsResponses = new List<FriendRequestResponse>();
                foreach (var friendRequest in friendRequests)
                {
                    var sender = await _userManager.GetUserAsync(friendRequest.User1Id);
                    var receiver = await _userManager.GetUserAsync(friendRequest.User2Id);
                    friendRequestsResponses.Add(new FriendRequestResponse
                    {
                        Id = friendRequest.Id,
                        Accepted = friendRequest.Accepted,
                        Sender = new BasicUserResponse { Id = sender.Id, Username = sender.Username },
                        Receiver = new BasicUserResponse { Id = receiver.Id, Username = receiver.Username },
                    });
                }
                return new JsonResult(friendRequestsResponses);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve friend requests for user with id ({id}).");
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

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin)]
        [HttpPost("{id}/friends")]
        public async Task<ActionResult> AddFriendAsync(
            int id,
            [FromBody] AddFriendRequest request)
        {
            try
            {
                var friend = await _userManager.AddUserFriendAsync(new AddUserFriendModel
                {
                    FriendId = request.UserId,
                    UserId = id,
                });
                return new JsonResult(new FriendRequestResponse
                {
                    Accepted = friend.Accepted,
                    Id = friend.Id,
                    Receiver = new BasicUserResponse { Id = friend.User2Id },
                    Sender = new BasicUserResponse { Id = friend.User1Id },
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not add user ({request.UserId}) as friend to user ({id}).");
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost("{id}/friend-requests")]
        public async Task<IActionResult> CreateFriendRequestAsync(
            int id,
            [FromBody] CreateFriendRequestRequest request)
        {
            try
            {
                var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
                if (subClaim is null ||
                    !int.TryParse(subClaim.Value, out int idFromToken) ||
                    request.SenderId != idFromToken)
                {
                    return Unauthorized();
                }

                var friendRequest = await _userManager.CreateFriendRequestAsync(new AddUserFriendModel
                {
                    UserId = request.SenderId,
                    FriendId = id,
                });
                var sender = await _userManager.GetUserAsync(request.SenderId);
                var receiver = await _userManager.GetUserAsync(id);

                _notificationsService.EnqueueNotification(new FriendRequestNotification(
                    friendRequestId: friendRequest.Id,
                    accepted: friendRequest.Accepted,
                    sender: sender,
                    receiver: receiver));
                return new JsonResult(new FriendRequestResponse
                {
                    Accepted = friendRequest.Accepted,
                    Id = friendRequest.Id,
                    Sender = new BasicUserResponse { Id = sender.Id, Username = sender.Username },
                    Receiver = new BasicUserResponse { Id = receiver.Id, Username = receiver.Username },
                });
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not send friend request to user with id ({id}).");
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost("{id}/friend-requests/{friendRequestId}")]
        public async Task<IActionResult> AnswerFriendRequestAsync(
            int id,
            int friendRequestId,
            [FromBody] FriendRequestAnswerRequest request)
        {
            try
            {
                var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
                if (subClaim is null ||
                    !int.TryParse(subClaim.Value, out int idFromToken) ||
                    idFromToken != id)
                {
                    return Unauthorized();
                }

                var friendRequest = await _userManager.GetFriendRequestAsync(friendRequestId);
                if (friendRequest is null)
                {
                    return NotFound();
                }
                else if (friendRequest.User2Id != id)
                {
                    // User1 sends the friend request, User2 answers to it
                    return Unauthorized();
                }

                if (request.Accept)
                {
                    await _userManager.AcceptFriendRequestAsync(friendRequestId);
                }
                else
                {
                    await _userManager.RejectFriendRequestAsync(friendRequestId);
                }

                _notificationsService.EnqueueNotification(new FriendRequestNotification(
                    friendRequestId: friendRequest.Id,
                    accepted: request.Accept,
                    sender: await _userManager.GetUserAsync(friendRequest.User1Id),
                    receiver: await _userManager.GetUserAsync(friendRequest.User2Id)));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not answer friend request with id ({friendRequestId})");
                return BadRequest();
            }
        }
    }
}