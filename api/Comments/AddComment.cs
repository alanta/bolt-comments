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
    public class AddComment
    {
        private readonly Authorization _authorization;

        public AddComment(Authorization authorization)
        {
            _authorization = authorization;
        }

        [FunctionName(nameof(AddComment))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment")] HttpRequest req,
            [Table(Tables.Comments)] IAsyncCollector<Comment> table,
            [Queue(Queues.CommentAdded)] ICollector<Contracts.CommentEvent> queue,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.AddComment, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

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
                Name = data.Value.Name,
                InReplyTo = data.Value.InReplyTo
            };
            
            await table.AddAsync( newComment );

            queue.Add(Contracts.Mapper.MapEvent(newComment, "Added"));

            return new AcceptedResult();
        }

        [FunctionName(nameof(ImportComment))]
        public async Task<IActionResult> ImportComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/import")] HttpRequest req,
            [Table(Tables.Comments)] IAsyncCollector<Comment> table,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.ImportComment, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

            var data = await req.GetBodyAsync<Contracts.ImportComment[]>();
           
            if( !data.IsValid ){
               return data.ValidationError();
            }

            foreach( var item in data.Value )
            {
                var newComment = new Comment{
                    RowKey = item.Id ?? Guid.NewGuid().ToString("N"),
                    PartitionKey = HttpUtility.UrlEncode( item.Path ),
                    Posted = item.Posted ?? DateTime.UtcNow,
                    Content = item.Content,
                    Email = item.Email,
                    Name = item.Name
                };
                await table.AddAsync( newComment );
            }

            return new AcceptedResult();
        }
    }
}
