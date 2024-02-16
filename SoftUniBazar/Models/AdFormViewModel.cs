using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Models
{
	public class AdFormViewModel
	{
		[Required(ErrorMessage = RequiredFieldErrorMessage)]
		[StringLength(AdNameMaxLength, MinimumLength = AdNameMinLength, ErrorMessage = FieldLengthErrorMessage)]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredFieldErrorMessage)]
		[StringLength(AdDescriptionMaxLength, MinimumLength = AdDescriptionMinLength, ErrorMessage = FieldLengthErrorMessage)]
		public string Description { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredFieldErrorMessage)]
		public string ImageUrl { get; set; } = string.Empty;

		[Required(ErrorMessage = RequiredFieldErrorMessage)]
		public decimal Price { get; set; }

		[Required(ErrorMessage = RequiredFieldErrorMessage)]
		public int CategoryId { get; set; }

		public IList<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
	}
}
