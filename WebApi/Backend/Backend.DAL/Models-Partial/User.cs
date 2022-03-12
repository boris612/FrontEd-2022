namespace Backend.DAL.Models
{
  public partial class User : IHasIntegerId
  {
    public int Id => UserId;
  }
}