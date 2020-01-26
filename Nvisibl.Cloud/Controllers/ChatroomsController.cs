using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Models.Chatrooms;
using Nvisibl.Cloud.Models.Users;
using Nvisibl.Business.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomsController : ControllerBase
    {
        private readonly IChatroomsManager _chatroomManagerService;
        private readonly ILogger<ChatroomsController> _logger;

        public ChatroomsController(IChatroomsManager chatroomManagerService, ILogger<ChatroomsController> logger)
        {
            _chatroomManagerService = chatroomManagerService ?? throw new ArgumentNullException(nameof(chatroomManagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return new JsonResult(await _chatroomManagerService.GetChatroomsAsync(page, pageSize));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
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
                    ? await _chatroomManagerService.GetChatroomUsersAsync(id)
                    : Array.Empty<Business.Models.Users.UserModel>();
                return new JsonResult(new
                {
                    chatroom.Id,
                    chatroom.Name,
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
            [FromBody] CreateChatroomModel createChatroomModel)
        {
            try
            {
                var chatroom = await _chatroomManagerService.CreateChatroomAsync(
                    new Business.Models.Chatrooms.CreateChatroomModel
                    {
                        ChatroomName = createChatroomModel.ChatroomName,
                        OwnerId = createChatroomModel.OwnerId,
                    });
                return chatroom is { }
                    ? new JsonResult(chatroom)
                    : (ActionResult)BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not create chatroom with name ({createChatroomModel.ChatroomName}).");
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


        [HttpPost("{id}/users")]
        public async Task<ActionResult> AddUserAsync(
            int id,
            [FromBody] AddUserToChatroomModel addUserToChatroomModel)
        {
            try
            {
                await _chatroomManagerService.AddUserToChatroomAsync(
                    new Business.Models.Chatrooms.AddUserToChatroomModel
                    {
                        ChatroomId = id,
                        UserId = addUserToChatroomModel.UserId,
                    });
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Could not add user with id ({addUserToChatroomModel.UserId}) to chatroom with id ({id}).");
                return BadRequest();
            }
        }
    }
}
