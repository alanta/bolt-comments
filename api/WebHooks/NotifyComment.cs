using Bolt.Comments.Contract;
using Bolt.Comments.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bolt.Comments.WebHooks
{

    public class NotifyComment : INotifyComment
    {
        private readonly SettingsService settings;
        private static readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public NotifyComment(SettingsService settings)
        {
            this.settings = settings;
        }

        public async Task NotifyNewComment(
            CommentEvent message,
            ILogger log,
            CancellationToken ct)
        {
            try
            {
                var url = (await settings.GetSettings())?.WebHookNewComment ?? "";

                await InvokeWebHook(url, message, "new comment", ct, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to invoke webhook for new comments");
            }
        }

        public async Task NotifyCommentPublished(
            CommentEvent message,
            ILogger log,
            CancellationToken ct)
        {
            try
            {
                var url = (await settings.GetSettings())?.WebHookCommentPublished ?? "";

                await InvokeWebHook(url, message, "comment published", ct, log);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to invoke webhook for comment published");
            }
        }

        private async Task InvokeWebHook(string url, CommentEvent message, string eventName, CancellationToken ct, ILogger log)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                log.LogInformation("No webhook configured for {event}.", eventName);
                return;
            }

            if (!TryParseUri(url, out var webhookUri))
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

                if (!response.IsSuccessStatusCode)
                {
                    log.LogError("Webhook for {event} failed: {status} {reason}", eventName, response.StatusCode, response.ReasonPhrase);
                    throw new InvalidOperationException($"Failed to invoke {eventName} web hook: {response.StatusCode} {response.ReasonPhrase}");
                }
                else
                {
                    log.LogInformation("Webhook {event} at '{url}' completed with status {status} {reason}", eventName, url, response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
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