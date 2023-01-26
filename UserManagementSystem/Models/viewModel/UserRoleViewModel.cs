namespace UserManagementSystem.Models.viewModel
{
    public class UserRoleViewModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
