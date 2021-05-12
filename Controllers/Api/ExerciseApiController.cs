using Grit.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Grit.Controllers
{
    [Route("exerciseapi")]
    public class ExerciseApiController : ApiController
    {
        private readonly IOpenExerciseResponse _exerciseService;

        public ExerciseApiController(IOpenExerciseResponse exerciseResponse)
        {
            _exerciseService = exerciseResponse;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            /*   if (string.IsNullOrEmpty(categoryId))
                   return BadRequest("CateogrryId parameter is missing");*/
            try
            {
                var exercises = await _exerciseService.GetExercises();
                return Ok(exercises);
            }
            catch (OpenExerciseException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    return BadRequest($"Error.");
                else
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message));
            }

        }

    }
}

