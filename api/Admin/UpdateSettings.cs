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
            [Table("Settings")] CloudTable table,
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

            settings.ApiKey = data.Value.ApiKey;

            await _settings.Save(table, settings);

            return new OkResult();
        }
    }
}