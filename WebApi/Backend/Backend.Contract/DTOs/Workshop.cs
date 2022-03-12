using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Contract.DTOs
{
  public class Workshop : IWebApiDto
  {
    public int Id { get; set; }    
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public DateTime Time { get; set; }
    [Required]
    public int SchoolId { get; set; }
    public string School { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }
    public int NoOfParticipants { get; set; }

    public int FreePlaces { get; set; }
  }
}
