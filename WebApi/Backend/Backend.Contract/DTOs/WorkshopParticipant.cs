using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Contract.DTOs
{
  public class WorkshopParticipant : IWebApiDto
  {
    public int Id { get; set; }            
    [Required]
    public DateTime ApplicationTime { get; set; }
    [Required]
    public int WorkshopId { get; set; }
    public string Workshop { get; set; }
    public string WorkshopSchool { get; set; }
    [Required]
    public int StudentId { get; set; }
    public string StudentSchool { get; set; }

    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
  }
}
