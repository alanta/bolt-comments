using System.ComponentModel.DataAnnotations;

namespace Bolt.Comments.Contract
{
    public class Settings
    {
        [Required, MinLength(10)]
        public string? ApiKey { get; set; }
        public string? WebHookNewComment {get; set;}
        public string? WebHookCommentPublished { get; set; }
    }
}