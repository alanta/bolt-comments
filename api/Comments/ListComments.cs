using Bolt.Comments.Contract;
using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Linq.Expressions;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class ListComments
    {
        private readonly Authorization _authorization;
        private readonly Mapper _mapper;
        private readonly ITableClientFactory _tableClientFactory;

        public ListComments(Authorization authorization, Mapper mapper, ITableClientFactory tableClientFactory)
        {
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            _mapper = mapper;
            _tableClientFactory = tableClientFactory;
        }


        [FunctionName(nameof(ListComments))]
        public async Task<IActionResult> ListApprovedComments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approved/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Authenticated, Authorization.Roles.ListComments ))
            {
                return new UnauthorizedResult();
            }

            Expression<Func<Comment,bool>> filter = 
                string.IsNullOrWhiteSpace(path) ? 
                    c => c.Approved : 
                    c => c.Approved && string.Equals(c.PartitionKey, HttpUtility.UrlEncode(path), StringComparison.OrdinalIgnoreCase);

            var table = await _tableClientFactory.GetTable(Tables.Comments);

            var comments = await table.QueryAsync( filter, cancellationToken: cancellationToken )
                .OrderBy(c => c.Posted)
                .Take(250)
                .ToArrayAsync();

            if( comments == null || !comments.Any() )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(_mapper.Map(comments));
        }

        [FunctionName(nameof(ListCommentsForApproval))]
        public async Task<IActionResult> ListCommentsForApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "comment/approvals/{*path}")] HttpRequest req,
            [FromQuery] string? path,
            ILogger log,
            CancellationToken cancellationToken)
        {
            if( !await _authorization.IsAuthorized(req, Authorization.Roles.Authenticated ))
            {
                return new UnauthorizedResult();
            }

            Expression<Func<Comment, bool>> filter =
               string.IsNullOrWhiteSpace(path) ?
                   c => !c.Approved :
                   c => !c.Approved && string.Equals(c.PartitionKey, HttpUtility.UrlEncode(path), StringComparison.OrdinalIgnoreCase);

            var table = await _tableClientFactory.GetTable(Tables.Comments);

            var comments = await table.QueryAsync<Comment>(filter, cancellationToken: cancellationToken)
               .OrderBy(c => c.Posted)
               .Take(250)
               .ToArrayAsync();

            if( comments == null || !comments.Any() )
            {
                return new NoContentResult();
            }

            return new OkObjectResult(_mapper.Map(comments));
        }
    }
}