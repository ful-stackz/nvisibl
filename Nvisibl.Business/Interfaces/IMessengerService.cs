using Nvisibl.Business.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Nvisibl.Business.Interfaces
{
    public interface IMessengerService
    {
        IObservable<MessageModel> DispatchedMessages { get; }

        Task SendMessageAsync(CreateMessageModel newMessageModel);
    }
}
