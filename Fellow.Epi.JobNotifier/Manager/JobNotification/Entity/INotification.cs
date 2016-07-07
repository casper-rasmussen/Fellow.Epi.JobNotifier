using System.Collections.Generic;

namespace Fellow.Epi.JobNotifier.Manager.JobNotification.Entity
{
	public interface INotification
	{
		string Subject { get; }

		string Message { get; }

		IEnumerable<string> Recipients { get; } 

		string Sender { get; }
	}
}
