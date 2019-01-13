using EPiServer.DataAbstraction;
using EPiServer.Notification;
using Fellow.Epi.JobNotifier.Infrastructure;
using Fellow.Epi.JobNotifier.Infrastructure.Formatter;
using Fellow.Epi.JobNotifier.Manager.JobNotification;
using StructureMap;

namespace Fellow.Epi.JobNotifier.Bootstrapper
{
	public class FellowEpiJobNotifierImplementationBootstrapper : Registry
	{
		public FellowEpiJobNotifierImplementationBootstrapper()
		{
			//Manager
			this.For<IJobNotificationManager>().Use<JobNotificationManager>();

			//Formatter
			this.For<IScheduledJobNotificationFormatter>().Use<ScheduledJobNotificationFormatter>();

			//Episerver overrides
			this.For<IUserNotificationFormatter>().Add<ScheduledJobNotificationFormatter>();
			this.For<INotificationFormatter>().Add<ScheduledJobNotificationFormatter>();
		}
	}
}
