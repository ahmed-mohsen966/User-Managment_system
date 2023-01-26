using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required,MaxLength(100)]
        public string Fname { get; set; }
        [Required, MaxLength(100)]
        public string Lname { get; set; }
        public byte[] ProfilePic { get; set; }
    }
}
