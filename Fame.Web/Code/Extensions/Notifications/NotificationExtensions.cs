using Fame.Web.Code.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Fame.Web.Code.Extensions
{
    public static class NotificationExtensions
	{
        public static NotificationList Notifications(this ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            var tempData = factory.GetTempData(context.HttpContext);
            return new NotificationList(tempData);
        }

        public static NotificationList Notifications(this IHtmlHelper htmlHelper)
		{
			return new NotificationList(htmlHelper.ViewContext.TempData);
		}

        public static NotificationList Notifications(this Controller controller)
		{
			return new NotificationList(controller.TempData);
		}

		public static IActionResult WithNotification(this ActionResult actionResult, NotificationType status, string message, params object[] args)
		{
			return new WithFeedBackResult(actionResult, status, string.Format(message, args));
		}
	}
}
