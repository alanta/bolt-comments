using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class ApproveRejectComment
    {
        private readonly Authorization _authorization;

        public ApproveRejectComment(Authorization authorization)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        }

        [FunctionName(nameof(ApproveComment))]
        public async Task<IActionResult> ApproveComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/approve/{id}")] HttpRequest req,
            [FromQuery] string id,
            [Table(Tables.Comments)] CloudTable commentsTable,
            [Queue(Queues.CommentPublished)] ICollector<Contracts.CommentEvent> queue,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Approve, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestResult();
            }

            TableQuery<Comment> rangeQuery = new TableQuery<Comment>().Where(
                 TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id))
                 .Take(1);

            
            TableContinuationToken token = new TableContinuationToken();
            var comment = (await commentsTable.ExecuteQuerySegmentedAsync<Comment>(rangeQuery, token))?.FirstOrDefault();

            if( comment == null )
            {
                return new BadRequestResult();
            }

            if (comment.Approved)
            {
                return new OkResult();
            }

            comment.Approved = true;

            var updateOp = TableOperation.Replace(comment);
            await commentsTable.ExecuteAsync(updateOp);

            queue.Add(Contracts.Mapper.MapEvent(comment, "Approved"));

            return new OkResult();
        }

        [FunctionName(nameof(RejectComment))]
        public async Task<IActionResult> RejectComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/reject/{id}")] HttpRequest req,
            [FromQuery] string id,
            [Table(Tables.Comments)] CloudTable commentsTable,
            [Queue(Queues.CommentPublished)] ICollector<Contracts.CommentEvent> queue,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Approve, Authorization.Roles.Admin ))
            {
                return new UnauthorizedResult();
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestResult();
            }

            TableQuery<Comment> rangeQuery = new TableQuery<Comment>().Where(
                 TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id))
                 .Take(1);

            
            TableContinuationToken token = new TableContinuationToken();
            var comment = (await commentsTable.ExecuteQuerySegmentedAsync<Comment>(rangeQuery, token))?.FirstOrDefault();

            if( comment == null )
            {
                return new BadRequestResult();
            }

            if (!comment.Approved)
            {
                return new OkResult();
            }

            comment.Approved = false;

            var updateOp = TableOperation.Replace(comment);
            await commentsTable.ExecuteAsync(updateOp);

            queue.Add(Contracts.Mapper.MapEvent(comment, "Rejected"));

            return new OkResult();
        }
    }
}
