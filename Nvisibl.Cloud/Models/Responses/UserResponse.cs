﻿using System;
using System.Collections.Generic;

namespace Nvisibl.Cloud.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public IList<BasicUserResponse> Friends { get; set; } = Array.Empty<BasicUserResponse>();
    }
}
