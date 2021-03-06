﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvisibl.DataLibrary.Models
{
    public class Chatroom
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsShared { get; set; }

        public IList<ChatroomUser> Users { get; set; } = new List<ChatroomUser>();

        public IList<Message> Messages { get; set; } = new List<Message>();
    }
}
