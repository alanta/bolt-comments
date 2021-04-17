using System;
using System.Web;

namespace Bolt.Comments.Contracts
{
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
        public static Contracts.Comment Map( Bolt.Comment data )
        {
            return new Contracts.Comment
            {
                Id = data.RowKey,
                Key = HttpUtility.UrlDecode(data.PartitionKey),
                Name = data.Name,
                Approved = data.Approved,
                Content = data.Content,
                Email = data.Email,
                Posted = data.Posted
            };
        }
    }
}