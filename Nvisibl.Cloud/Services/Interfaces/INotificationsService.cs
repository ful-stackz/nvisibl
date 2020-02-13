using Nvisibl.Cloud.Models.Data;
using System;

namespace Nvisibl.Cloud.Services.Interfaces
{
    public interface INotificationsService
    {
        IObservable<Notification> Notifications { get; }

        void SendNotification(Notification notification);
    }
}