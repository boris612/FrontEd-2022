using GraphQLWebService.Models;
using HotChocolate;
using System.Threading.Tasks;

namespace GraphQLWebService.GraphQLSetup
{
  public partial class Mutations
  {
    public async Task<Town> AddTown([Service] FrontedContext ctx, TownInput input)
    {
      var item = new Town
      {
        Name = input.Name,
        Postcode = input.Postcode
      };
      ctx.Add(item);
      await ctx.SaveChangesAsync();
      return item;
    }

    public async Task<Town> UpdateTown([Service] FrontedContext ctx, int id, TownInput input)
    {
      var item = await ctx.Towns.FindAsync(id);
      if (item != null)
      {
        item.Name = input.Name;
        item.Postcode = input.Postcode;
        await ctx.SaveChangesAsync();
        return item;
      }
      else
      {
        return null;
      }
    }
    public async Task<bool> DeleteTown([Service] FrontedContext ctx, int id)
    {
      var item = await ctx.Towns.FindAsync(id);
      if (item != null)
      {
        ctx.Remove(item);
        await ctx.SaveChangesAsync();
        return true;
      }
      else
      {
        return false;
      }
    }
  }
}