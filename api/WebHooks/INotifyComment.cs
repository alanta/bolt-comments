using Bolt.Comments.Contract;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bolt.Comments.WebHooks
{
    public interface INotifyComment
    {
        Task NotifyCommentPublished(CommentEvent message, ILogger log, CancellationToken ct);
        Task NotifyNewComment(CommentEvent message, ILogger log, CancellationToken ct);
    }
}