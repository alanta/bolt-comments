using Bolt.Comments.Contract;
using Bolt.Comments.WebHooks;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class DeleteComment
    {
        private readonly Authorization _authorization;
        private readonly INotifyComment _notifier;
        private readonly Mapper _mapper;
        private readonly ITableClientFactory _tableClientFactory;

        public DeleteComment(Authorization authorization, INotifyComment notifyComment, Mapper mapper, ITableClientFactory tableClientFactory)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            _notifier = notifyComment ?? throw new ArgumentNullException(nameof(notifyComment));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tableClientFactory = tableClientFactory ?? throw new ArgumentNullException(nameof(tableClientFactory));
        }

        [FunctionName(nameof(DeleteComment))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "comment/{id}")] HttpRequest req,
            [FromQuery] string id,
            CancellationToken ct,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Approve, Authorization.Roles.Admin))
            {
                return new UnauthorizedResult();
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestResult();
            }

            var table = await _tableClientFactory.GetTable(Tables.Comments);

            var comment = await table.QueryAsync<Comment>(c=> c.RowKey == id).FirstOrDefaultAsync();

            if( comment == null )
            {
                return new OkResult();
            }

            var result = await table.DeleteEntityAsync(comment.PartitionKey, comment.RowKey);

            if( !result.IsError && comment.Approved )
            {
                comment.Approved = false;
                await _notifier.NotifyCommentPublished(_mapper.MapEvent(comment, "Deleted"), log, ct);
            }
            
            return result.IsError ? new OkResult() : new StatusCodeResult(result.Status);
        }

    }
}
