using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Bolt
{
    public class HttpResponseBody<T>
    {
        public bool IsValid { get; set; }
        public T Value { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; } = Array.Empty<ValidationResult>();
    }

    public static class HttpRequestExtensions
    {
        public static async Task<HttpResponseBody<T>> GetBodyAsync<T>(this HttpRequest request)
        {
            var body = new HttpResponseBody<T>();
            var bodyString = await request.ReadAsStringAsync();
            if( bodyString?.Length == 0 ){
                body.IsValid = false;
                body.ValidationResults = new List<ValidationResult>(){ new ValidationResult("Request was empty")};
            }
            body.Value = JsonConvert.DeserializeObject<T>(bodyString);

            var results = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), results, true);
            body.ValidationResults = results;
            return body;
        }

        public static BadRequestObjectResult ValidationError<T>(this HttpResponseBody<T> responseBody ){
             var problem = new ValidationProblemDetails{
                    Title = "Validation failed",
                    Detail = "One or more properties do not have expected values.",
                    Type = "https://alanta.nl/error/input-invalid",
                    Status = 400
                };

                foreach(var validation in responseBody.ValidationResults)
                {
                    problem.Errors.Add( string.Join(", ", validation.MemberNames), new []{validation.ErrorMessage} );
                }

                var error = new BadRequestObjectResult(problem);
                error.ContentTypes.Add( "application/problem+json" );
                return error;
        }
    }
}
