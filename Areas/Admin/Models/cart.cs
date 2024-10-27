namespace Duc_BTL.Areas.Admin.Models
{
	public class cart
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int UserId { get; set; }
		public int Quantity { get; set; }
		public double? Total { get; set; }
		public bool? Status { get; set; }
	}
}
