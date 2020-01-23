using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Models;
using Nvisibl.Cloud.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomsController : ControllerBase
    {
        private readonly IChatroomManagerService _chatroomManagerService;
        private readonly ILogger<ChatroomsController> _logger;

        public ChatroomsController(IChatroomManagerService chatroomManagerService, ILogger<ChatroomsController> logger)
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
                return new JsonResult(includeUsers
                    ? await _chatroomManagerService.GetChatroomByIdWithUsersAsync(id)
                    : await _chatroomManagerService.GetChatroomByIdAsync(id));
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
                var chatroom = await _chatroomManagerService.CreateChatroomAsync(createChatroomModel);
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
            [FromBody] UserModel user)
        {
            try
            {
                await _chatroomManagerService.AddUserToChatroomAsync(id, user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Could not add user with id ({user.Id}) to chatroom with id ({id}).");
                return BadRequest();
            }
        }
    }
}
