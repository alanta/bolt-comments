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
    public static class ListComments
    {
        [FunctionName(nameof(ListComments))]
        public static async Task<IActionResult> ListApprovedComments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approved/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            [Table("Comments")] CloudTable table,
            ILogger log)
        {


            var query =  new TableQuery<Comment>().Where(
                TableQuery.GenerateFilterConditionForBool( nameof(Comment.Approved), QueryComparisons.Equal, true )
                .AddPathQuery(path))
                .Take(250);
           
            var comments = await table.QueryAsync<Comment>(query);

            if( comments == null || comments.Count() == 0 )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(comments.Select( c => Contracts.Mapper.Map( c )).ToArray());
        }

        [FunctionName(nameof(ListCommentsForApproval))]
        public static async Task<IActionResult> ListCommentsForApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approvals/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            [Table("Comments")] CloudTable table,
            ILogger log)
        {
            TableQuery<Comment> query = new TableQuery<Comment>().Where(
                 TableQuery.GenerateFilterConditionForBool( nameof(Comment.Approved), QueryComparisons.Equal, false )
                 .AddPathQuery(path))
                 .Take(250);
           
            var comments = await table.QueryAsync<Comment>(query);

            if( comments == null || comments.Count() == 0 )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(comments.Select( c => Contracts.Mapper.Map( c )).ToArray());
        }

        private static string AddPathQuery( this string filter, string? path )
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