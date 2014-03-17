namespace Spark.Web
{
	using System;
	using System.Web;

	/// <summary>
	/// Utility class for dealing with URLs.
	/// </summary>
	public static class UrlHelper
	{
		/// <summary>
		/// Adds a Querystring parameter to the current Request's URL.
		/// </summary>
		/// <param name="parameterName">The key for the Querystring entry.</param>
		/// <param name="parameterValue">The value for the Querystring entry.</param>
		/// <returns>A URL concatenated with the new querystring parameter.</returns>
		public static string AddParameterToUrl(string parameterName, string parameterValue)
		{
			return AddParameterToUrl(HttpContext.Current.Request, parameterName, parameterValue);
		}

		/// <summary>
		/// Adds a Querystring parameter to the current Request's URL.
		/// </summary>
		/// <param name="request">The request containing the URL to manage.</param>
		/// <param name="parameterName">The key for the Querystring entry.</param>
		/// <param name="parameterValue">The value for the Querystring entry.</param>
		/// <returns>A URL concatenated with the new querystring parameter.</returns>
		public static string AddParameterToUrl(HttpRequest request, string parameterName, string parameterValue)
		{
			try
			{
				var qsh = new QueryStringHelper(request.Url.Query);
				qsh.AddOrReplace(parameterName, parameterValue);

				var returnUri = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port)
				{
					Path = request.Url.AbsolutePath,
					Query = qsh.GetQueryString()
				};

				return returnUri.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Removes a parameter from the current request's URL.
		/// </summary>
		/// <param name="parameterName">Parameter to be removed.</param>
		/// <returns>The updated URL.</returns>
		public static string RemoveParameterFromUrl(string parameterName)
		{
			return RemoveParameterFromUrl(HttpContext.Current.Request, parameterName);
		}

		/// <summary>
		/// Removes a parameter from the specified URL.
		/// </summary>
		/// <param name="request">The request containing the URL to manage.</param>
		/// <param name="parameterName">Parameter to be removed.</param>
		/// <returns>The updated URL.</returns>
		public static string RemoveParameterFromUrl(HttpRequest request, string parameterName)
		{
			try
			{
				var qsh = new QueryStringHelper(request.Url.Query);
				qsh.RemoveByName(parameterName);

				var returnUri = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port)
					{
						Path = request.Url.AbsolutePath,
						Query = qsh.GetQueryString()
					};

				return returnUri.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
