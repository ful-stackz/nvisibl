using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Business.Interfaces;
using System;
using System.Threading.Tasks;
using Nvisibl.Cloud.Models.Requests;
using Nvisibl.Cloud.Models.Responses;
using Nvisibl.Business.Models.Messages;

namespace Nvisibl.Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesManager _messagesManagerService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessagesManager messagesManagerService, ILogger<MessagesController> logger)
        {
            _messagesManagerService = messagesManagerService ?? throw new ArgumentNullException(nameof(messagesManagerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(
            [FromBody] CreateMessageRequest request)
        {
            try
            {
                var message = await _messagesManagerService.CreateMessageAsync(
                    new CreateMessageModel
                    {
                        AuthorId = request.AuthorId,
                        Body = request.Body,
                        ChatroomId = request.ChatroomId,
                        TimeSentUtc = request.TimeSentUtc,
                    });
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
