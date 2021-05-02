using Bolt.Comments.Contract;
using System;
using System.Web;
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
    public class ListComments
    {
        private readonly Authorization _authorization;

        public ListComments(Authorization authorization)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        }


        [FunctionName(nameof(ListComments))]
        public async Task<IActionResult> ListApprovedComments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approved/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            [Table(Tables.Comments)] CloudTable table,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Authenticated, Authorization.Roles.ListComments ))
            {
                return new UnauthorizedResult();
            }

            var query =  new TableQuery<Comment>().Where(
                TableQuery.GenerateFilterConditionForBool( nameof(Comment.Approved), QueryComparisons.Equal, true )
                .AddPathQuery(path))
                .OrderBy(nameof(Comment.PartitionKey))
                .Take(250);
           
            var comments = await table.QueryAsync<Comment>(query);

            if( comments == null || comments.Count() == 0 )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(Mapper.Map(comments));
        }

        [FunctionName(nameof(ListCommentsForApproval))]
        public async Task<IActionResult> ListCommentsForApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approvals/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            [Table(Tables.Comments)] CloudTable table,
            ILogger log)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Authenticated ))
            {
                return new UnauthorizedResult();
            }

            TableQuery<Comment> query = new TableQuery<Comment>().Where(
                 TableQuery.GenerateFilterConditionForBool( nameof(Comment.Approved), QueryComparisons.Equal, false )
                 .AddPathQuery(path))
                 .OrderBy(nameof(Comment.Posted))
                 .Take(250);
           
            var comments = await table.QueryAsync<Comment>(query);

            if( comments == null || comments.Count() == 0 )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(Mapper.Map(comments));
        }
    }

    public static class TableQueryHelper
    {
        public static string AddPathQuery( this string filter, string? path )
        {
            if (string.IsNullOrEmpty(path))
            {
                return filter;
            }

            return TableQuery.CombineFilters(
                filter,
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(Comment.PartitionKey), QueryComparisons.Equal, HttpUtility.UrlEncode( path )));

        }
    }

}