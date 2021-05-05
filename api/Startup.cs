using Bolt.Comments.Domain;
using Bolt.Comments.WebHooks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Bolt.Comments.Startup))]

namespace Bolt.Comments
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<SettingsService>();
            builder.Services.AddScoped<Authorization>();
            builder.Services.AddScoped<INotifyComment, NotifyComment>();
        }
    }
}