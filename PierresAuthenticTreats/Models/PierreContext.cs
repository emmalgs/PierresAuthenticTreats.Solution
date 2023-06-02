using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PierresAuthenticTreats.Models
{
  public class PierreContext : IdentityDbContext<AppUser>
  {
    public DbSet<Treat> Treats { get; set; }
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }

    public PierreContext(DbContextOptions options) : base(options) { }
  }
}