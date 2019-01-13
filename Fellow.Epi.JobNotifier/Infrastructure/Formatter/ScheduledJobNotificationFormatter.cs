using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPiServer.Notification;

namespace Fellow.Epi.JobNotifier.Infrastructure.Formatter
{
	public class ScheduledJobNotificationFormatter : INotificationFormatter, IUserNotificationFormatter, IScheduledJobNotificationFormatter
	{
		public string ChannelName { get { return "epi.scheduledjobs"; } }
	
		/// <summary>
		///     The name of the formatter.
		/// </summary>
		/// <value>The name of the formatter.</value>
		public string FormatterName
		{
			get
			{
				return "scheduledjobsnotificationformatter";
			}
		}

		/// <summary>
		///     Specifies which channels the formatter supports.
		/// </summary>
		/// <value>The supported channel names.</value>
		public IEnumerable<string> SupportedChannelNames
		{
			get
			{
				return new[] { this.ChannelName };
			}
		}
        
        public Task<IEnumerable<FormatterNotificationMessage>> FormatMessagesAsync(IEnumerable<FormatterNotificationMessage> notifications, string recipient, NotificationFormat format, string channelName)
        {
            return Task.FromResult(notifications);
        }

        public Task<UserNotificationMessage> FormatUserMessageAsync(UserNotificationMessage notification)
        {
            if (notification != null)
            {
                notification.Content = "<div style=\"color:#f7542b\">" + notification.Content + "</div>";
            }

            return Task.FromResult(notification);
        }
    }
}
