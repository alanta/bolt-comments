using Azure;
using Azure.Data.Tables;
using System;

namespace Bolt.Comments
{
    public class Comment : ITableEntity
    {
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Name {get; set;} = "";
        public string? Email {get; set;}
        public string Content {get;set;} = "";
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }

        public string? InReplyTo { get; set; }
    }
}
