using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Markdig;

namespace Bolt.Comments.Contract
{
    public class Mapper
    {
        private static MD5 hasher = MD5.Create();
        private readonly MarkdownPipeline _markdownOutputPipeline;
        private readonly MarkdownPipeline _markdownInputPipeline;

        public Mapper(MarkdownPipeline markdownPipeline)
        {
            _markdownOutputPipeline =  new MarkdownPipelineBuilder()
                    .UseEmphasisExtras(Markdig.Extensions.EmphasisExtras.EmphasisExtraOptions.Default)
                    .UsePipeTables()
                    .UseAutoLinks(new Markdig.Extensions.AutoLinks.AutoLinkOptions{OpenInNewWindow = true})
                    .UseReferralLinks("nofollow")
                    .Build();

            _markdownInputPipeline =  new MarkdownPipelineBuilder()
                    .DisableHtml()
                    .Build();
        }

        private static string UniqueHash(Comments.Comment comment)
        {
            var id = string.IsNullOrWhiteSpace(comment.Email) ? comment.Name : comment.Email;
            var hash = hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(id.ToLower()));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public CommentsPerKey[] Map(IReadOnlyCollection<Comments.Comment> data)
        {
            if (data == null)
            {
                return Array.Empty<CommentsPerKey>();
            }

            return data.Select(c => Map(c)).ToLookup(c => c.Key).Select(set => new CommentsPerKey
            {
                Key = set.Key,
                Comments = set.OrderByDescending(c => c.Posted).ToArray()
            }).ToArray();
        }

        public Comment Map(Comments.Comment data)
        {
            return MapInternal<Comment>(data);
        }

        public CommentEvent MapEvent(Comments.Comment data, string eventName)
        {
            var dto = MapInternal<CommentEvent>(data);
            dto.Event = eventName;
            return dto;
        }

        private TComment MapInternal<TComment>(Comments.Comment data)
            where TComment : Comment, new()
        {
            return new TComment
            {
                Id = data.RowKey,
                Key = HttpUtility.UrlDecode(data.PartitionKey),
                Name = data.Name,
                Approved = data.Approved,
                Content = Markdown.ToHtml(data.Content, _markdownOutputPipeline),
                Markdown = data.Content,
                Email = data.Email ?? "",
                Posted = data.Posted,
                Avatar = $"https://www.gravatar.com/avatar/{UniqueHash(data)}?d=identicon"
            };
        }

        public string PurgeContent(string content)
        {
            // Expand markdown to HTML - all input is now HTML
            var html = Markdown.ToHtml(content, _markdownInputPipeline);

            // Purge HTML and convert back to Markdown
            var markdown = HtmlToMarkdown.Convert(html);

            return markdown;
        }
    }
}