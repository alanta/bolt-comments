using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bolt.Comments
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> asyncEnumerable)
        {
            await foreach (var item in asyncEnumerable)
            {
                return item;
            }
            return default;
        }
    }
}