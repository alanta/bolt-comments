using System;
using System.ComponentModel.DataAnnotations;

namespace Bolt.Comments.Contract
{
    public class NewComment
    {
        [Required]
        public string Name {get; set;} = "";
        
        [DataType(DataType.EmailAddress)]
        public string? Email {get; set;}
        
        [Required, DataType(DataType.MultilineText)]
        public string Content {get;set;} = "";
        
        [Required, DataType(DataType.Url)]
        public string Path { get; set; } = "";

        [DataType(DataType.Text)]
        public string? InReplyTo { get; set; }
    }

    public class ImportComment : NewComment
    {
        [DataType(DataType.Date)]
        public DateTime? Posted {get; set;}
        
        [DataType(DataType.Text)]
        public string? Id { get; internal set; }
    }
}
