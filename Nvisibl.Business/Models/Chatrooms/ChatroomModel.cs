using Nvisibl.Business.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Business.Models.Chatrooms
{
    public class ChatroomModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsShared { get; set; }

        [Required]
        public IList<UserModel> Users { get; set; } = Array.Empty<UserModel>();
    }
}
