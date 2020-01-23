using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nvisibl.DataLibrary.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Username { get; set; } = string.Empty;

        public virtual IList<Friend> Friends { get; set; } = new List<Friend>();

        public virtual IList<Friend> FriendedBy { get; set; } = new List<Friend>();
    }
}
