using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
using System;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/friend-requests")]
    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IUsersManager _usersManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<FriendRequestsController> _logger;

        public FriendRequestsController(
            IUsersManager usersManager,
            INotificationsService notificationsService,
            ILogger<FriendRequestsController> logger)
        {
            _usersManager = usersManager ?? throw new ArgumentNullException(nameof(usersManager));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost]
        public async Task<IActionResult> CreateFriendRequestAsync(
            [FromBody] CreateFriendRequestRequest request)
        {
            var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (subClaim is null ||
                !int.TryParse(subClaim.Value, out int idFromToken) ||
                request.SenderId != idFromToken)
            {
                return Unauthorized();
            }

            try
            {
                var friendRequest = await _usersManager.CreateFriendRequestAsync(new AddUserFriendModel
                {
                    UserId = request.SenderId,
                    FriendId = request.ReceiverId,
                });

                var sender = await _usersManager.GetUserAsync(request.SenderId);
                var receiver = await _usersManager.GetUserAsync(request.ReceiverId);

                _notificationsService.EnqueueNotification(
                    new FriendRequestNotification(friendRequest, sender, receiver));

                return new JsonResult(new FriendRequestResponse
                {
                    Accepted = friendRequest.Accepted,
                    Id = friendRequest.Id,
                    Sender = new BasicUserResponse { Id = sender.Id, Username = sender.Username },
                    Receiver = new BasicUserResponse { Id = receiver.Id, Username = receiver.Username },
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "A conflict occurred while trying to send a friend request.");
                return Conflict();
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                _logger.LogWarning(ex, "Could not send friend request.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Could not send a friend request from user with id '{request.SenderId}' to user with id " +
                    $"'{request.ReceiverId}'.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost("{id}")]
        public async Task<IActionResult> AnswerFriendRequestAsync(
            int id,
            [FromBody] FriendRequestAnswerRequest request)
        {
            var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (subClaim is null || !int.TryParse(subClaim.Value, out int idFromToken) || idFromToken != id)
            {
                return Unauthorized();
            }

            try
            {
                var friendRequest = await _usersManager.GetFriendRequestAsync(id);
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
                    await _usersManager.AcceptFriendRequestAsync(id);
                }
                else
                {
                    await _usersManager.RejectFriendRequestAsync(id);
                }

                _notificationsService.EnqueueNotification(
                    new FriendRequestNotification(
                        friendRequest,
                        sender: await _usersManager.GetUserAsync(friendRequest.User1Id),
                        receiver: await _usersManager.GetUserAsync(friendRequest.User2Id)));

                return Ok();
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                _logger.LogWarning(ex, $"Could not answer friend request with id '{id}'");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not answer friend request with id '{id}'.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
