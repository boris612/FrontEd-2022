using AutoMapper;
using Backend.Contract.DTOs;
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
  public class WorkshopParticipantsController : CrudController<DTOs.WorkshopParticipant, EF.WorkshopParticipant, EF.WorkshopParticipant>
  {
    public WorkshopParticipantsController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

    protected override Dictionary<string, Expression<Func<EF.WorkshopParticipant, object>>> OrderSelectors => orderSelectors;

    private static Dictionary<string, Expression<Func<EF.WorkshopParticipant, object>>> orderSelectors = new Dictionary<string, Expression<Func<EF.WorkshopParticipant, object>>>
    {
      [nameof(DTOs.WorkshopParticipant.Id).ToUpper()] = a => a.Id,
      [nameof(DTOs.WorkshopParticipant.Name).ToUpper()] = a => a.Participant.Name,
      [nameof(DTOs.WorkshopParticipant.Surname).ToUpper()] = a => a.Participant.Surname,
      [nameof(DTOs.WorkshopParticipant.Email).ToUpper()] = a => a.Participant.Email,
      [nameof(DTOs.WorkshopParticipant.WorkshopId).ToUpper()] = a => a.WorkshopId,
      [nameof(DTOs.WorkshopParticipant.Workshop).ToUpper()] = a => a.Workshop.Title,
      [nameof(DTOs.WorkshopParticipant.StudentId).ToUpper()] = a => a.ParticipantId,
      [nameof(DTOs.WorkshopParticipant.WorkshopSchool).ToUpper()] = a => $"{a.Workshop.School.Name} {a.Workshop.School.Town.Name}",
      [nameof(DTOs.WorkshopParticipant.StudentSchool).ToUpper()] = a => $"{a.Participant.School.Name} {a.Participant.School.Town.Name}"
    };

    protected override async Task Validate(DTOs.WorkshopParticipant dto, bool isUpdate)
    {
      //check that student exists
      bool exists = await ctx.Student.AnyAsync(s => s.Id == dto.StudentId);
      if (!exists)
      {
        throw new ValidationException($"There is no student with id: {dto.StudentId}");
      }

      //check that workshop exists
      exists = await ctx.Workshop.AnyAsync(w => w.Id == dto.WorkshopId);
      if (!exists)
      {
        throw new ValidationException($"There is no workshop with id: {dto.WorkshopId}");
      }

      if (!isUpdate)
      {
        //check for free places
        int freePlaces = await ctx.ViewWorkshops
                                  .Where(w => w.Id == dto.WorkshopId)
                                  .Select(w => w.FreePlaces)
                                  .FirstAsync();
        if (freePlaces <= 0)
        {
          throw new ValidationException($"No more free places in workshop: {dto.WorkshopId}");
        }
      }

      //check that someone is not enrolled twice
      var query = ctx.WorkshopParticipant
                     .Where(p => p.WorkshopId == dto.WorkshopId && p.ParticipantId == dto.StudentId);
      var item = await query.FirstOrDefaultAsync();
      if (item != null)
      {
        if (!isUpdate || item.Id != dto.Id) //if such combination exists, it must be the item that is currently being updated
        {
          throw new ValidationException($"Student cannot enroll twice to the workshop");
        }
      }
    }
  }
}
