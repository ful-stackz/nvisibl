using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatContext _chatContext;
        private readonly List<IRepository> _repositories;

        private bool _isDisposed;

        public UnitOfWork(ChatContext chatContext)
        {
            _chatContext = chatContext ?? throw new ArgumentNullException(nameof(chatContext));
            _repositories = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<RepositoryAttribute>() is { })
                .Where(t => t.GetConstructor(new Type[] { typeof(ChatContext) }) is { })
                .Select(t => Activator.CreateInstance(t, chatContext))
                .OfType<IRepository>()
                .ToList();
        }

        public T GetRepository<T>()
            where T : class, IRepository
        {
            var targetType = typeof(T);
            return _repositories.FirstOrDefault(repo => targetType.IsAssignableFrom(repo.GetType())) as T ??
                throw new InvalidOperationException($"Repository of type {nameof(T)} does not exist.");
        }

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
