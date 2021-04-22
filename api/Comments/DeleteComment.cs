using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class DeleteComment
    {
        private readonly Authorization _authorization;

        public DeleteComment(Authorization authorization)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        }

        [FunctionName(nameof(DeleteComment))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "comment/{id}")] HttpRequest req,
            [FromQuery] string id,
            [Table("Comments")] CloudTable outputTable,
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

            TableQuery<Comment> rangeQuery = new TableQuery<Comment>().Where(
                 TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id))
                 .Take(1);

            
            TableContinuationToken token = new TableContinuationToken();
            var comment = (await outputTable.ExecuteQuerySegmentedAsync<Comment>(rangeQuery, token))?.FirstOrDefault();

            if( comment == null )
            {
                return new OkResult();
            }

            var deleteOp = TableOperation.Delete(comment);
            var result = await outputTable.ExecuteAsync(deleteOp);
            
            return result.HttpStatusCode < 300 ? new OkResult() : new StatusCodeResult(result.HttpStatusCode);
        }

    }
}
