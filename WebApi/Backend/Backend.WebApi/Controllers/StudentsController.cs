using AutoMapper;
using Backend.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Controllers
{
  public class StudentsController : CrudController<DTOs.Student, EF.Student, EF.Student>
  {
    public StudentsController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.Student, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.Student, object>>> orderSelectors = new Dictionary<string, Expression<Func<EF.Student, object>>>
    {
      [nameof(DTOs.Student.Id).ToUpper()] = a => a.Id,
      [nameof(DTOs.Student.Name).ToUpper()] = a => a.Name,
      [nameof(DTOs.Student.Surname).ToUpper()] = a => a.Surname,
      [nameof(DTOs.Student.Email).ToUpper()] = a => a.Email,
      [nameof(DTOs.Student.School).ToUpper()] = a => a.School.Name,
      [nameof(DTOs.Student.Town).ToUpper()] = a => a.School.Town.Name
    };
  }
}
