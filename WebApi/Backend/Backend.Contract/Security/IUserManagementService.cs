using System.Threading.Tasks;

namespace Backend.Contract.Security
{
  public interface IUserManagementService
  {
    Task<bool> IsValidUser(string username, string password);
    Task<UserDto> GetUserData(string username);
    Task<bool> IsValidRefreshToken(string username, string refreshToken);
    Task ReplaceRefreshToken(string username, string oldRefreshToken, string newRefreshToken, int validDays);
    Task AddRefreshToken(string username, string refreshToken, int validDays);
  }
}
