using System.Collections.ObjectModel;

namespace Agash.Webhook.Abstractions;

/// <summary>
/// Represents an inbound webhook request using transport-neutral data.
/// </summary>
/// <remarks>
/// This model intentionally avoids any dependency on ASP.NET Core, Kestrel, or other
/// hosting frameworks. It can be created from <c>HttpContext</c>, <c>HttpListenerContext</c>,
/// test fixtures, or any custom transport adapter.
/// </remarks>
public sealed class WebhookRequest
{
    private static readonly IReadOnlyDictionary<string, string[]> _emptyHeaders =
        new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));

    /// <summary>
    /// Gets the HTTP method used by the inbound request.
    /// </summary>
    /// <value>
    /// A non-empty HTTP method such as <c>POST</c> or <c>GET</c>.
    /// </value>
    public required string Method { get; init; }

    /// <summary>
    /// Gets the request path.
    /// </summary>
    /// <value>
    /// The absolute request path, for example <c>/webhooks/kofi/instance-id/events</c>.
    /// </value>
    public required string Path { get; init; }

    /// <summary>
    /// Gets the raw query string, if present.
    /// </summary>
    /// <value>
    /// The raw query string including or excluding the leading question mark depending on the
    /// producing transport adapter. Consumers should not assume a particular format beyond
    /// the value being the original raw query representation.
    /// </value>
    public string? QueryString { get; init; }

    /// <summary>
    /// Gets the request content type, if known.
    /// </summary>
    /// <value>
    /// The request content type such as <c>application/json</c> or
    /// <c>application/x-www-form-urlencoded</c>, or <see langword="null"/> if unknown.
    /// </value>
    public string? ContentType { get; init; }

    /// <summary>
    /// Gets the request headers.
    /// </summary>
    /// <value>
    /// A case-insensitive mapping of header names to zero or more string values.
    /// </value>
    public IReadOnlyDictionary<string, string[]> Headers { get; init; } = _emptyHeaders;

    /// <summary>
    /// Gets the raw request body bytes.
    /// </summary>
    /// <value>
    /// The raw request body payload. This value is never <see langword="null"/>.
    /// </value>
    public required byte[] Body { get; init; }

    /// <summary>
    /// Gets the first header value for the specified header name, if present.
    /// </summary>
    /// <param name="name">The header name to look up.</param>
    /// <returns>
    /// The first header value when the header exists and contains at least one value;
    /// otherwise <see langword="null"/>.
    /// </returns>
    public string? GetFirstHeaderValue(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return Headers.TryGetValue(name, out string[]? values) && values.Length > 0
            ? values[0]
            : null;
    }

    /// <summary>
    /// Determines whether the request content type matches the specified media type.
    /// </summary>
    /// <param name="mediaType">
    /// The media type to compare against, for example <c>application/json</c>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the request content type starts with the specified media type
    /// using an ordinal, case-insensitive comparison; otherwise <see langword="false"/>.
    /// </returns>
    public bool HasContentType(string mediaType)
    {
        ArgumentException.ThrowIfNullOrEmpty(mediaType);

        return ContentType is not null &&
               ContentType.StartsWith(mediaType, StringComparison.OrdinalIgnoreCase);
    }
}