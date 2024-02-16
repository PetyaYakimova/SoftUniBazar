using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Data
{
	[Comment("Categories")]
	public class Category
	{
		[Key]
		[Comment("Category Identifier")]
		public int Id { get; set; }

		[Required]
		[MaxLength(CategoryNameMaxLength)]
		[Comment("Category Name")]
		public string Name { get; set; } = string.Empty;

		public IList<Ad> Ads { get; set; } = new List<Ad>();
	}
}