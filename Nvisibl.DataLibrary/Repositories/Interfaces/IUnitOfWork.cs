using System;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IFriendsRepository FriendsRepository { get; }
        IUserRepository UserRepository { get; }
        IChatroomRepository ChatroomRepository { get; }
        IChatroomUserRepository ChatroomUserRepository { get; }
        IMessageRepository MessageRepository { get; }

        Task<int> CompleteAsync();
    }
}
