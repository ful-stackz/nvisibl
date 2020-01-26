using System;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Responses
{
    public class MessageResponse
    {
        public int AuthorId { get; set; }
        public string Body { get; set; } = string.Empty;
        public int ChatroomId { get; set; }
        public int Id { get; set; }
        public DateTime TimeSentUtc { get; set; }
    }
}
