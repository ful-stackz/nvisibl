using Nvisibl.Cloud.Factories.Interfaces;
using Nvisibl.DataLibrary.Contexts;
using Nvisibl.DataLibrary.Repositories;
using Nvisibl.DataLibrary.Repositories.Interfaces;

namespace Nvisibl.Cloud.Factories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ChatContext _chatContext;

        public UnitOfWorkFactory(ChatContext chatContext)
        {
            _chatContext = chatContext ?? throw new System.ArgumentNullException(nameof(chatContext));
        }

        public IUnitOfWork Create() =>
            new UnitOfWork(_chatContext);
    }
}
