using System;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class MessageModel
    {
        [Required]
        public int Id { get; set; }

        public string Body { get; set; } = string.Empty;

        public DateTime TimeSentUtc { get; set; }
    }
}
