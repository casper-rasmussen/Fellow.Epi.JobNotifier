using EPiServer.DataAbstraction;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;

namespace Fellow.Epi.JobNotifier.Manager.JobNotification
{
	public interface IJobNotificationManager
	{
		bool TryGet(ScheduledJob job, bool success, string message, out INotification notification);
	}
}
