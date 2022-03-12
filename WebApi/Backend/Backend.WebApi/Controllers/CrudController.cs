using AutoMapper;
using Backend.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Contract.DTOs;
using Backend.DAL;

namespace Backend.WebApi.Controllers
{
  [Authorize]
  public abstract class CrudController<TDto, TViewEntity, TTableEntity> : GetController<TDto, TViewEntity>
    where TViewEntity : class, IHasIntegerId
    where TTableEntity : class, IHasIntegerId
    where TDto : IWebApiDto
  {
    protected CrudController(FrontedContext ctx, IMapper mapper) : base(ctx, mapper)
    {
    }

     protected virtual Task Validate(TDto dto, bool isUpdate) {
      return Task.CompletedTask;
    }     

    /// <summary>
    /// Creates a new item.    
    /// </summary>
    /// <param name="model">id does not have to be sent (if sent it would be ignored)</param>    
    /// <returns>A newly created item</returns>
    /// <response code="201">Returns route to newly created item and sent data updated with id</response>
    /// <response code="400">If the model is null or not valid</response>  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Create(TDto model)
    {
      await Validate(model, isUpdate: false);
      var entity = mapper.Map<TTableEntity>(model);
      ctx.Add(entity);
      await ctx.SaveChangesAsync();

      var addedItem = await Get(entity.Id);

      return CreatedAtAction(nameof(Get), new { entity.Id }, addedItem.Value);
    }

    /// <summary>
    /// Update the item
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>    
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Update(int id, TDto model)
    {
      if (model.Id != id) //ModelState.IsValid & model != null checked automatically due to [ApiController]
      {
        return Problem(statusCode: StatusCodes.Status400BadRequest, detail: $"Different ids: {id} vs {model.Id}");
      }
      else
      {
        var item = await ctx.Set<TTableEntity>().FindAsync(id);
        if (item == null)
        {
          return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
        }
        await Validate(model, isUpdate: true);
        mapper.Map(model, item);
        await ctx.SaveChangesAsync();
        return NoContent();
      }
    }

    /// <summary>
    /// Delete the item base on primary key value (id)
    /// </summary>
    /// <param name="id">Primary key value</param>    
    /// <returns></returns>
    /// <response code="204">If the item is deleted</response>
    /// <response code="404">If the item with id does not exist</response>      
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete(int id)
    {
      var item = await ctx.Set<TTableEntity>().FindAsync(id);
      if (item == null)
      {
        return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Invalid id = {id}");
      }
      ctx.Remove(item);
      await ctx.SaveChangesAsync();
      return NoContent();
    }
  }
}
