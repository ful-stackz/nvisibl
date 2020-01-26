using Nvisibl.Cloud.Models.Messages;

namespace Nvisibl.Cloud.WebSockets.Messages
{
    internal static class Mappers
    {
        internal static CreateMessageModel ToCreateMessageModel(Client.ChatroomMessageMessage message) =>
            new CreateMessageModel
            {
                AuthorId = message.AuthorId,
                Body = message.Body,
                ChatroomId = message.ChatroomId,
                TimeSentUtc = message.TimeSentUtc,
            };
    }
}
