using System.ComponentModel.DataAnnotations;

namespace Nvisibl.DataLibrary.Models
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        public int User1Id { get; set; }

        [Required]
        public int User2Id { get; set; }

        [Required]
        public bool Accepted { get; set; }

        public User? User1 { get; set; }

        public User? User2 { get; set; }
    }
}
