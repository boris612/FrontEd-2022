using AutoMapper;
using Backend.WebApi.Util.Security;
using System.Collections.Generic;
using DTOs = Backend.Contract.DTOs;
using EF = Backend.DAL.Models;

namespace Backend.WebApi.Mappings
{
  public class EFMappingProfile : Profile
  {
    public EFMappingProfile()
    {
      CreateMap<DTOs.Town, EF.Town>();
      CreateMap<EF.Town, DTOs.Town>();


      CreateMap<DTOs.School, EF.School>()
        .ForMember(e => e.Town, opt => opt.Ignore()); //as they both have Town, but with different meaning (town name vs entity)
      CreateMap<EF.School, DTOs.School>()
        .ForMember(dto => dto.Town, opt => opt.MapFrom(e => $"{e.Town.Name} ({e.Town.Postcode})"));

   
      CreateMap<DTOs.Student, EF.Student>()
        .ForMember(e => e.School, opt => opt.Ignore());  //as they both have School, but with different meaning
      CreateMap<EF.Student, DTOs.Student>()
        .ForMember(dto => dto.School, opt => opt.MapFrom(e => e.School.Name))
        .ForMember(dto => dto.Town, opt => opt.MapFrom(e => $"{e.School.Town.Postcode} {e.School.Town.Name}"));

    
      CreateMap<DTOs.Workshop, EF.Workshop>()
        .ForMember(e => e.School, opt => opt.Ignore());
      CreateMap<EF.ViewWorkshops, DTOs.Workshop>();        


      CreateMap<DTOs.WorkshopParticipant, EF.WorkshopParticipant>()
        .ForMember(e => e.Workshop, opt => opt.Ignore())
        .ForMember(e => e.ParticipantId, opt => opt.MapFrom(dto => dto.StudentId));
      CreateMap<EF.WorkshopParticipant, DTOs.WorkshopParticipant>()
        .ForMember(dto => dto.Name, opt => opt.MapFrom(e => e.Participant.Name))
        .ForMember(dto => dto.Surname, opt => opt.MapFrom(e => e.Participant.Surname))
        .ForMember(dto => dto.Email, opt => opt.MapFrom(e => e.Participant.Email))
        .ForMember(dto => dto.Workshop, opt => opt.MapFrom(e => e.Workshop.Title))        
        .ForMember(dto => dto.WorkshopSchool, opt => opt.MapFrom(e => $"{e.Workshop.School.Name} {e.Workshop.School.Town.Name}"))
        .ForMember(dto => dto.StudentSchool, opt => opt.MapFrom(e => $"{e.Participant.School.Name} {e.Participant.School.Town.Name}"));
     
      CreateMap<DTOs.User, EF.User>()
        .ForMember(e => e.UserId, opt => opt.MapFrom(dto => dto.Id))        
        .ForMember(e => e.Name, opt => opt.MapFrom(dto => dto.UserName))
        .ForMember(e => e.Password, opt =>
        {
          opt.PreCondition(dto => dto.ChangePassword);
          opt.ConvertUsing<PasswordConverter,  KeyValuePair<string, string>> (dto => KeyValuePair.Create(dto.UserName, dto.Password));
        });
      CreateMap<EF.User, DTOs.User>()
        .ForMember(dto => dto.UserName, opt => opt.MapFrom(e => e.Name))
        .ForMember(dto => dto.Password, opt => opt.Ignore());      
    }
  }
}
