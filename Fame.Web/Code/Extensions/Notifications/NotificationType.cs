using System.ComponentModel.DataAnnotations;

namespace Fame.Web.Code.Extensions
{
    /// <summary>Defines a list of message status types</summary>
    public enum NotificationType
	{
        [Display(Name = "Success")]
		Success,
        [Display(Name = "Information")]
        Information,
        [Display(Name = "Warning")]
        Warning,
        [Display(Name = "Oops")]
        Error
	}
}
