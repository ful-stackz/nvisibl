using System;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Messages
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
