# Fellow.Epi.JobNotifier: Real-time notifications about failed Scheduled Jobs

Provides Administrators with real-time updates via Episervers notification system. It acts as the channel for communication between failing scheduled jobs and the Episerver User Notification System.

## Installation and Usage

You can get the latest version of Fellow.Epi.JobNotifier through [Episervers NuGet Feed](http://nuget.episerver.com/en/OtherPages/Package/?packageId=Fellow.Epi.JobNotifier)
Be aware that Fellow.Epi.JobNotifier requires EPiServer.CMS.Core version 9.8.0.0 or higher.

Please use this GitHub project for any issues, questions or other kinds of feedback.

By default, notifications are sent to all users part of a "NotificationUsers" group. Please be aware that it is using a dedicated user, named "NotificationUser", as sender and this user has to be configured prior use.

## Configuration

It is possible to adjust the Notification behaviour, recipient, sender and content by replacing or intercepting existing implemetation of IJobNotificationManager.

```
public interface IJobNotificationManager
{
	bool TryGet(ScheduledJob job, bool success, string message, out INotification notification);
}
```

Default implementation is based on above mentioned conventions and provides a seamless setup. 

```
class JobNotificationManager : IJobNotificationManager
{
	public bool TryGet(ScheduledJob job, bool success, string message, out INotification notification)
	{
		notification = default(INotification);

		if (success)
			return false;

		notification = new Notification()
		{
			Subject = "Scheduled Job Failed",
			Message = String.Format("{0}: {1}", job.Name, message),
			Recipients = Roles.GetUsersInRole("NotificationUsers"),
			Sender = "NotificationUser"
		};

		return true;
	}
}
```