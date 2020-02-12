namespace Nvisibl.Cloud.Models.Responses
{
    public class FriendRequestResponse
    {
        public int Id { get; set; }
        public bool Accepted { get; set; }
        public BasicUserResponse Sender { get; set; } = new BasicUserResponse();
        public BasicUserResponse Receiver { get; set; } = new BasicUserResponse();
    }
}
