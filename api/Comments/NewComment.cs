using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bolt.Comments
{
    public class NewComment
    {
        [Required]
        public string Name {get; set;} = "";
        [Required, DataType(DataType.EmailAddress)]
        public string Email {get; set;} = "";
        [Required, DataType(DataType.MultilineText)]
        public string Content {get;set;} = "";
        [Required, DataType(DataType.Url)]
        public string Path { get; set; } = "";
    }
}
