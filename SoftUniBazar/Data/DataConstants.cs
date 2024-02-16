namespace SoftUniBazar.Data
{
	public static class DataConstants
	{
		public const int AdNameMinLength = 5;
		public const int AdNameMaxLength = 25;

		public const int AdDescriptionMinLength = 15;
		public const int AdDescriptionMaxLength = 250;

		public const string DateTimeFormat = "dd-MM-yyyy H:mm";

		public const int CategoryNameMinLength = 3;
		public const int CategoryNameMaxLength = 15;

		public const string RequiredFieldErrorMessage = "The field {0} is required.";
		public const string FieldLengthErrorMessage = "The field {0} must be between {2} and {1} characters long.";
	}
}
