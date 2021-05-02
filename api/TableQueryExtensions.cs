using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bolt.Comments
{
    public static class TableQueryExtensions
    {
        /// <summary>
        /// Get rows from an Azure Storage Table.
        /// </summary>
        /// <typeparam name="TEntity">type of entity, extending Microsoft.Azure.Cosmos.Table.TableEntity</typeparam>
        /// <param name="table">the CloudTable, e.g. _tableClient.GetTableReference("table_name");</param>
        /// <param name="tableQuery">a TableQuery object, e.g. for filter, select</param>
        /// <returns></returns>
        public static async Task<IReadOnlyList<TEntity>> QueryAsync<TEntity>(
            this CloudTable table,
            TableQuery<TEntity> tableQuery) where TEntity : ITableEntity, new()
        {
            List<TEntity> results = new List<TEntity>();
            TableContinuationToken? continuationToken = new TableContinuationToken();
            do
            {
                var queryResults = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }
    }
}