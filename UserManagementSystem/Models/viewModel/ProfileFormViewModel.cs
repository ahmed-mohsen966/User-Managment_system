using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models.viewModel
{
	public class ProfileFormViewModel
	{
		public string Id { get; set; }
		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
		[Display(Name = "First Name")]
		public string Fname { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
		[Display(Name = "Last Name")]
		public string Lname { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100)]
		[Display(Name = "User Name")]
		public string UserName { get; set; }
	}
}
