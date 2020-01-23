using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Models;
using Nvisibl.DataLibrary.Repositories.Interfaces;

namespace Nvisibl.DataLibrary.Repositories
{
    [Repository]
    internal class ChatroomUserRepository : Repository<ChatroomUser>, IChatroomUserRepository
    {
        public ChatroomUserRepository(ChatContext context)
            : base(context)
        {
        }
    }
}
