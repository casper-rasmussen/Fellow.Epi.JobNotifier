using System.Collections.Generic;

namespace Fellow.Epi.JobNotifier.Manager.JobNotification.Entity
{
	public class Notification : INotification
	{
		public Notification()
		{
			this.Recipients = new List<string>();
		}

		public string Subject { get; set; }

		public string Message { get; set; }

		public IEnumerable<string> Recipients { get; set; }

		public string Sender { get; set; }
	}
}
