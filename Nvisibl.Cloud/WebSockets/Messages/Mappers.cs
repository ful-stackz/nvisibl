using Nvisibl.Cloud.Models;
using System;

namespace Nvisibl.Cloud.WebSockets.Messages
{
    internal static class Mappers
    {
        internal static CreateMessageModel ToCreateMessageModel(Client.ChatroomMessageMessage message) =>
            message is null
            ? throw new ArgumentNullException(nameof(message))
            : new CreateMessageModel
            {
                AuthorId = message.AuthorId,
                Body = message.Body,
                ChatroomId = message.ChatroomId,
                TimeSentUtc = message.TimeSentUtc,
            };
    }
}
