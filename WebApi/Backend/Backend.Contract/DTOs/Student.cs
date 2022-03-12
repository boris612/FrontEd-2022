using System.ComponentModel.DataAnnotations;

namespace Backend.Contract.DTOs
{
  public class Student : IWebApiDto
  {
    public int Id { get; set; }    
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    [Required] public string Email { get; set; }
    public int SchoolId { get; set; }
    public string School { get; set; }
    public string Town { get; set; }
  }
}
