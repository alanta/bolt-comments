using Bolt.Comments.Contract;
using Bolt.Comments.WebHooks;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Threading;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class AddComment
    {
        private readonly Authorization _authorization;
        private readonly INotifyComment _notifier;
        private readonly Mapper _mapper;
        private readonly ITableClientFactory _tableClientFactory;

        public AddComment(Authorization authorization, INotifyComment notifyComment, Mapper mapper, ITableClientFactory tableClientFactory)
        {
            _authorization = authorization;
            _notifier = notifyComment;
            _mapper = mapper;
            _tableClientFactory = tableClientFactory;
        }

        [FunctionName(nameof(AddComment))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment")] HttpRequest req,
            CancellationToken ct,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.AddComment, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

            var data = req.IsForm() ? await req.GetFormAsync<NewComment>() : await req.GetBodyAsync<NewComment>();
            
            if( !data.IsValid ){
               return data.ValidationError();
            }

            var newComment = new Comment{
                RowKey = Guid.NewGuid().ToString("N"),
                PartitionKey = HttpUtility.UrlEncode( data.Value!.Path ),
                Posted = DateTime.UtcNow,
                Content = _mapper.PurgeContent(data.Value.Content),
                Email = data.Value.Email,
                Name = data.Value.Name,
                InReplyTo = data.Value.InReplyTo
            };
            
            var table = await _tableClientFactory.GetTable(Tables.Comments);
            var response = await table.AddEntityAsync(newComment, ct);
            if( response.IsError )
            { 
                log.LogError("Failed to store new comment on {path}. Table client failed with status {status}", newComment.PartitionKey, response.Status);
                return new StatusCodeResult(response.Status);
            }

            await _notifier.NotifyNewComment(_mapper.MapEvent(newComment, "Added"), log, ct);

            return new AcceptedResult();
        }

        [FunctionName(nameof(ImportComment))]
        public async Task<IActionResult> ImportComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/import")] HttpRequest req,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.ImportComment, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

            var data = await req.GetBodyAsync<ImportComment[]>();
           
            if( !data.IsValid ){
               return data.ValidationError();
            }

            var table = await _tableClientFactory.GetTable(Tables.Comments);

            foreach ( var item in data.Value! )
            {
                var newComment = new Comment{
                    RowKey = item.Id ?? Guid.NewGuid().ToString("N"),
                    PartitionKey = HttpUtility.UrlEncode( item.Path ),
                    Posted = item.Posted ?? DateTime.UtcNow,
                    Content = HtmlToMarkdown.Convert(item.Content),
                    Email = item.Email,
                    Name = item.Name
                };

                var response = await table.AddEntityAsync( newComment );
                if (response.IsError)
                {
                    log.LogError("Failed to store new comment. Table client failed with status {status}", response.Status);
                }
            }

            return new AcceptedResult();
        }
    }
}
