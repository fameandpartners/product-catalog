using System;

namespace Fame.Web.Code.Extensions
{
    public static class NotificationTypeExtensions
	{
		public static string ToCssStyle(this NotificationType notificationType)
		{
			switch (notificationType)
			{
				case NotificationType.Error:
					return "alert-danger";
				case NotificationType.Information:
					return "alert-info";
				case NotificationType.Success:
					return "alert-success";
				case NotificationType.Warning:
					return "alert-warning";
			}
			throw new ApplicationException("Invalid FeedbackType");
		}

		public static string ToButtonType(this NotificationType notificationType)
		{
			switch (notificationType)
			{
				case NotificationType.Error:
					return "close_white.png";
				case NotificationType.Information:
					return "close_white.png";
				case NotificationType.Success:
					return "close_white.png";
				case NotificationType.Warning:
					return "close.png";
			}
			throw new ApplicationException("Invalid FeedbackType");
		}
	}
}
