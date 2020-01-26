using Microsoft.Extensions.DependencyInjection;
using Nvisibl.Cloud.Models.Messages;
using Nvisibl.Cloud.Services.Interfaces;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Nvisibl.Cloud.Services
{
    public class MessengerService : IMessengerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Subject<MessageModel> _dispatchedMessagesSubject = new Subject<MessageModel>();

        public MessengerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IObservable<MessageModel> DispatchedMessages =>
            _dispatchedMessagesSubject.AsObservable();

        public async Task SendMessageAsync(CreateMessageModel createMessageModel)
        {
            using var scope = _serviceProvider.CreateScope();
            var messagesManager = scope.ServiceProvider.GetService<IMessagesManager>();

            var message = await messagesManager.CreateMessageAsync(createMessageModel);

            _dispatchedMessagesSubject.OnNext(message);
        }
    }
}
