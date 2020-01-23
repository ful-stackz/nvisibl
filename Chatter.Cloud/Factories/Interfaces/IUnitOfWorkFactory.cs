using Nvisibl.DataLibrary.Repositories.Interfaces;

namespace Nvisibl.Cloud.Factories.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}