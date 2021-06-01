using Grit.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Grit.Services
{
    public interface IOpenExerciseResponse
    {
        Task<List<ExerciseApi>> GetExercises();
    }

    public class OpenExerciseResponse : IOpenExerciseResponse
    {
        private readonly IHttpClientFactory _httpFactory;

        public OpenExerciseResponse(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }


        public async Task<List<ExerciseApi>> GetExercises()
        {

            string url = BuildUrl(Resources.WgerApiResource, 500);
            var exercises = new List<ExerciseApi>();

            var client = _httpFactory.CreateClient("ExerciseApiClient");
            var response = await client.GetAsync(url);


            if (response.IsSuccessStatusCode)
            {
                var jsonOpts = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };
                var contentStream = await response.Content.ReadAsStreamAsync();
                var exerciseResponse = await JsonSerializer.DeserializeAsync<ExerciseResponse>(contentStream, jsonOpts);

                foreach (var exercise in exerciseResponse.ExerciseObjs)
                {
                    exercises.Add(new ExerciseApi
                    {
                        Name = exercise.Name,
                        Description = exercise.Description,
                        MuscleIds = exercise.MuscleId,
                        EquipmentIds = exercise.EquiptmentId,
                        CategoryId = exercise.CategoryId
                    });
                }

                return exercises;
            }
            else
            {
                throw new OpenExerciseException(response.StatusCode, Resources.WgerApiError + response.ReasonPhrase);
            }
        }

        private string BuildUrl(string resource, int limit)
        {
            return $"https://wger.de/api/v2/{resource}" +
                   $"?language=2&limit={limit}";
        }

    }
}

