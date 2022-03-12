using AutoMapper;
using Backend.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Controllers
{
  public class SchoolsController : CrudController<DTOs.School, EF.School, EF.School>
  {
    public SchoolsController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.School, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.School, object>>> orderSelectors = new()
    {
      [nameof(DTOs.School.Id).ToUpper()] = a => a.Id,
      [nameof(DTOs.School.Name).ToUpper()] = a => a.Name,
      [nameof(DTOs.School.TownId).ToUpper()] = a => a.TownId,
      [nameof(DTOs.School.Town).ToUpper()] = a => a.Town.Name
    };
  }
}
