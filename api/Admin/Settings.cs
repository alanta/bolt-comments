using System.ComponentModel.DataAnnotations;

namespace Bolt.Comments
{
    public class Settings
    {
        [Required, MinLength(10)]
        public string ApiKey { get; set; } = "";
    }
}