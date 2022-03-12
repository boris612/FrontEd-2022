using GraphQLWebService.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLWebService.GraphQLSetup
{
  public class Queries
  {    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Town> AllTowns([Service] FrontedContext ctx) => ctx.Towns.AsNoTracking();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Town> GetTowns([Service] FrontedContext ctx) => ctx.Towns.AsNoTracking();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Student> GetStudents([Service] FrontedContext ctx) => ctx.Students.AsNoTracking();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Workshop> GetWorkshops([Service] FrontedContext ctx) => ctx.Workshops.AsNoTracking();

    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<School> GetSchools([Service] FrontedContext ctx) => ctx.Schools.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<WorkshopParticipant> GetParticipants(int workshopId, [Service] FrontedContext ctx) 
      => ctx.WorkshopParticipants
            .AsNoTracking()
            .Where(wp => wp.WorkshopId == workshopId);
  }
}
