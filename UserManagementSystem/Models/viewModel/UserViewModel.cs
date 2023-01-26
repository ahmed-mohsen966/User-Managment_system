namespace UserManagementSystem.Models.viewModel
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string Fname { get; set; }
		public string Lname { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}	
