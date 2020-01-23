using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ChatContext _chatContext;

        private bool _isDisposed;

        public UnitOfWork(ChatContext chatContext)
        {
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
            FriendsRepository = new FriendsRepository(chatContext);
            UserRepository = new UserRepository(chatContext);
        }

        public IFriendsRepository FriendsRepository { get; }
        public IUserRepository UserRepository { get; }

        public async Task<int> CompleteAsync()
        {
            return await _chatContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    _chatContext.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
