using System.ComponentModel.DataAnnotations;

namespace Nvisibl.Cloud.Models
{
    public class MessageWithChatroomModel : MessageModel
    {
        [Required]
        public ChatroomModel Chatroom { get; set; } = new ChatroomModel();
    }
}
