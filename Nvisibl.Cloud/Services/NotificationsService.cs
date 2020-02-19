using Nvisibl.Cloud.Models.Data;
using Nvisibl.Cloud.Services.Interfaces;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Nvisibl.Cloud.Services
{
    internal class NotificationsService : INotificationsService, IDisposable
    {
        private const string SchedulerThreadName = "NotificationServiceThread";

        private readonly Subject<Notification> _notificiationsSubject = new Subject<Notification>();
        private readonly EventLoopScheduler _notificationScheduler;

        private bool _isDisposed;

        public NotificationsService()
        {
            _notificationScheduler = new EventLoopScheduler(threadStart => new Thread(threadStart)
            {
                IsBackground = true,
                Name = SchedulerThreadName,
                Priority = ThreadPriority.Normal,
            });
        }

        public IObservable<Notification> Notifications =>
            _notificiationsSubject.ObserveOn(_notificationScheduler).AsObservable();

        public void SendNotification(Notification notification)
        {
            if (notification is null)
            {
                return;
            }

            _notificiationsSubject.OnNext(notification);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _notificiationsSubject.Dispose();
                    _notificationScheduler.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
