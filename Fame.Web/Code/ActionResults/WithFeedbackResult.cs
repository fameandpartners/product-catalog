using Fame.Web.Code.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fame.Web.Code.ActionResults
{
    public class WithFeedBackResult : IActionResult
	{
		public ActionResult Result { get; private set; }

		private readonly NotificationType _status;
		private readonly string _message;
        private bool notificationAdded = false;

		/// <summary>Constructor</summary>
		/// <param name="result">The action result to be executed</param>
		/// <param name="status">The status of the notification message to be added to the action result</param>
		/// <param name="message">The notification message to be added to the action result</param>
		public WithFeedBackResult(ActionResult result, NotificationType status, string message)
		{
			Result = result;
			_status = status;
			_message = message;
		}

        public Task ExecuteResultAsync(ActionContext context)
        {
            if (!notificationAdded)
            {
                notificationAdded = true;
                context.Notifications().Add(_status, _message);
            }
            return Result.ExecuteResultAsync(context);
        }
    }
}
