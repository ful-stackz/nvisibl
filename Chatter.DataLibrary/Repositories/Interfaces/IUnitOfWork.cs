using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IFriendsRepository FriendsRepository { get; }
        IUserRepository UserRepository { get; }

        Task<int> CompleteAsync();
    }
}
