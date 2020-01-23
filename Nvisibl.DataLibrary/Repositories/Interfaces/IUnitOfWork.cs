using System;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        T GetRepository<T>() where T : class;
        Task<int> CompleteAsync();
    }
}
