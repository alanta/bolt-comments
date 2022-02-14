using Azure.Data.Tables;
using System.Threading.Tasks;

namespace Bolt.Comments
{
    public interface ITableClientFactory
    {
        Task<TableClient> GetTable(string tableName);        
    }
}