using Bolt.Comments.Contract;
using Bolt.Comments.WebHooks;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class ApproveRejectComment
    {
        private readonly Authorization _authorization;
        private readonly INotifyComment _notifier;
        private readonly Mapper _mapper;
        private readonly ITableClientFactory _tableClientFactory;

        public ApproveRejectComment(Authorization authorization, INotifyComment notifyComment, Mapper mapper, ITableClientFactory tableClientFactory)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            _notifier = notifyComment ?? throw new ArgumentNullException(nameof(notifyComment));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tableClientFactory = tableClientFactory ?? throw new ArgumentNullException(nameof(tableClientFactory));
        }

        [FunctionName(nameof(ApproveComment))]
        public async Task<IActionResult> ApproveComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/approve/{id}")] HttpRequest req,
            [FromQuery] string id,
            CancellationToken ct,
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

            var table = await _tableClientFactory.GetTable(Tables.Comments);
            var comment = await table.QueryAsync<Comment>(c => c.RowKey == id).FirstOrDefaultAsync();

            if( comment == null )
            {
                return new BadRequestResult();
            }

            if (comment.Approved)
            {
                return new OkResult();
            }

            comment.Approved = true;

            var response = await table.UpdateEntityAsync(comment, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace, cancellationToken: ct);
            if (response.IsError)
            {
                log.LogError("Failed to approve comment {id}. Table client failed with status {status}", id, response.Status);
            }

            await _notifier.NotifyCommentPublished(_mapper.MapEvent(comment, "Approved"), log, ct);

            return new OkResult();
        }

        [FunctionName(nameof(RejectComment))]
        public async Task<IActionResult> RejectComment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/reject/{id}")] HttpRequest req,
            [FromQuery] string id,
            CancellationToken ct,
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

            var table = await _tableClientFactory.GetTable(Tables.Comments);
            var comment = await table.QueryAsync<Comment>(c => c.RowKey == id).FirstOrDefaultAsync();

            if ( comment == null )
            {
                return new BadRequestResult();
            }

            if (!comment.Approved)
            {
                return new OkResult();
            }

            comment.Approved = false;

            var response = await table.UpdateEntityAsync(comment, Azure.ETag.All, Azure.Data.Tables.TableUpdateMode.Replace, cancellationToken: ct);
            if (response.IsError)
            {
                log.LogError("Failed to approve comment {id}. Table client failed with status {status}", id, response.Status);
            }

            await _notifier.NotifyCommentPublished(_mapper.MapEvent(comment, "Rejected"), log, ct);

            return new OkResult();
        }
    }
}
