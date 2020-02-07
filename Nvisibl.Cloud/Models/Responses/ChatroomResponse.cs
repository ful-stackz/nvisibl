using System;
using System.Collections.Generic;

namespace Nvisibl.Cloud.Models.Responses
{
    public class ChatroomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsShared { get; set; }
        public IList<BasicUserResponse> Users { get; set; } = Array.Empty<BasicUserResponse>();
    }
}
