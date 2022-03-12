using Microsoft.AspNetCore.Mvc;
using Backend.Contract.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DAL.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backend.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]/[action]")]
  public class LookupController : ControllerBase
  {
    private readonly FrontedContext ctx;

    public LookupController(FrontedContext ctx)
    {
      this.ctx = ctx;
    }

    [HttpGet]
    public async Task<List<TextValue<int>>> Towns()
    {
      var list = await ctx.Town
                          .OrderBy(a => a.Name)
                          .Select(a => new TextValue<int>
                          {
                            Value = a.Id,
                            Text = $"{a.Name} ({a.Postcode})"
                          })
                          .ToListAsync();
      return list;
    }

    [HttpGet]
    public async Task<List<TextValue<int>>> Schools()
    {
      var list = await ctx.School
                          .OrderBy(a => a.Name)
                          .Select(a => new TextValue<int>
                          {
                            Value = a.Id,
                            Text = $"{a.Name} ({a.Town.Postcode} {a.Town.Name})"
                          })
                          .ToListAsync();
      return list;
    }

    /// <summary>
    /// Get the list of possible participants
    /// </summary>
    /// <param name="workshopId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<TextValue<int>>> Students(int workshopId)
    {     
      var list = await ctx.Student
                          .Where(s => !s.WorkshopParticipant.Where(wp => wp.WorkshopId == workshopId).Any())
                          .OrderBy(a => a.Surname)
                          .ThenBy(a => a.Name)
                          .Select(a => new TextValue<int>
                          {
                            Value = a.Id,
                            Text = $"{a.Surname}, {a.Name} ({a.Email})"
                          })
                          .ToListAsync();
      return list;
    }
  }
}
