using AutoMapper;
using Backend.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Controllers
{
  public class UsersController : CrudController<DTOs.User, EF.User, EF.User>
  {
    public UsersController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.User, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.User, object>>> orderSelectors = new Dictionary<string, Expression<Func<EF.User, object>>>
    {
      [nameof(DTOs.User.Id).ToUpper()] = a => a.UserId,
      [nameof(DTOs.User.FirstName).ToUpper()] = a => a.FirstName,
      [nameof(DTOs.User.LastName).ToUpper()] = a => a.LastName,
      [nameof(DTOs.User.UserName).ToUpper()] = a => a.Name
    };
  }
}
