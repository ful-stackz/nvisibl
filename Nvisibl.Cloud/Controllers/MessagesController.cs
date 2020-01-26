using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Models.Messages;
using Nvisibl.Cloud.Services.Interfaces;
using System;
using System.Threading.Tasks;

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
            [FromBody] CreateMessageModel createMessageModel)
        {
            try
            {
                return new JsonResult(await _messagesManagerService.CreateMessageAsync(createMessageModel));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    $"Could add message from user with id ({createMessageModel.AuthorId}) to chatroom with " +
                    $"id ({createMessageModel.ChatroomId}).");
                return BadRequest();
            }
        }
    }
}
