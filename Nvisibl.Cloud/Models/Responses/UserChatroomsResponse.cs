using System;
using System.Collections.Generic;

namespace Nvisibl.Cloud.Models.Responses
{
    public class UserChatroomsResponse
    {
        public IEnumerable<ChatroomResponse> Chatrooms { get; set; } = Array.Empty<ChatroomResponse>();
    }
}
