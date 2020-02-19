namespace Nvisibl.Business.Models.Users
{
    public class FriendModel
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public bool Accepted { get; set; }
    }
}
