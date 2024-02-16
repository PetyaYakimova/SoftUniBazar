using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUniBazar.Data
{
	[Comment("Ad Buyers")]
	public class AdBuyer
	{
		[Comment("Buyer Identifier")]
		public string BuyerId { get; set; } = string.Empty;

		[ForeignKey(nameof(BuyerId))]
		public IdentityUser Buyer { get; set; } = null!;

		[Comment("Ad Identifier")]
		public int AdId { get; set; }

		[ForeignKey(nameof(AdId))]
		public Ad Ad { get; set; } = null!;
	}
}
