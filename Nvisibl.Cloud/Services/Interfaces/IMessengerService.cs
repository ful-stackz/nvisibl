using Nvisibl.Cloud.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface IMessengerService
    {
        IObservable<MessageModel> DispatchedMessages { get; }

        Task SendMessageAsync(CreateMessageModel newMessageModel);
    }
}
