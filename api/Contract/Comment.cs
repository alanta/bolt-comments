using System;

namespace Bolt.Comments.Contract
{
    public class Comment 
    {
        public string Id {get; set;} = "";
        public string Key { get; set; } = "";
        public string Name {get; set;} = "";
        public string Email {get; set;} = "";
        public string Content {get;set;} = "";
        public string Markdown {get;set;} = "";
        public string Avatar {get;set;} = "";
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }
    }
}