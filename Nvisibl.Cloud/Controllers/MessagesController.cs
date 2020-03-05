using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Business.Interfaces;
using System;
using System.Threading.Tasks;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;
using Nvisibl.Business.Models.Messages;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Nvisibl.Cloud.Authentication;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesManager _messagesManager;
        private readonly IUsersManager _usersManager;
        private readonly IChatroomsManager _chatroomsManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IMessagesManager messagesManager,
            IUsersManager usersManager,
            IChatroomsManager chatroomsManager,
            INotificationsService notificationsService,
            ILogger<MessagesController> logger)
        {
            _messagesManager = messagesManager ?? throw new ArgumentNullException(nameof(messagesManager));
            _usersManager = usersManager ?? throw new ArgumentNullException(nameof(usersManager));
            _chatroomsManager = chatroomsManager ?? throw new ArgumentNullException(nameof(chatroomsManager));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateMessageRequest request)
        {
            var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (subClaim is null ||
                !int.TryParse(subClaim.Value, out int idFromToken) ||
                request.AuthorId != idFromToken)
            {
                return Unauthorized();
            }

            try
            {
                var sender = await _usersManager.GetUserAsync(request.AuthorId);
                if (sender is null)
                {
                    return BadRequest($"User with id '{request.AuthorId}' does not exist.");
                }

                var senderChatrooms = await _chatroomsManager.GetUserChatroomsAsync(sender);
                if (senderChatrooms is null || !senderChatrooms.Any(x => x.Id == request.ChatroomId))
                {
                    return Unauthorized(
                        $"User with id '{request.AuthorId}' is not a member of chatroom with id " +
                        $"'${request.ChatroomId}'.");
                }

                var message = await _messagesManager.CreateMessageAsync(
                    new CreateMessageModel
                    {
                        AuthorId = request.AuthorId,
                        Body = request.Body,
                        ChatroomId = request.ChatroomId,
                        TimeSentUtc = request.TimeSentUtc,
                    });

                _notificationsService.EnqueueNotification(new ChatroomMessageNotification(message));

                return new JsonResult(new MessageResponse
                {
                    AuthorId = message.AuthorId,
                    Body = message.Body,
                    ChatroomId = message.ChatroomId,
                    Id = message.Id,
                    TimeSentUtc = message.TimeSentUtc.ToString("o"),
                });
            }
            catch (Exception ex) when (
                ex is ArgumentOutOfRangeException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                _logger.LogWarning(
                    exception: ex,
                    $"Could not send message from user with id '{request.AuthorId}' to chatroom with " +
                    $"id '{request.ChatroomId}'.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    exception: ex,
                    $"Could not send message from user with id '{request.AuthorId}' to chatroom with " +
                    $"id '{request.ChatroomId}'.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
