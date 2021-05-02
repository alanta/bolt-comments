using System;

namespace Bolt.Comments.Contract
{
    public class CommentsPerKey
    {
        public string Key {get; set;} = "";
        public Comment[] Comments {get; set;} = Array.Empty<Comment>();
    }
}