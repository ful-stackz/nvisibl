using System.ComponentModel.DataAnnotations;

namespace Nvisibl.DataLibrary.Models
{
    public class Friend
    {
        [Required]
        public int User1Id { get; set; }

        [Required]
        public int User2Id { get; set; }

        public User? User1 { get; set; }

        public User? User2 { get; set; }
    }
}
