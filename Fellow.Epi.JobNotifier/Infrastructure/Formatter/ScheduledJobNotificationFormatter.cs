using System.Collections.Generic;
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

		/// <summary>
		///     Performs formatting of messages.
		/// </summary>
		/// <param name="notifications">Messages to format</param>
		/// <param name="recipient">The receiver of the message</param>
		/// <param name="format">The format to format to</param>
		/// <param name="channelName">The message channel</param>
		/// <returns>A list of formatted messages</returns>
		/// <remarks>One use case for a formatter might be to combine several messages into one.</remarks>
		public IEnumerable<FormatterNotificationMessage> FormatMessages(IEnumerable<FormatterNotificationMessage> notifications, string recipient, NotificationFormat format, string channelName)
		{
			return notifications;
		}

		/// <summary>
		///     Formats the user message.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <returns>UserNotificationMessage.</returns>
		public UserNotificationMessage FormatUserMessage(UserNotificationMessage notification)
		{
			if (notification != null)
			{
				notification.Content = "<div style=\"color:#f7542b\">" + notification.Content + "</div>";
			}

			return notification;
		}
	}
}
