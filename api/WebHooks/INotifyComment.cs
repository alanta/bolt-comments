using System.Threading;
using System.Threading.Tasks;
using Bolt.Comments.Contracts;
using Microsoft.Extensions.Logging;

namespace Bolt.Comments
{
    public interface INotifyComment
    {
        Task NotifyCommentPublished(CommentEvent message, ILogger log, CancellationToken ct);
        Task NotifyNewComment(CommentEvent message, ILogger log, CancellationToken ct);
    }
}