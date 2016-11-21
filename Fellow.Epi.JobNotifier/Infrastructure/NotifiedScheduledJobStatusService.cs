using System;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Logging;
using EPiServer.Notification;
using Fellow.Epi.JobNotifier.Infrastructure.Formatter;
using Fellow.Epi.JobNotifier.Manager.JobNotification;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;
using EPiServer.DataAbstraction.Internal;
using EPiServer.DataAccess.Internal;
using EPiServer.ServiceLocation;

namespace Fellow.Epi.JobNotifier.Infrastructure
{
	class NotifiedScheduledJobStatusService : DefaultScheduledJobRepository, IScheduledJobStatusService
	{
		private readonly INotifier _notifier;
		private readonly IScheduledJobRepository _scheduledJobRepository;
		private readonly IScheduledJobNotificationFormatter _formatter;
		private readonly IJobNotificationManager _jobNotificationManager;
		private readonly ILogger _logger;

		public NotifiedScheduledJobStatusService(INotifier notifier, IScheduledJobRepository scheduledJobRepository, IScheduledJobNotificationFormatter formatter, IJobNotificationManager jobNotificationManager, ILogger logger, ServiceAccessor<SchedulerDB> db)
			: base(db)
		{
			this._notifier = notifier;
			this._scheduledJobRepository = scheduledJobRepository;
			this._formatter = formatter;
			this._jobNotificationManager = jobNotificationManager;
			this._logger = logger;
		}

		public override void ReportExecutedJob(Guid id, ScheduledJobLogItem status)
		{

			base.ReportExecutedJob(id, status);

			ScheduledJob executedJob = this._scheduledJobRepository.Get(id);

			INotification notification;

			bool found = this._jobNotificationManager.TryGet(executedJob, status.Succeeded, status.Message, out notification);


			if (found)
			{
				if (String.IsNullOrEmpty(notification.Sender))
				{
					this._logger.Error("Sender has to be set in order to notify about changes");
					return;
				}

				if (notification.Recipients == null || !notification.Recipients.Any())
				{
					this._logger.Error("Recipient has to be set in order to notify about changes");
					return;
				}

				this._notifier.PostNotificationAsync(new NotificationMessage()
				{
					ChannelName = this._formatter.ChannelName,
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
