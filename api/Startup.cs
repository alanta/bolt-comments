using Bolt.Comments.Domain;
using Bolt.Comments.WebHooks;
using Markdig;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Bolt.Comments.Startup))]

namespace Bolt.Comments
{
    public partial class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITableClientFactory, TableClientFactory>(sp => new TableClientFactory( builder.GetContext().Configuration.GetValue<string>("DataStorage")));
            builder.Services.AddScoped<SettingsService>();
            builder.Services.AddScoped<Authorization>();
            builder.Services.AddScoped<INotifyComment, NotifyComment>();
            builder.Services.AddSingleton<Contract.Mapper>();
        }
    }
}