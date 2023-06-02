using System.Collections.Generic;

namespace PierresAuthenticTreats.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    public string Type { get; set; }
    public List<FlavorTreat> JoinEntities { get; }
  }
}