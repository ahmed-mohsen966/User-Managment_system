using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace UserManagementSystem.ViewModels
{
	public class RoleFormViewModel
	{
		[System.ComponentModel.DataAnnotations.Required]
		[StringLength(100)]
		public string Name { get; set; }
	}
}
