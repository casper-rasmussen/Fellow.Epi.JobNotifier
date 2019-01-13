using System;
using System.Web.Security;
using EPiServer.DataAbstraction;
using EPiServer.Shell.Security;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;

namespace Fellow.Epi.JobNotifier.Manager.JobNotification
{
	class JobNotificationManager : IJobNotificationManager
	{
	    private readonly UIRoleProvider _uiRoleProvider;
	    public JobNotificationManager(UIRoleProvider uiRoleProvider)
	    {
	        this._uiRoleProvider = uiRoleProvider;
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
				Recipients = this._uiRoleProvider.GetUsersInRole("NotificationUsers"),
				Sender = "NotificationUser"
			};

			return true;
		}
	}
}
