using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Bolt.Comments.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bolt.Comments
{
    [StorageAccount("DataStorage")]
    public class NotifyComment
    {
        private readonly SettingsService settings;
        private static readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions{
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = false,
        };

        public NotifyComment(SettingsService settings)
        {
            this.settings = settings;
        }

        [FunctionName(nameof(NotifyNewComment))]
        [ExponentialBackoffRetry(10, "00:00:20", "00:01:00")]
        public async Task NotifyNewComment(
            [QueueTrigger(Queues.CommentAdded)] Contracts.CommentEvent message,
            ILogger log,
            CancellationToken ct)
        {
            var url = (await settings.GetSettings())?.WebHookNewComment ?? "";

            await InvokeWebHook(url, message, "new comment", ct, log);
        }

        [FunctionName(nameof(NotifyCommentPublished))]
        [ExponentialBackoffRetry(10, "00:00:20", "00:01:00")]
        public async Task NotifyCommentPublished(
            [QueueTrigger(Queues.CommentPublished)] Contracts.CommentEvent message,
            ILogger log,
            CancellationToken ct)
        {
            var url = (await settings.GetSettings())?.WebHookCommentPublished ?? "";

            await InvokeWebHook(url, message, "comment approved", ct, log);
        }

        private async Task InvokeWebHook(string url, Contracts.CommentEvent message, string eventName, CancellationToken ct, ILogger log)
        {
            if( string.IsNullOrWhiteSpace(url))
            {
                log.LogWarning("No webhook configured for {event}.", eventName);
                return;
            }

            if( !TryParseUri(url, out var webhookUri ))
            {
                log.LogWarning("Webhook for {event} is invalid: '{url}'", eventName, url);
                throw new InvalidOperationException($"Webhook URI for {eventName} is invalid.");
            }

            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(message, serializationOptions);

                using var client = new HttpClient();
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, webhookUri)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                log.LogDebug("Invoking {event} webhook at '{url}' with body {body}", eventName, url, json);
                var response = await client.SendAsync(httpRequest, ct);

                if( !response.IsSuccessStatusCode )
                {
                    log.LogError("Webhook for {event} failed: {status} {reason}", eventName, response.StatusCode, response.ReasonPhrase);
                    throw new InvalidOperationException($"Failed to invoke {eventName} web hook: {response.StatusCode} {response.ReasonPhrase}");
                }
                else
                {
                    log.LogInformation("Webhook {event} at '{url}' completed with status {status} {reason}", eventName, url, response.StatusCode, response.ReasonPhrase);
                }
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Webhook for {event} failed.", eventName);
                throw new InvalidOperationException($"Failed to invoke {eventName} web hook.", ex);
            }
        }

        private bool TryParseUri(string uri, out Uri parsed)
        {
            try
            {
                parsed = new Uri(uri);
                return true;
            }
            catch
            {
                parsed = new Uri("https://localhost");
                return false;
            }
        }
    }
}