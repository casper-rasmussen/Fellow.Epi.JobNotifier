using System;
using System.Web.Security;
using EPiServer.DataAbstraction;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;

namespace Fellow.Epi.JobNotifier.Manager.JobNotification
{
	class JobNotificationManager : IJobNotificationManager
	{
		public bool DismissNotification(ScheduledJob job, bool success)
		{
			//Dismiss if it succeeded
			return success;
		}

		public bool TryGet(ScheduledJob job, bool success, string message, out INotification notification)
		{
			notification = default(INotification);

			if (success)
				return false;

			notification = new Notification()
			{
				Subject = "Scheduled Job Failed",
				Message = String.Format("{0}: {1}", job.Name, message),
				Recipients = Roles.GetUsersInRole("Administrators"),
				Sender = "NotificationUser"
			};

			return true;
		}
	}
}
