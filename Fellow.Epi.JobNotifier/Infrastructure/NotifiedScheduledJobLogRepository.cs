using System;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.DataAbstraction;
using EPiServer.Logging;
using EPiServer.Notification;
using Fellow.Epi.JobNotifier.Infrastructure.Formatter;
using Fellow.Epi.JobNotifier.Manager.JobNotification;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;
using EPiServer.DataAccess.Internal;
using EPiServer.Scheduler;
using EPiServer.Scheduler.Internal;

namespace Fellow.Epi.JobNotifier.Infrastructure
{
	class NotifiedScheduledJobLogRepository : DefaultScheduledJobLogRepository, IScheduledJobLogRepository
    {
		private readonly INotifier _notifier;
		private readonly IScheduledJobRepository _scheduledJobRepository;
		private readonly IScheduledJobNotificationFormatter _formatter;
		private readonly IJobNotificationManager _jobNotificationManager;
		private static readonly ILogger Logger = LogManager.GetLogger(typeof(NotifiedScheduledJobLogRepository));


		public NotifiedScheduledJobLogRepository(INotifier notifier, IScheduledJobRepository scheduledJobRepository, IScheduledJobNotificationFormatter formatter, IJobNotificationManager jobNotificationManager, SchedulerDB db)
			: base(db)
		{
			_notifier = notifier;
			_scheduledJobRepository = scheduledJobRepository;
			_formatter = formatter;
			_jobNotificationManager = jobNotificationManager;
		}

        async Task IScheduledJobLogRepository.LogAsync(Guid id, ScheduledJobLogItem item)
        {
            await LogAsync(id, item);

            var executedJob = _scheduledJobRepository.Get(id);
            var succeeded = item.Status == ScheduledJobExecutionStatus.Succeeded;

            INotification notification;
            var found = _jobNotificationManager.TryGet(executedJob, succeeded, item.Message, out notification);


            if (found)
            {
                if (string.IsNullOrEmpty(notification.Sender))
                {
                    Logger.Error("Sender has to be set in order to notify about changes");
                    return;
                }

                if (notification.Recipients == null || !notification.Recipients.Any())
                {
                    Logger.Error("Recipient has to be set in order to notify about changes");
                    return;
                }

                await _notifier.PostNotificationAsync(new NotificationMessage
                {
                    ChannelName = _formatter.ChannelName,
                    Content = notification.Message,
                    Subject = notification.Subject,
                    Recipients = notification.Recipients.Select(name => new NotificationUser(name)),
                    Sender = new NotificationUser(notification.Sender),
                    TypeName = "ScheduledJobNotification"
                });
            }
        }
	}
}
