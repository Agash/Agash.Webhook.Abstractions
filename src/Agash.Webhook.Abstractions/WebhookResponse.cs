using System.Collections.ObjectModel;

namespace Agash.Webhook.Abstractions;

/// <summary>
/// Represents a transport-neutral HTTP response produced by webhook handlers.
/// </summary>
/// <remarks>
/// This model is designed to be translated back into the host transport by an adapter,
/// such as ASP.NET Core or a custom listener.
/// </remarks>
public sealed class WebhookResponse
{
    private static readonly IReadOnlyDictionary<string, string[]> EmptyHeaders =
        new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));

    /// <summary>
    /// Gets the HTTP status code to return.
    /// </summary>
    /// <value>
    /// The HTTP status code for the response. The default value is <c>200</c>.
    /// </value>
    public int StatusCode { get; init; } = 200;

    /// <summary>
    /// Gets the content type of the response body, if present.
    /// </summary>
    /// <value>
    /// A media type such as <c>application/json</c> or <c>text/plain</c>, or
    /// <see langword="null"/> when the response has no body content type.
    /// </value>
    public string? ContentType { get; init; }

    /// <summary>
    /// Gets the response headers.
    /// </summary>
    /// <value>
    /// A case-insensitive mapping of header names to zero or more string values.
    /// </value>
    public IReadOnlyDictionary<string, string[]> Headers { get; init; } = EmptyHeaders;

    /// <summary>
    /// Gets the raw response body bytes, if any.
    /// </summary>
    /// <value>
    /// The response body bytes, or <see langword="null"/> when the response has no body.
    /// </value>
    public byte[]? Body { get; init; }

    /// <summary>
    /// Creates an empty response with the specified status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <returns>A new <see cref="WebhookResponse"/> instance.</returns>
    public static WebhookResponse Empty(int statusCode) =>
        new()
        {
            StatusCode = statusCode,
            Body = null,
            ContentType = null,
        };

    /// <summary>
    /// Creates a UTF-8 plain text response with the specified status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="content">The textual response body content.</param>
    /// <returns>A new <see cref="WebhookResponse"/> instance.</returns>
    public static WebhookResponse PlainText(int statusCode, string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        return new WebhookResponse
        {
            StatusCode = statusCode,
            ContentType = "text/plain; charset=utf-8",
            Body = System.Text.Encoding.UTF8.GetBytes(content),
        };
    }
}