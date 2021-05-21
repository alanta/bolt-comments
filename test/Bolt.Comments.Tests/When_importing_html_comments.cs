using FluentAssertions;
using Xunit;

namespace Bolt.Comments.Tests
{
    public class When_importing_html_comments
    {
        [Fact]
        public void It_should_remove_script_tags()
        {
            // Arrange
            var html = @"Hi <script>alert('boo')</script> there!";

            // Act
            var scrubbed = HtmlToMarkdown.Convert(html);

            // Assert
            scrubbed.Should().Be("Hi there!");
        }
        
        [Fact]
        public void It_should_remove_inline_tags()
        {
            // Arrange
            var html = @"Hi, <span> my name </span><strong>is</strong> slim <em>shady!</em>";

            // Act
            var scrubbed = HtmlToMarkdown.Convert(html);

            // Assert
            scrubbed.Should().Be("Hi, my name __is__ slim _shady!_");
        }
        
        [Fact]
        public void It_should_handle_links_tags()
        {
            // Arrange
            var html = @"I am <a href=""https://localhost"">the great</a> Cornholio.";

            // Act
            var scrubbed = HtmlToMarkdown.Convert(html);

            // Assert
            scrubbed.Should().Be("I am [the great](https://localhost) Cornholio.");
        }
    }
}