using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
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
        private readonly IUsersManager _usersManager;
        private readonly IChatroomsManager _chatroomsManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUsersManager usersManager,
            IChatroomsManager chatroomsManager,
            ILogger<UsersController> logger)
        {
            _usersManager = usersManager ?? throw new ArgumentNullException(nameof(usersManager));
            _chatroomsManager = chatroomsManager ?? throw new ArgumentNullException(nameof(chatroomsManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? data = null)
        {
            const string IncludeChatroomsKey = "CHATROOMS";
            const string IncludeFriendsKey = "FRIENDS";
            const string IncludeFriendRequestsKey = "FRIEND-REQUESTS";
            string optionalData = data?.ToUpperInvariant() ?? string.Empty;

            try
            {
                var users = await _usersManager.GetUsersAsync(page, pageSize);
                var response = users
                    .Select(user => new UserResponse
                    {
                        Id = user.Id,
                        Username = user.Username,
                    })
                    .ToList();

                if (optionalData.Contains(IncludeChatroomsKey))
                {
                    foreach (var userResponse in response)
                    {
                        userResponse.Chatrooms = (await _chatroomsManager
                            .GetUserChatroomsAsync(users.First(user => user.Id == userResponse.Id)))
                            .Select(chatroom => new BasicChatroomResponse
                            {
                                Id = chatroom.Id,
                                IsShared = chatroom.IsShared,
                                Name = chatroom.Name,
                            })
                            .ToList();
                    }
                }

                if (optionalData.Contains(IncludeFriendsKey))
                {
                    foreach (var userResponse in response)
                    {
                        userResponse.Friends = (await _usersManager.GetUserFriendsAsync(userResponse.Id))
                            .Select(friend => new BasicUserResponse
                            {
                                Id = friend.Id,
                                Username = friend.Username,
                            })
                            .ToList();
                    }
                }

                if (optionalData.Contains(IncludeFriendRequestsKey))
                {
                    foreach (var userResponse in response)
                    {
                        userResponse.FriendRequests = new List<FriendRequestResponse>();
                        var friendRequests = await _usersManager.GetUserFriendRequestsAsync(userResponse.Id);
                        foreach (var friendRequest in friendRequests)
                        {
                            var sender = await _usersManager.GetUserAsync(friendRequest.User1Id);
                            var receiver = await _usersManager.GetUserAsync(friendRequest.User2Id);
                            userResponse.FriendRequests.Add(new FriendRequestResponse
                            {
                                Accepted = friendRequest.Accepted,
                                Id = friendRequest.Id,
                                Receiver = new BasicUserResponse { Id = receiver.Id, Username = receiver.Username },
                                Sender = new BasicUserResponse { Id = sender.Id, Username = sender.Username, },
                            });
                        }
                    }
                }

                return new JsonResult(response);
            }
            catch (Exception ex) when (
                ex is ArgumentNullException ||
                ex is ArgumentOutOfRangeException)
            {
                _logger.LogWarning(
                    exception: ex,
                    $"Could not retrieve users. {nameof(page)} = {page}; {nameof(pageSize)} = {pageSize}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    exception: ex,
                    $"Could not retrieve users. {nameof(page)} = {page}; {nameof(pageSize)} = {pageSize}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(
            int id,
            [FromQuery] string? data = null)
        {
            const string IncludeChatroomsKey = "CHATROOMS";
            const string IncludeFriendsKey = "FRIENDS";
            const string IncludeFriendRequestsKey = "FRIEND-REQUESTS";
            string optionalData = data?.ToUpperInvariant() ?? string.Empty;

            var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (subClaim is null || !int.TryParse(subClaim.Value, out int idFromToken) || idFromToken != id)
            {
                return Unauthorized();
            }

            try
            {
                var user = await _usersManager.GetUserAsync(id);
                if (user is null)
                {
                    return NotFound();
                }

                var response = new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                };

                if (optionalData.Contains(IncludeChatroomsKey))
                {
                    response.Chatrooms = (await _chatroomsManager
                        .GetUserChatroomsAsync(user))
                        .Select(chatroom => new BasicChatroomResponse
                        {
                            Id = chatroom.Id,
                            IsShared = chatroom.IsShared,
                            Name = chatroom.Name,
                        })
                        .ToList();
                }

                if (optionalData.Contains(IncludeFriendsKey))
                {
                    response.Friends = (await _usersManager.GetUserFriendsAsync(user.Id))
                        .Select(friend => new BasicUserResponse
                        {
                            Id = friend.Id,
                            Username = friend.Username,
                        })
                        .ToList();
                }

                if (optionalData.Contains(IncludeFriendRequestsKey))
                {
                    response.FriendRequests = new List<FriendRequestResponse>();
                    var friendRequests = await _usersManager.GetUserFriendRequestsAsync(user.Id);
                    foreach (var friendRequest in friendRequests)
                    {
                        var sender = await _usersManager.GetUserAsync(friendRequest.User1Id);
                        var receiver = await _usersManager.GetUserAsync(friendRequest.User2Id);
                        response.FriendRequests.Add(new FriendRequestResponse
                        {
                            Accepted = friendRequest.Accepted,
                            Id = friendRequest.Id,
                            Receiver = new BasicUserResponse { Id = receiver.Id, Username = receiver.Username },
                            Sender = new BasicUserResponse { Id = sender.Id, Username = sender.Username, },
                        });
                    }
                }

                return new JsonResult(response);
            }
            catch (Exception ex) when (
                ex is ArgumentNullException ||
                ex is ArgumentOutOfRangeException)
            {
                _logger.LogWarning(exception: ex, $"Could not retrieve the details of user with id {id}.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not retrieve the details of user with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateUserRequest request)
        {
            try
            {
                var user = await _usersManager.CreateUserAsync(new CreateUserModel
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
                _logger.LogError(ex, $"Could not create new user record. {request.Username}");
                return BadRequest();
            }
        }
    }
}