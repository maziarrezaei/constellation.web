namespace Constellation.Web.Pagination
{
	/// <summary>
	/// Contract for use with Pagination Controls.
	/// </summary>
	public interface IPaginationControl
	{
		/// <summary>
		/// Gets or sets the paginator that handles pagination logic.
		/// </summary>
		Paginator Paginator { get; set; }

		/// <summary>
		/// Gets the current page, resolved by the pagination control.
		/// </summary>
		int CurrentPage
		{
			get;
		}
	}
}
