using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.WebApi.Models
{
  public class Tokens
  {
    [Required]
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [Required]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
  }
}
