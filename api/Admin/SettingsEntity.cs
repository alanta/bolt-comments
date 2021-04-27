using Microsoft.Azure.Cosmos.Table;

namespace Bolt.Comments
{
    public class SettingsEntity : TableEntity
    {
        public string ApiKey { get; set; } = "";
        public string WebHookNewComment { get; set; } = "";
        public string WebHookCommentPublished { get; set; } = "";
    }
}