using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Bolt.Comments.Contracts
{
    public class CommentsPerKey
    {
        public string Key {get; set;}
        public Comment[] Comments {get; set;}
    }

    public class Comment 
    {
        public string Id {get; set;} = "";
        public string Key { get; set; } = "";
        public string Name {get; set;} = "";
        public string Email {get; set;} = "";
        public string Content {get;set;} = "";
        public string Avatar {get;set;} = "";
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }
    }

    public class CommentEvent : Comment
    {
        public string Event {get; set;} = "";
    }

    public static class Mapper
    {
        private static MD5 hasher = MD5.Create();

        private static string UniqueHash(Bolt.Comment comment)
        {
            var id = string.IsNullOrWhiteSpace(comment.Email) ? comment.Name : comment.Email;
            var hash = hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(id.ToLower()));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public static Contracts.CommentsPerKey[] Map( IReadOnlyCollection<Bolt.Comment> data)
        {
            if( data == null )
            {
                return Array.Empty<Contracts.CommentsPerKey>();
            }

            return data.Select( c => Map( c )).ToLookup( c => c.Key ).Select( set => new CommentsPerKey{
                Key = set.Key,
                Comments = set.OrderByDescending(c => c.Posted).ToArray()
            } ).ToArray();
        }

        public static Contracts.Comment Map( Bolt.Comment data )
        {
            return MapInternal<Contracts.Comment>(data);
        }

        public static Contracts.CommentEvent MapEvent( Bolt.Comment data, string eventName )
        {
            var dto = MapInternal<Contracts.CommentEvent>(data);
            dto.Event = eventName;
            return dto;
        }

        private static TComment MapInternal<TComment>( Bolt.Comment data )
            where TComment : Contracts.Comment, new()
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