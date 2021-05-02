using Bolt.Comments.Admin;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bolt.Comments.Domain
{
    public class SettingsService 
    {
        public const string TableName = "Settings";
        public const string Connection = "DataStorage";

        public SettingsService(IConfiguration configuration, ILogger<SettingsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private static SettingsEntity? _settings; // using static as poormans cache
        private readonly IConfiguration _configuration;
        private readonly ILogger<SettingsService> _logger;

        public async Task<ApiKey[]> GetApiKeys()
        {
            var settings = await GetSettings();

            return new[]{
                new ApiKey{
                    Key = settings.ApiKey,
                    Name = "Bolt API",
                    UserId = "boltapi",
                    Roles = new[]{ Authorization.Roles.AddComment, Authorization.Roles.ListComments }
                }
            };
        }

        public void Update( SettingsEntity settings ){
            _settings = settings;
        }

        public async Task<SettingsEntity> GetSettings()
        {
            if( _settings != null )
            {
                return _settings;
            }

            var table = await GetTable(TableName);

            return await GetSettings(table);
        }

        public async Task<SettingsEntity> GetSettings(CloudTable table){
            var query =  new TableQuery<SettingsEntity>().Take(1);
            var settings = (await table.QueryAsync<SettingsEntity>(query)).FirstOrDefault();

            if( settings == null )
            {
                settings = InitializeSettings();
                await Save( table, settings );
            }

            return _settings = settings;
        }

        public async Task Save(CloudTable table, SettingsEntity settings)
        {
            var updateOp = TableOperation.InsertOrReplace(settings);
            await table.ExecuteAsync(updateOp);
        }

        public SettingsEntity InitializeSettings(){
            return new SettingsEntity{
                RowKey = Guid.NewGuid().ToString("N"),
                PartitionKey = "Settings",
                ApiKey = ApiKeyGenerator.GetUniqueKey(20)
            };
        }

        private CloudStorageAccount CreateStorageAccount(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                _logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                _logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }

            return storageAccount;
        }

        private async Task<CloudTable> GetTable(string tableName)
        {
            string storageConnectionString = _configuration.GetValue<string>(Connection);

            CloudStorageAccount storageAccount = CreateStorageAccount(storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}