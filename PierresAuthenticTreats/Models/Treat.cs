using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PierresAuthenticTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "Treat must have a name!")]
    public string Name { get; set; }
    public float Price { get; set; }
    public List<FlavorTreat> JoinEntities { get; }
    public AppUser User { get; set; }
  }
}