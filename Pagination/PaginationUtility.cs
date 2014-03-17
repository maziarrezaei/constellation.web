namespace Constellation.Web.Pagination
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Helper class for binding multiple paginator controls at one time.
	/// </summary>
	public static class PaginationBinder
	{
		/// <summary>
		/// Helper method for binding a collection of items to 1 or more <see cref="IPaginationControl"/>.
		/// </summary>
		/// <typeparam name="T">The generic type parameter.</typeparam>
		/// <param name="collection">The items to be paginated.</param>
		/// <param name="pageSize">The number of items to be exposed per page on the paginator.</param>
		/// <param name="binder">Action for perform for populating the collection of <paramref name="collection"/>.</param>
		/// <param name="pagers">Parameter collection of <see cref="IPaginationControl"/> to bind data to.</param>
		public static void BindPagination<T>(ICollection<T> collection, int pageSize, Action<IEnumerable<T>> binder, params IPaginationControl[] pagers)
		{
			BindPagination(collection, collection.Count, pageSize, binder, pagers);
		}

		/// <summary>
		/// Helper method for binding a collection of items to 1 or more <see cref="IPaginationControl"/>.
		/// </summary>
		/// <typeparam name="T">The generic type parameter.</typeparam>
		/// <param name="items">The items to be paginated.</param>
		/// <param name="totalItems">The total count of items in the collection.</param>
		/// <param name="pageSize">The number of items to be exposed per page on the paginator.</param>
		/// <param name="binder">Action for perform for populating the collection of <paramref name="items"/>.</param>
		/// <param name="pagers">Parameter collection of <see cref="IPaginationControl"/> to bind data to.</param>
		public static void BindPagination<T>(IEnumerable<T> items, int totalItems, int pageSize, Action<IEnumerable<T>> binder, params IPaginationControl[] pagers)
		{
			var currentPage = 0;
			var paginator = new Paginator(totalItems, pageSize);

			foreach (var pager in pagers)
			{
				pager.Paginator = paginator;
				currentPage = pager.CurrentPage;
			}

			if (binder == null)
			{
				return;
			}

			var page = paginator.GetPage(items, currentPage);
			binder(page);
		}
	}
}