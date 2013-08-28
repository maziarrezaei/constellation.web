namespace Spark.Web
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Text;
	using System.Web;

	/// <summary>
	/// Used to parse and manipulate an HTTP querystring.
	/// </summary>
	/// <remarks>
	/// Borrowed from http://www.diplo.co.uk/blog/2010/11/4/manipulating-querystrings-in-csharp-and-net.aspx
	/// </remarks>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. StyleCop doesn't like URLs")]
	public class QueryStringHelper
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="QueryStringHelper"/> class using the passed querystring.
		/// </summary>
		/// <param name="qs">The querystring to use.</param>
		public QueryStringHelper(string qs)
		{
			this.QueryStringCollection = HttpUtility.ParseQueryString(qs);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryStringHelper"/> class using the querystring in the current HttpContext.Request.
		/// </summary>
		public QueryStringHelper()
		{
			if (HttpContext.Current == null || HttpContext.Current.Request == null)
			{
				throw new NullReferenceException("There is no HttpContext or the Request is null");
			}

			this.QueryStringCollection = new NameValueCollection(HttpContext.Current.Request.QueryString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryStringHelper"/> class from an HttpRequest.
		/// </summary>
		/// <param name="request">The HTTP request to use.</param>
		public QueryStringHelper(HttpRequest request)
		{
			this.QueryStringCollection = new NameValueCollection(request.QueryString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="QueryStringHelper"/> class.
		/// </summary>
		/// <param name="nvc">
		/// The NameValueCollection.
		/// </param>
		public QueryStringHelper(NameValueCollection nvc)
		{
			this.QueryStringCollection = nvc;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the current query string collection.
		/// </summary>
		public NameValueCollection QueryStringCollection { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Checks whether a key with the name exists.
		/// </summary>
		/// <param name="name">The name to check.</param>
		/// <returns>True if it exists or false if it doesn't.</returns>
		public bool NameExists(string name)
		{
			return !string.IsNullOrEmpty(this.GetByName(name));
		}

		/// <summary>
		/// Gets a querystring value by name.
		/// </summary>
		/// <param name="name">The name of the key to get.</param>
		/// <returns>The string value associated with the key.</returns>
		public string GetByName(string name)
		{
			return this.QueryStringCollection.Get(name);
		}

		/// <summary>
		/// Gets a querystring value and converts it to the destination type.
		/// </summary>
		/// <typeparam name="T">The type to convert it to.</typeparam>
		/// <param name="name">The name of the key to get.</param>
		/// <returns>The value of the key converted to a type of T.</returns>
		/// <exception cref="System.FormatException">Throws a System.FormatException if the type cannot be converted.</exception>
		public T GetByName<T>(string name)
		{
			return (T)Convert.ChangeType(this.GetByName(name), typeof(T));
		}

		/// <summary>
		/// Gets a querystring value and converts it to the destination type. If it cannot be converted then the defaultValue is returned instead.
		/// </summary>
		/// <typeparam name="T">The type to convert it to.</typeparam>
		/// <param name="name">The name of the key to get.</param>
		/// <param name="defaultValue">The default value to return if it cannot be converted.</param>
		/// <returns>The value of the key converted to a type of T or the default value.</returns>
		public T GetByName<T>(string name, T defaultValue)
		{
			try
			{
				return this.GetByName<T>(name);
			}
			catch (FormatException)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Adds the specified name and value to the querystring. If the value already exists it is added with a comma.
		/// </summary>
		/// <param name="name">The name to add.</param>
		/// <param name="value">The value to add.</param>
		public void Add(string name, string value)
		{
			this.QueryStringCollection.Add(name, value);
		}

		/// <summary>
		/// Adds the specified name and value (converted to a string) to the querystring. 
		/// If the value already exists it is added with a comma.
		/// </summary>
		/// <param name="name">The name to add.</param>
		/// <param name="value">The value to add.</param>
		public void Add(string name, object value)
		{
			this.Add(name, value.ToString());
		}

		/// <summary>
		/// Adds a new value or replaces it if a key with that value already exists.
		/// </summary>
		/// <param name="name">The name to add.</param>
		/// <param name="value">The value to add.</param>
		public void AddOrReplace(string name, string value)
		{
			this.QueryStringCollection.Set(name, value);
		}

		/// <summary>
		/// Adds a new value or replaces it if a key with that value already exists. The value is converted to a string.
		/// </summary>
		/// <param name="name">The name to add.</param>
		/// <param name="value">The value to add.</param>
		public void AddOrReplace(string name, object value)
		{
			this.AddOrReplace(name, value.ToString());
		}

		/// <summary>
		/// Removes the key with the name.
		/// </summary>
		/// <param name="name">The name of the key to remove.</param>
		public void RemoveByName(string name)
		{
			this.QueryStringCollection.Remove(name);
		}

		/// <summary>
		/// Removes all the keys that have a specific value.
		/// </summary>
		/// <param name="value">The value to check for.</param>
		/// <returns>A count of the keys removed.</returns>
		public int RemoveByValue(string value)
		{
			var removed = 0;

			for (var i = this.QueryStringCollection.Keys.Count - 1; i > 0; i--)
			{
				if (this.QueryStringCollection.Get(i) != value)
				{
					continue;
				}

				this.QueryStringCollection.Remove(this.QueryStringCollection.Keys[i]);
				removed++;
			}

			return removed;
		}

		/// <summary>
		/// Clears this querystring.
		/// </summary>
		public void Clear()
		{
			this.QueryStringCollection.Clear();
		}

		/// <summary>
		/// Returns a count of the keys in the querystring.
		/// </summary>
		/// <returns>A positive integer or 0 if no values.</returns>
		public int Count()
		{
			return this.QueryStringCollection.Count;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current querystring.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return this.GetQueryString();
		}

		/// <summary>
		/// Gets the querystring as a string of name and value pairs delimited with an ampersand.
		/// </summary>
		/// <returns>The querystring as a string.</returns>
		public string GetQueryString()
		{
			return this.GetQueryString(false);
		}

		/// <summary>
		/// Gets the querystring as a string of name and value pairs delimited with an ampersand.
		/// </summary>
		/// <param name="removeEmpty">If set to <c>true</c> keys with no value are removed.</param>
		/// <returns>The querystring as a string with empty keys removed.</returns>
		public string GetQueryString(bool removeEmpty)
		{
			var qs = new StringBuilder();

			var len = this.QueryStringCollection.Keys.Count;

			for (var i = 0; i < len; i++)
			{
				var val = this.QueryStringCollection.Get(i);
				if (string.IsNullOrEmpty(val) && removeEmpty)
				{
					continue;
				}

				qs.AppendFormat("{0}={1}", HttpUtility.UrlEncode(this.QueryStringCollection.GetKey(i)), HttpUtility.UrlEncode(this.QueryStringCollection.Get(i)));
				if (i < len - 1)
				{
					qs.Append("&");
				}
			}

			return qs.ToString();
		}

		/// <summary>
		/// Converts the query string NameValueCollection to a generic Dictionary for iteration.
		/// </summary>
		/// <returns>A Dictionary of string name and value pairs.</returns>
		public Dictionary<string, string> ToDictionary()
		{
			return this.QueryStringCollection.Cast<string>().Select(s => new { Key = s, Value = this.QueryStringCollection[s] }).ToDictionary(p => p.Key, p => p.Value);
		}
		#endregion
	}
}
