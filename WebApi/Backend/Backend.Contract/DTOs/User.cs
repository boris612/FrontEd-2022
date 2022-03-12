using System.ComponentModel.DataAnnotations;

namespace Backend.Contract.DTOs
{
  public class User : IWebApiDto
  {
    public int Id { get; set; }    
    [Required] public string UserName { get; set; }
    
    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }
    
    public bool ChangePassword { get; set; }

    public string Password { get; set; }    
  }
}
