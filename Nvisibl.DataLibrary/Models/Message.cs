using System;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.DataLibrary.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Body { get; set; } = string.Empty;

        [Required]
        public DateTime TimeSentUtc { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int ChatroomId { get; set; }

        public User? Author { get; set; }

        public Chatroom? Chatroom { get; set; }
    }
}
