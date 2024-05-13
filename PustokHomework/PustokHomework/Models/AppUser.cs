using Microsoft.AspNetCore.Identity;

namespace PustokHomework.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
