using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresAuthenticTreats.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    [Required(ErrorMessage = "The flavor's type cannot be empty!")]
    public string Type { get; set; }
    public List<FlavorTreat> JoinEntities { get; }
    public AppUser User { get; set; }
  }
}