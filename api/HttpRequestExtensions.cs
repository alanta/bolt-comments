using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bolt.Comments
{
    public class HttpResponseBody<T>
    {
        public bool IsValid { get; set; }
        public T? Value { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; } = Array.Empty<ValidationResult>();
    }

    public static class HttpRequestExtensions
    {
        private static readonly string[] formContentTypes = new[]{ "application/x-www-form-urlencoded", "multipart/form-data" };

        public static bool IsForm(this HttpRequest request) => formContentTypes.Any( contentType =>  request.ContentType?.StartsWith(contentType, StringComparison.OrdinalIgnoreCase) ?? false);
        
        /// <summary>
        /// Attempts to bind a form collection to a model of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The model type. Must have a public parameterless constructor.</typeparam>
        /// <param name="form">The form data to bind.</param>
        /// <param name="request">The request.</param>
        /// <returns>A new instance of type <typeparamref name="T" /> containing the form data.</returns>
        public static async Task<HttpResponseBody<T>> GetFormAsync<T>(this HttpRequest request) where T : new()
        {
            // based on https://stackoverflow.com/a/60284398/64096
            var body = new HttpResponseBody<T>();
            var form = await request.ReadFormAsync();
            if( form == null || form?.Count == 0 ){
                body.IsValid = false;
                body.ValidationResults = new List<ValidationResult>(){ new ValidationResult("Request was empty")};
            }

            var props = typeof(T).GetProperties().ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            var instance = body.Value = Activator.CreateInstance<T>();

            foreach (var key in form!.Keys)
            {
                if (!props.TryGetValue(key, out var prop) || !prop.CanWrite)
                {
                    continue;
                }

                // assuming single values and primitive values only
                var value = form[key].FirstOrDefault(); 
                prop.SetValue(instance, Convert.ChangeType(value, prop.PropertyType) );
            }

            var results = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value!, new ValidationContext(body.Value!, null, null),
                    results, true);
            body.ValidationResults = results;

            return body;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
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
            body.IsValid = Validator.TryValidateObject(body.Value!, new ValidationContext(body.Value!, null, null),
                    results, true);
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
