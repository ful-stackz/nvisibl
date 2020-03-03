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

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesManager _messagesManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IMessagesManager messagesManagerService,
            INotificationsService notificationsService,
            ILogger<MessagesController> logger)
        {
            _messagesManager = messagesManagerService ?? throw new ArgumentNullException(nameof(messagesManagerService));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin + "," + JwtSchemes.User)]
        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateMessageRequest request,
            [FromServices] IUsersManager usersManager,
            [FromServices] IChatroomsManager chatroomsManager)
        {
            try
            {
                var sender = await usersManager.GetUserAsync(request.AuthorId);
                if (sender is null)
                {
                    return BadRequest($"User with ID '{request.AuthorId}' does not exist.");
                }

                var senderChatrooms = await chatroomsManager.GetUserChatroomsAsync(sender);
                if (senderChatrooms is null || !senderChatrooms.Any(x => x.Id == request.ChatroomId))
                {
                    return Unauthorized(
                        $"User with ID '{request.AuthorId}' is not a member of chatroom with ID " +
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
                    TimeSentUtc = message.TimeSentUtc,
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Could add message from user with id ({request.AuthorId}) to chatroom with " +
                    $"id ({request.ChatroomId}).");
                return BadRequest();
            }
        }
    }
}
