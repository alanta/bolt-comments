using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public static class AddComment
    {
        [FunctionName(nameof(AddComment))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment")] HttpRequest req,
            [Table("Comments")] IAsyncCollector<Comment> table,
            ILogger log)
        {
            var data = await req.GetBodyAsync<Contracts.NewComment>();
            
            if( !data.IsValid ){
               return data.ValidationError();
            }

            var newComment = new Comment{
                RowKey = Guid.NewGuid().ToString("N"),
                PartitionKey = HttpUtility.UrlEncode( data.Value.Path ),
                Posted = DateTime.UtcNow,
                Content = data.Value.Content,
                Email = data.Value.Email,
                Name = data.Value.Name
            };
            
            await table.AddAsync( newComment );

            return new AcceptedResult();
        }
    }
}
