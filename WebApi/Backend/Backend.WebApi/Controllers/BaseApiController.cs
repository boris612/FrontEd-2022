using Backend.WebApi.Util.ExceptionFilters;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [TypeFilter(typeof(BadRequestOnRuleValidationException), Order = 20)]
  [TypeFilter(typeof(ProblemDetailsForSqlException), Order = 10)] 
  [TypeFilter(typeof(ProblemDetailsForException), Order = 1)] //last one
  public abstract class BaseApiController : ControllerBase
  {   
    //[HttpOptions]
    //[HttpOptions("{id}")]
    //public virtual void Options()
    //{
    //}
  }
}
