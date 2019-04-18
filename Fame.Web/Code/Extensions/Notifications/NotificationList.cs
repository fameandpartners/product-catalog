using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fame.Web.Code.Extensions
{
    /// <summary>Contains a list of status messages</summary>
    [Serializable]
	public class NotificationList
	{
		private const string DictionaryName = "__feedback_messages__";
		private readonly List<Notification> _feedback;
        private ITempDataDictionary _dataDictionary;

		/// <summary>Constructor</summary>
		/// <param name="dataDictionary">The temporary data dictionary for the request</param>
		public NotificationList(ITempDataDictionary dataDictionary)
		{
            _dataDictionary = dataDictionary;
            if (_dataDictionary.ContainsKey(DictionaryName)){
                _feedback = _dataDictionary.Get<List<Notification>>(DictionaryName);
            } else
            {
                _feedback = new List<Notification>();
            }
		}

		/// <summary>Gets the list of status messages</summary>
		public List<Notification> All
		{
			get { return _feedback.ToList(); }
		}

		public bool Any
		{
			get { return _feedback.Any(); }
		}

		/// <summary>Adds a new status message to be displayed to the user</summary>
		/// <param name="status">The status type for the message</param>
		/// <param name="message">The message to be displayed to the user</param>
		/// <param name="args">The arguments for the message</param>
		/// <returns>A chaining reference</returns>
		public NotificationList Add(NotificationType status, string message, params object[] args)
		{
			_feedback.Add(new Notification { Status = status, Message = string.Format(message, args) });
            _dataDictionary.Put(DictionaryName, _feedback);
			return this;
		}
	}
}
