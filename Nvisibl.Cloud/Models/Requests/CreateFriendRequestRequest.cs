﻿using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models.Requests
{
    public class CreateFriendRequestRequest
    {
        [Required]
        public int SenderId { get; set; }
    }
}
