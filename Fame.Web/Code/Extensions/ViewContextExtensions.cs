using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fame.Web.Code.Extensions
{
    public static class ViewContextExtensions
	{
		public static string AreaName(this ViewContext context)
		{
            var areaName = context.RouteData.Values["area"];
            if (areaName == null || string.IsNullOrEmpty(areaName.ToString())) return "";
            return areaName.ToString().ToLowerInvariant();
        }

		public static string ControllerName(this ViewContext context)
		{
			return context.GetRouteValue("Controller");
		}

		public static string ActionName(this ViewContext context)
		{
			return context.GetRouteValue("Action");
		}

		public static string GetDataToken(this ViewContext context, string tokenName)
		{
			var token = context.RouteData.DataTokens[tokenName];
			return token == null ? string.Empty : token.ToString().ToLower();
		}

		public static string GetRouteValue(this ViewContext context, string tokenName)
		{
			var token = context.RouteData.Values[tokenName];
			return token == null ? string.Empty : token.ToString().ToLower();

		}
	}
}
