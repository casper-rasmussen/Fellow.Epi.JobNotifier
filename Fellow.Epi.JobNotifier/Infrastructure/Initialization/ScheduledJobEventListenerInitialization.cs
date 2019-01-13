using System;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.Internal;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.Notification;
using EPiServer.ServiceLocation;
using Fellow.Epi.JobNotifier.Bootstrapper;
using Fellow.Epi.JobNotifier.Manager.JobNotification;
using Fellow.Epi.JobNotifier.Manager.JobNotification.Entity;
using StructureMap;
using Fellow.Epi.JobNotifier.Infrastructure.Formatter;

namespace Fellow.Epi.JobNotifier.Infrastructure.Initialization
{
	[InitializableModule]
	[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
	public class ScheduledJobEventListenerInitialization : IInitializableModule
	{
		
		public void Initialize(InitializationEngine context)
		{
		    context.InitComplete += (s, e) =>
		    {
		        IScheduledJobEventsRaiser eventRaiser = context.Locate.Advanced.GetInstance<IScheduledJobEventsRaiser>();

		        eventRaiser.Executed += (sender, args) =>
		        {
		            IJobNotificationManager jobNotificationManager = context.Locate.Advanced.GetInstance<IJobNotificationManager>();

		            INotification notification;

		            bool found = jobNotificationManager.TryGet(args.Job, !args.Job.HasLastExecutionFailed, args.Job.CurrentStatusMessage, out notification);

		            if (found)
		            {
		                ILogger logger = LogManager.GetLogger();
                        
                        if (String.IsNullOrEmpty(notification.Sender))
		                {
		                    logger.Error("Sender has to be set in order to notify about changes");
		                    return;
		                }

		                if (notification.Recipients == null || !notification.Recipients.Any())
		                {
		                    logger.Error("Recipient has to be set in order to notify about changes");
		                    return;
		                }

		                INotifier notifier = context.Locate.Advanced.GetInstance<INotifier>();
		                IScheduledJobNotificationFormatter formatter = context.Locate.Advanced.GetInstance<IScheduledJobNotificationFormatter>();

		                notifier.PostNotificationAsync(new NotificationMessage()
		                {
		                    ChannelName = formatter.ChannelName,
		                    Content = notification.Message,
		                    Subject = notification.Subject,
		                    Recipients = notification.Recipients.Select(name => new NotificationUser(name)),
		                    Sender = new NotificationUser(notification.Sender),
		                    TypeName = "ScheduledJobNotification"
		                });
		            }
		        };

            };
		    
		}

        private void Context_InitComplete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Uninitialize(InitializationEngine context)
		{
		}

		public void Preload(string[] parameters)
		{
		}
	}
}
