using System.ComponentModel.DataAnnotations;

namespace Duc_BTL.Areas.Admin.Models
{
	public class product
	{
		public int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập tên")]
    public string? Name { get; set; }
		public string? Description { get; set; }
		public string? Image { get; set; }
		public double? Price { get; set; }
		public int? Quantity { get; set; }
		public int? CategoryId { get; set; }

		public category? category { get; set; }
	}
}
