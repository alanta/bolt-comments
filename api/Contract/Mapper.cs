using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Bolt.Comments.Contract
{
    public static class Mapper
    {
        private static MD5 hasher = MD5.Create();

        private static string UniqueHash(Comments.Comment comment)
        {
            var id = string.IsNullOrWhiteSpace(comment.Email) ? comment.Name : comment.Email;
            var hash = hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(id.ToLower()));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public static CommentsPerKey[] Map(IReadOnlyCollection<Comments.Comment> data)
        {
            if (data == null)
            {
                return Array.Empty<CommentsPerKey>();
            }

            return data.Select(c => Map(c)).ToLookup(c => c.Key).Select(set => new CommentsPerKey
            {
                Key = set.Key,
                Comments = set.OrderByDescending(c => c.Posted).ToArray()
            }).ToArray();
        }

        public static Comment Map(Comments.Comment data)
        {
            return MapInternal<Comment>(data);
        }

        public static CommentEvent MapEvent(Comments.Comment data, string eventName)
        {
            var dto = MapInternal<CommentEvent>(data);
            dto.Event = eventName;
            return dto;
        }

        private static TComment MapInternal<TComment>(Comments.Comment data)
            where TComment : Comment, new()
        {
            return new TComment
            {
                Id = data.RowKey,
                Key = HttpUtility.UrlDecode(data.PartitionKey),
                Name = data.Name,
                Approved = data.Approved,
                Content = data.Content,
                Email = data.Email ?? "",
                Posted = data.Posted,
                Avatar = $"https://www.gravatar.com/avatar/{UniqueHash(data)}?d=identicon"
            };
        }
    }
}