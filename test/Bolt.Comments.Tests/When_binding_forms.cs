using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using Xunit;

namespace Bolt.Comments.Tests
{
    public class When_binding_forms
    {
        [Theory,
        InlineData(null, false, "Default to JSON"),
        InlineData("application/x-www-form-urlencoded", true, "URL Encoded form is a form"),
        InlineData("multipart/form-data", true, "Multipart form is a form"),
        InlineData("text/plain", false, "Assume JSON for other content types"),
        InlineData("application/json", false, "JSON is not a form encoding")
        ]
        public void It_should_correctly_detect_form_data(string contentType, bool isForm, string because)
        {
            // Arrange
            var request = A.Fake<HttpRequest>();
            request.ContentType = contentType;

            // Act
            var result = request.IsForm();

            // Assert
            result.Should().Be(isForm, because);

        }

        [Fact]
        public void It_should_correctly_bind_all_data()
        {
            // Arrange
            //var request = new HttpRequest();

            // Act

            // Assert

        }
    }
}
