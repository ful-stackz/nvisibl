using System;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Messages
{
    public class CreateMessageModel
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int ChatroomId { get; set; }

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        public DateTime TimeSentUtc { get; set; }
    }
}
