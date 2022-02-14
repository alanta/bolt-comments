using Azure;
using Azure.Data.Tables;
using System;

namespace Bolt.Comments.Admin
{
    public class SettingsEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        
        public string ApiKey { get; set; } = "";
        public string WebHookNewComment { get; set; } = "";
        public string WebHookCommentPublished { get; set; } = "";
    }
}