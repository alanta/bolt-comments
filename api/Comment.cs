using Microsoft.Azure.Cosmos.Table;
using System;

namespace Bolt.Comments
{
    public class Comment : TableEntity
    {
        public string Name {get; set;} = "";
        public string? Email {get; set;}
        public string Content {get;set;} = "";
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }

        public string? InReplyTo { get; set; }
    }
}
