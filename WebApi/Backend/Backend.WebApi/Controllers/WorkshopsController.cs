using AutoMapper;
using Backend.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Controllers
{
  public class WorkshopsController : CrudController<DTOs.Workshop, EF.ViewWorkshops, EF.Workshop>
  {
    public WorkshopsController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.ViewWorkshops, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.ViewWorkshops, object>>> orderSelectors = new Dictionary<string, Expression<Func<EF.ViewWorkshops, object>>>
    {
      [nameof(DTOs.Workshop.Id).ToUpper()] = a => a.Id,
      [nameof(DTOs.Workshop.Title).ToUpper()] = a => a.Title,
      [nameof(DTOs.Workshop.Time).ToUpper()] = a => a.Time,
      [nameof(DTOs.Workshop.Description).ToUpper()] = a => a.Description,
      [nameof(DTOs.Workshop.SchoolId).ToUpper()] = a => a.SchoolId,
      [nameof(DTOs.Workshop.School).ToUpper()] = a => a.School,
      [nameof(DTOs.Workshop.Capacity).ToUpper()] = a => a.Capacity,
      [nameof(DTOs.Workshop.NoOfParticipants).ToUpper()] = a => a.NoOfParticipants,
      [nameof(DTOs.Workshop.FreePlaces).ToUpper()] = a => a.FreePlaces
    };    
  }
}
