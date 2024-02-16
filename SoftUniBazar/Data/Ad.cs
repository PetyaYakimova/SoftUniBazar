using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Data
{
	[Comment("Ads")]
	public class Ad
	{
		[Key]
		[Comment("Ad Identifier")]
		public int Id { get; set; }

		[Required]
		[MaxLength(AdNameMaxLength)]
		[Comment("Ad Name")]
		public string Name { get; set; } = string.Empty;

		[Required]
		[MaxLength(AdDescriptionMaxLength)]
		[Comment("Ad Description")]
		public string Description { get; set; } = string.Empty;

		[Required]
		[Comment("Ad Price")]
		public decimal Price { get; set; }

		[Required]
		[Comment("Ad Owner")]
		public string OwnerId { get; set; } = string.Empty;

		[ForeignKey(nameof(OwnerId))]
		public IdentityUser Owner { get; set; } = null!;

		[Required]
		[Comment("Ad Image URL")]
		public string ImageUrl { get; set; } = string.Empty;

		[Required]
		[Comment("Ad Created On Date")]
		public DateTime CreatedOn { get; set; }

		[Required]
		[Comment("Ad Category")]
		public int CategoryId { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; } = null!;

		public IList<AdBuyer> AdsBuyers { get; set; } = new List<AdBuyer>();
	}
}
