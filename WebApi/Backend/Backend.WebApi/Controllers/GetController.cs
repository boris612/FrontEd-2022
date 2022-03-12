using AutoMapper;
using Backend.DAL.Models;
using Backend.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Backend.Contract.DTOs;
using Backend.WebApi.Util.Extensions;
using Backend.DAL;
using Backend.WebApi.Util;

namespace Backend.WebApi.Controllers
{
  [Authorize]
  public abstract class GetController<TDto, TViewEntity> : BaseApiController
    where TViewEntity : class, IHasIntegerId
    where TDto : IWebApiDto
  {
    protected readonly FrontedContext ctx;
    protected readonly IMapper mapper;

    protected GetController(FrontedContext ctx, IMapper mapper)
    {
      this.ctx = ctx;
      this.mapper = mapper;
    }

    protected abstract Dictionary<string, Expression<Func<TViewEntity, object>>> OrderSelectors { get; }    

    /// <summary>
    ///  Get page items sorted by the value from load params
    /// </summary>
    /// <param name="loadParams"></param>
    /// <returns></returns>
    [HttpGet]
    public virtual async Task<Items<TDto>> GetAll([FromQuery] LoadParams loadParams)
    {
      var result = new Items<TDto>();
      var query = ctx.Set<TViewEntity>().AsNoTracking();

      var filters = FilterStringParser.Parse(loadParams.Filter);
      if (filters != null && filters.Count > 0)
      {
        query = query.ApplyFilter(filters, OrderSelectors);
      }

      result.Count = await query.CountAsync();

      if (result.Count > 0)
      {
        var sort = loadParams?.ToSortOrder();
        if (sort != null)
        {
          query = query.ApplySort(sort, OrderSelectors);
        }
        else
        {
          query = query.OrderBy(e => e.Id);
        }

        if (loadParams.First.HasValue)
        {
          query = query.Skip(loadParams.First.Value);
        }
        if (loadParams.Rows.HasValue)
        {
          query = query.Take(loadParams.Rows.Value);
        }

        var projectedQuery = mapper.ProjectTo<TDto>(query);
        result.Data = await projectedQuery.ToListAsync();
      }
      return result;
    }

    /// <summary>
    /// Returns single item based on primary key value
    /// </summary>
    /// <param name="id"></param>    
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<ActionResult<TDto>> Get(int id)
    {
      var entity = await ctx.Set<TViewEntity>().Where(s => s.Id == id).FirstOrDefaultAsync();
      if (entity == null)
      {
        return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"No data for id = {id}");
      }
      else
      {
        var dto = mapper.Map<TDto>(entity);
        return dto;
      }
    }
  }
}