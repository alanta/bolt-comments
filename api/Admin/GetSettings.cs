using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using Bolt.Comments.Domain;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class GetSettings
    {
        private readonly SettingsService _settings;
        private readonly Authorization _authorization;

        public GetSettings(SettingsService settings, Authorization authorization)
        {
            _settings = settings;
            _authorization = authorization;
        }

        [FunctionName(nameof(GetSettings))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "settings")] HttpRequest req,
            [Table(Tables.Settings)] CloudTable table,
            ILogger log)
        {
            if (!await _authorization.IsAuthorized(req, Authorization.Roles.Admin))
            {
                return new UnauthorizedResult();
            }

            var settings = await _settings.GetSettings(table);

            return new OkObjectResult(new { ApiKey = settings.ApiKey, WebHookNewComment = settings.WebHookNewComment, WebHookCommentPublished = settings.WebHookCommentPublished });
        }
    }
}