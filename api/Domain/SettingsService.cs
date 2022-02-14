using Bolt.Comments.Admin;
using System;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace Bolt.Comments.Domain
{
    public class SettingsService 
    {
        public const string TableName = "Settings";

        public SettingsService(ITableClientFactory tableClientFactory)
        {
            _tableClientFactory = tableClientFactory;
        }

        private static SettingsEntity? _settings; // using static as poormans cache
        private readonly ITableClientFactory _tableClientFactory;

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

            var table = await _tableClientFactory.GetTable(Tables.Settings);
            return await GetSettings(table);
        }

        public async Task<SettingsEntity> GetSettings(TableClient table){
            var settings = await table.QueryAsync<SettingsEntity>(maxPerPage: 1).FirstOrDefaultAsync();

            if( settings == null )
            {
                settings = InitializeSettings();
                await Save( table, settings );
            }

            return _settings = settings;
        }

        public async Task Save(SettingsEntity settings)
        {
            var table = await _tableClientFactory.GetTable(Tables.Settings);
            await Save( table, settings );
        }

        private async Task Save(TableClient table, SettingsEntity settings)
        {
            await table.UpsertEntityAsync(settings);
        }

        public SettingsEntity InitializeSettings(){
            return new SettingsEntity{
                RowKey = Guid.NewGuid().ToString("N"),
                PartitionKey = "Settings",
                ApiKey = ApiKeyGenerator.GetUniqueKey(20)
            };
        }
    }
}