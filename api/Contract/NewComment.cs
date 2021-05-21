using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bolt.Comments.Contract
{
    public class NewComment : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if( Content?.Contains("<script", StringComparison.OrdinalIgnoreCase) ?? false ){
                yield return new ValidationResult("Comment contains unsafe markup.", new string[] { nameof(Content).ToLower() });
            }

            if( !string.IsNullOrWhiteSpace(Email) && !Validation.IsValidEmail(Email)){
                yield return new ValidationResult("Invalid e-mail address.", new string[] { nameof(Email).ToLower() });
            }
        }
    }

    public class ImportComment : NewComment
    {
        [DataType(DataType.Date)]
        public DateTime? Posted {get; set;}
        
        [DataType(DataType.Text)]
        public string? Id { get; internal set; }
    }
}
