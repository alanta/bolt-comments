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
    public static class ApproveComment
    {
        // TODO : Authentication !!!
        [FunctionName(nameof(ApproveComment))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "comment/approve/{id}")] HttpRequest req,
            [FromQuery] string id,
            [Table("Comments")] CloudTable commentsTable,
            ILogger log)
        {
            if( !req.IsAuthorized(Authorization.Roles.Authenticated ))
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

            return new OkResult();
        }

    }
}
