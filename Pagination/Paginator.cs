namespace Constellation.Web.Pagination
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents the <see cref="Paginator"/> class which encapsulates all pagination logic.
	/// </summary>
	public class Paginator
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Paginator"/> class.
		/// </summary>
		/// <param name="itemCount">The count of items in the <see cref="Paginator"/>.</param>
		/// <param name="pageSize">The page size of items to display on the <see cref="Paginator"/>.</param>
		public Paginator(int itemCount, int pageSize)
		{
			this.ItemCount = itemCount;
			this.PageSize = pageSize;
			this.SetPageCount();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Item count.
		/// </summary>
		public int ItemCount
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the page size.
		/// </summary>
		public int PageSize
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the page count.
		/// </summary>
		public int PageCount
		{
			get;
			private set;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Helper method for returning a subset of <see cref="items"/>.
		/// </summary>
		/// <typeparam name="T">Generic T of items.</typeparam>
		/// <param name="items">The items to be paginated.</param>
		/// <param name="page">The page to display.</param>
		/// <returns>A list of items that represents the current page.</returns>
		public IEnumerable<T> GetPage<T>(IEnumerable<T> items, int page)
		{
			if (page < 0 || page > this.PageCount)
			{
				return new T[0];
			}

			var startIndex = this.PageSize * page;
			return items.Skip(startIndex).Take(this.PageSize);
		}

		/// <summary>
		/// Represents a method for determining the page count.
		/// </summary>
		private void SetPageCount()
		{
			this.PageCount = this.ItemCount / this.PageSize;
			if (this.ItemCount % this.PageSize > 0)
			{
				this.PageCount += 1;
			}
		}
		#endregion
	}
}