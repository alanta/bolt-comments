using System;
using System.Collections.Generic;
using System.Linq;
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
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }
        
    }

    public static class Mapper
    {
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
            return new Contracts.Comment
            {
                Id = data.RowKey,
                Key = HttpUtility.UrlDecode(data.PartitionKey),
                Name = data.Name,
                Approved = data.Approved,
                Content = data.Content,
                Email = data.Email ?? "",
                Posted = data.Posted
            };
        }
    }
}