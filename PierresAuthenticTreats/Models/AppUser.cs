using Microsoft.AspNetCore.Identity;

namespace PierresAuthenticTreats.Models
{
  public class AppUser : IdentityUser
  {
    public float OrderTotal { get; set; }
  }
}