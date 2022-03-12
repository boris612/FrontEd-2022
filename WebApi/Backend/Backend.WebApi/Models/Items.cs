using System.Collections.Generic;

namespace Backend.WebApi.Models
{
  public class Items<T>
  {
    public List<T> Data { get; set; }
    public int Count { get; set; }
  }
}
