using System;
using Microsoft.Azure.Cosmos.Table;

namespace Bolt
{
    public class Comment : TableEntity
    {
        public string Name {get; set;} = "";
        public string Email {get; set;} = "";
        public string Content {get;set;} = "";
        public DateTime Posted { get; set;} = DateTime.MinValue;
        public bool Approved { get; set; }
        
    }
}
