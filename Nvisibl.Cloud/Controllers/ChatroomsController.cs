using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Business.Interfaces;
using Nvisibl.Business.Models.Chatrooms;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Nvisibl.Cloud.Authentication;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.Cloud.Models.Data;
using Microsoft.AspNetCore.Http;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomsController : ControllerBase
    {
        private readonly IChatroomsManager _chatroomsManager;
        private readonly IMessagesManager _messagesManager;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<ChatroomsController> _logger;

        public ChatroomsController(
            IChatroomsManager chatroomsManager,
            IMessagesManager messagesManager,
            INotificationsService notificationsService,
            ILogger<ChatroomsController> logger)
        {
            _chatroomsManager = chatroomsManager ?? throw new ArgumentNullException(nameof(chatroomsManager));
            _messagesManager = messagesManager ?? throw new ArgumentNullException(nameof(messagesManager));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin)]
        [HttpGet]
        public async Task<ActionResult> GetAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? data = null)
        {
            const string IncludeUsersKey = "USERS";
            string optionalData = data?.ToUpperInvariant() ?? string.Empty;

            try
            {
                var chatrooms = await _chatroomsManager.GetChatroomsAsync(page, pageSize);
                var response = chatrooms
                    .Select(chatroom => new ChatroomResponse
                    {
                        Id = chatroom.Id,
                        IsShared = chatroom.IsShared,
                        Name = chatroom.Name,
                    })
                    .ToList();

                if (optionalData.Contains(IncludeUsersKey))
                {
                    foreach (var chatroomResponse in response)
                    {
                        chatroomResponse.Users = (await _chatroomsManager
                            .GetChatroomUsersAsync(chatroomResponse.Id))
                            .Select(user => new BasicUserResponse
                            {
                                Id = user.Id,
                                Username = user.Username,
                            })
                            .ToList();
                    }
                }

                return new JsonResult(response);
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                _logger.LogWarning(ex, "Could not retrieve chatrooms with details.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not retrieve chatrooms with details.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.User)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(
            int id,
            [FromQuery] string? data = null,
            [FromQuery] DateTime? messagesOlderThan = null,
            [FromQuery] int messagesCount = 20)
        {
            const string IncludeMessagesKey = "MESSAGES";
            const string IncludeUsersKey = "USERS";
            string optionalData = data?.ToUpperInvariant() ?? string.Empty;

            try
            {
                var chatroom = await _chatroomsManager.GetChatroomAsync(id);
                var response = new ChatroomResponse
                {
                    Id = chatroom.Id,
                    IsShared = chatroom.IsShared,
                    Name = chatroom.Name,
                };

                if (optionalData.Contains(IncludeMessagesKey))
                {
                    response.Messages = (await _messagesManager
                        .GetChatroomMessagesAsync(chatroom.Id, messagesOlderThan ?? DateTime.UtcNow, messagesCount))
                        .Select(message => new MessageResponse
                        {
                            AuthorId = message.AuthorId,
                            Body = message.Body,
                            ChatroomId = message.ChatroomId,
                            Id = message.Id,
                            TimeSentUtc = message.TimeSentUtc.ToString("o"),
                        })
                        .ToList();
                }

                if (optionalData.Contains(IncludeUsersKey))
                {
                    response.Users = (await _chatroomsManager
                        .GetChatroomUsersAsync(chatroom.Id))
                        .Select(user => new BasicUserResponse
                        {
                            Id = user.Id,
                            Username = user.Username,
                        })
                        .ToList();
                }

                return new JsonResult(response);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                _logger.LogWarning(ex, $"Could not retrieve the details of chatroom with id '{id}'.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not retrieve the details of chatroom with id '{id}'.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin + "," + JwtSchemes.User)]
        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateChatroomRequest request)
        {
            try
            {
                var chatroom = await _chatroomsManager.CreateChatroomAsync(new CreateChatroomModel
                {
                    ChatroomName = request.ChatroomName ?? string.Empty,
                    OwnerId = request.OwnerId,
                    IsShared = request.IsShared,
                });
                if (chatroom is null)
                {
                    return BadRequest();
                }

                _notificationsService.EnqueueNotification(new ChatroomChangedNotification(chatroom));

                return new JsonResult(new BasicChatroomResponse
                {
                    Id = chatroom.Id,
                    Name = chatroom.Name,
                    IsShared = chatroom.IsShared,
                });
            }
            catch (Exception ex) when (
                ex is ArgumentOutOfRangeException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                _logger.LogWarning(ex, "Could not create chatroom.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create chatroom.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/users")]
        public async Task<ActionResult> AddUserAsync(
            int id,
            [FromBody] AddUserToChatroomRequest request)
        {
            try
            {
                var chatroom = await _chatroomsManager.GetChatroomAsync(id);
                if (chatroom is null)
                {
                    return NotFound();
                }

                var users = await _chatroomsManager.GetChatroomUsersAsync(id);
                if (!chatroom.IsShared && users.Count() == 2)
                {
                    return BadRequest("Chatroom is at full capacity.");
                }

                await _chatroomsManager.AddUserToChatroomAsync(new AddUserToChatroomModel
                {
                    ChatroomId = id,
                    UserId = request.UserId,
                });

                _notificationsService.EnqueueNotification(
                    new ChatroomChangedNotification(
                        chatroom: await _chatroomsManager.GetChatroomAsync(id)));

                return Ok();
            }
            catch (Exception ex) when (
                ex is ArgumentOutOfRangeException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                _logger.LogWarning(
                    ex,
                    $"Could not add user with id '{request.UserId}' to chatroom with id '{id}'.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not add user to chatroom.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin)]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _chatroomsManager.DeleteChatroomAsync(id);
                return Ok();
            }
            catch (Exception ex) when (
                ex is ArgumentOutOfRangeException ||
                ex is ArgumentNullException ||
                ex is InvalidOperationException)
            {
                _logger.LogWarning(ex, $"Could not delete chatroom with id '{id}'.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not delete chatroom with id '{id}'.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
