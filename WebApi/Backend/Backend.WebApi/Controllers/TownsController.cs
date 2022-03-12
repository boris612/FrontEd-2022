using AutoMapper;
using Backend.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Controllers
{
  public class TownsController : CrudController<DTOs.Town, EF.Town, EF.Town>
  {
    public TownsController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.Town, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.Town, object>>> orderSelectors = new()
    {
      [nameof(DTOs.Town.Id).ToUpper()] = a => a.Id,
      [nameof(DTOs.Town.Name).ToUpper()] = a => a.Name,
      [nameof(DTOs.Town.Postcode).ToUpper()] = a => a.Postcode
    };

    protected override async Task Validate(DTOs.Town dto, bool isUpdate)
    {
      var query = ctx.Town
                     .Where(t => t.Name == dto.Name && t.Postcode == t.Postcode);
      var item = await query.FirstOrDefaultAsync();
      if (item != null)
      {
        if (!isUpdate || item.Id != dto.Id) //if such combination exists, it must be the item that is currently being updated
        {
          throw new ValidationException($"Town must have unique (postcode, name) and ({dto.Postcode}, {dto.Name}) violates that");
        }
      }
    }
  }
}
