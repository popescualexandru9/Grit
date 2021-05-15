using Grit.Models;
using Grit.Models.Training;
using Grit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace Grit.Controllers
{
    public class FeedCalendarApiController : ApiController
    {
        private ApplicationDbContext _context;
        public FeedCalendarApiController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public FeedCalendarApi GetWorkouts(string userId)
        {
            var history = new List<DayCalendar>();

            var trainingSplits = _context.UserSplits.Where(x => x.UserID == userId).Select(x => x.Split.Id).ToList();
            var workouts = _context.Workouts.Where(x => trainingSplits.Contains(x.TrainingSplit_Id)).OrderByDescending(x => x.Date).ToList();

            var i = 0;
            foreach (var workout in workouts)
            {
                var trainingSplitName = _context.TrainingSplits.FirstOrDefault(x => x.Id == workout.TrainingSplit_Id).Name;

                // Convert DateTime to miliseconds
                var start = workout.Date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var end = start + TimeSpan.FromMinutes(Convert.ToDouble(workout.TimeSpan)).TotalMilliseconds;

                history.Add(new DayCalendar
                {
                    Id = i,
                    Name = trainingSplitName + " " + workout.Name,
                    Url = "",
                    Kind = "",
                    Start = start.ToString(),
                    End = end.ToString()
                });
                i += 1;
            }

            return new FeedCalendarApi
            {
                Success = 1,
                Result = history
            };
        }

        [HttpGet]
        [Route("feedCalendarApi/{userId}")]
        public IHttpActionResult Get(string userId)
        {
            try
            {
                var calendarTimeFrame = GetWorkouts(userId);
                return Ok(calendarTimeFrame);
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