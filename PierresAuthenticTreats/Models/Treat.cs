using System.Collections.Generic;

namespace PierresAuthenticTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public List<FlavorTreat> JoinEntities { get; }
    public AppUser User { get; set; }
  }
}