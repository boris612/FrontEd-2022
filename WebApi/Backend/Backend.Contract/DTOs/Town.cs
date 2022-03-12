using System.ComponentModel.DataAnnotations;

namespace Backend.Contract.DTOs
{
  public class Town : IWebApiDto
  {
    public int Id { get; set; }
    public int Postcode { get; set; }
    [Required] public string Name { get; set; }
  }
}
