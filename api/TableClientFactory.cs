using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bolt.Comments
{
    public partial class Startup
    {
        public class TableClientFactory : ITableClientFactory
        {
            private HashSet<string> tablesTouched = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly string connectionString;

            public TableClientFactory(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public async Task<TableClient> GetTable(string tableName)
            {
                var client = new TableClient(this.connectionString, tableName);
                if (!tablesTouched.Contains(tableName))
                {
                    await client.CreateIfNotExistsAsync();
                    tablesTouched.Add(tableName);
                }

                return client;
            }
        }
    }
}