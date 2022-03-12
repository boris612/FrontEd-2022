using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Backend.WebApi.Util.Security
{
  public class PasswordConverter : IValueConverter<KeyValuePair<string, string>, string>
  {
    private readonly IPasswordHasher<string> passwordHasher;

    public PasswordConverter(IPasswordHasher<string> passwordHasher)
    {
      this.passwordHasher = passwordHasher;
    }

    public string Convert(KeyValuePair<string, string> usernameAndPassword, ResolutionContext context)
        => passwordHasher.HashPassword(usernameAndPassword.Key, usernameAndPassword.Value);
  }
}