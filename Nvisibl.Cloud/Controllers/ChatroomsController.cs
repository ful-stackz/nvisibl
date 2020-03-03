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

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomsController : ControllerBase
    {
        private readonly IChatroomsManager _chatroomManagerService;
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<ChatroomsController> _logger;

        public ChatroomsController(
            IChatroomsManager chatroomManagerService,
            INotificationsService notificationsService,
            ILogger<ChatroomsController> logger)
        {
            _chatroomManagerService = chatroomManagerService ?? throw new ArgumentNullException(nameof(chatroomManagerService));
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
                var chatrooms = await _chatroomManagerService.GetChatroomsAsync(page, pageSize);
                return new JsonResult(chatrooms.Select(cr => new BasicChatroomResponse
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    IsShared = cr.IsShared,
                }));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Could not retrieve chatrooms. {nameof(page)} = {page}; {nameof(pageSize)} = {pageSize}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(
            int id,
            [FromQuery] bool includeUsers = false)
        {
            try
            {
                var chatroom = await _chatroomManagerService.GetChatroomAsync(id);
                var chatroomUsers = includeUsers
                    ? (await _chatroomManagerService.GetChatroomUsersAsync(id)).Select(user => new BasicUserResponse
                    {
                        Id = user.Id,
                        Username = user.Username,
                    }).ToList()
                    : Enumerable.Empty<BasicUserResponse>().ToList();
                return new JsonResult(new ChatroomResponse
                {
                    Id = chatroom.Id,
                    Name = chatroom.Name,
                    IsShared = chatroom.IsShared,
                    Users = chatroomUsers,
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve chatroom with id ({id}).");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateChatroomRequest request)
        {
            try
            {
                var chatroom = await _chatroomManagerService.CreateChatroomAsync(new CreateChatroomModel
                {
                    ChatroomName = request.ChatroomName ?? string.Empty,
                    OwnerId = request.OwnerId,
                    IsShared = request.IsShared,
                });
                if (chatroom is null)
                {
                    return BadRequest();
                }

                _notificationsService.EnqueueNotification(new ChatroomChangedNotification(
                    chatroomId: chatroom.Id,
                    chatroomName: chatroom.Name,
                    participants: new int[] { request.OwnerId, }));

                return new JsonResult(new BasicChatroomResponse
                {
                    Id = chatroom.Id,
                    Name = chatroom.Name,
                    IsShared = chatroom.IsShared,
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not create chatroom with name ({request.ChatroomName}).");
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _chatroomManagerService.DeleteChatroomAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not delete chatroom with id ({id}).");
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtSchemes.Admin + "," + JwtSchemes.User)]
        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetChatroomMessages(
            int id,
            [FromServices] IMessagesManager messagesManager,
            [FromQuery] DateTime? olderThan = null,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var messages = await messagesManager.GetChatroomMessagesAsync(
                    id,
                    olderThan ?? DateTime.UtcNow,
                    pageSize);
                return new JsonResult(messages.Select(m => new MessageResponse
                {
                    AuthorId = m.AuthorId,
                    Body = m.Body,
                    ChatroomId = m.ChatroomId,
                    Id = m.Id,
                    TimeSentUtc = m.TimeSentUtc,
                }));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not retrieve messages for chatroom with id ({id}).");
                return BadRequest();
            }
        }

        [HttpPost("{id}/users")]
        public async Task<ActionResult> AddUserAsync(
            int id,
            [FromBody] AddUserToChatroomRequest request)
        {
            try
            {
                var chatroom = await _chatroomManagerService.GetChatroomAsync(id);
                if (chatroom is null)
                {
                    return NotFound();
                }

                var users = await _chatroomManagerService.GetChatroomUsersAsync(id);
                if (!chatroom.IsShared && users.Count() == 2)
                {
                    return BadRequest();
                }

                await _chatroomManagerService.AddUserToChatroomAsync(new AddUserToChatroomModel
                {
                    ChatroomId = id,
                    UserId = request.UserId,
                });

                _notificationsService.EnqueueNotification(new ChatroomChangedNotification(
                    chatroomId: chatroom.Id,
                    chatroomName: chatroom.Name,
                    participants: users.Select(user => user.Id).Append(request.UserId)));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Could not add user with id ({request.UserId}) to chatroom with id ({id}).");
                return BadRequest();
            }
        }
    }
}
