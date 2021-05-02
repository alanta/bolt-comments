using Bolt.Comments.Contract;
using Bolt.Comments.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Bolt.Comments.Admin
{
    [StorageAccount("DataStorage")]
    public class UpdateSettings
    {
        private readonly SettingsService _settings;
        private readonly Authorization _authorization;

        public UpdateSettings(SettingsService settings, Authorization authorization)
        {
            _settings = settings;
            _authorization = authorization;
        }

        [FunctionName(nameof(UpdateSettings))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "settings")] HttpRequest req,
            [Table(Tables.Settings)] CloudTable table,
            ILogger log)
        {
            if (!await _authorization.IsAuthorized(req, Authorization.Roles.Admin))
            {
                return new UnauthorizedResult();
            }

            var data = await req.GetBodyAsync<Settings>();
            if (!data.IsValid)
            {
                return data.ValidationError();
            }

            var settings = await _settings.GetSettings(table);

            if( !string.IsNullOrEmpty(data.Value.ApiKey) )
            {
                settings.ApiKey = data.Value.ApiKey;
            }

            if( data.Value.WebHookNewComment != null )
            {
                // can be empty to disable webhook
                settings.WebHookNewComment = data.Value.WebHookNewComment;
            }

            if( data.Value.WebHookCommentPublished != null )
            {
                // can be empty to disable webhook
                settings.WebHookCommentPublished = data.Value.WebHookCommentPublished;
            }

            await _settings.Save(table, settings);

            return new OkResult();
        }
    }
}