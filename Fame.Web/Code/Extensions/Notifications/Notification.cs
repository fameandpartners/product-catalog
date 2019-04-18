using System;

namespace Fame.Web.Code.Extensions
{
    /// <summary>Defines a single status message</summary>
    [Serializable]
	public class Notification
	{
		/// <summary>Gets or sets the status message type</summary>
		public NotificationType Status { get; set; }

		/// <summary>Gets or sets the status message</summary>
		public string Message { get; set; }

        public bool Persist { get; set; }
	}
}
