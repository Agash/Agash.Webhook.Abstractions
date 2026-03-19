namespace Agash.Webhook.Abstractions;

/// <summary>
/// Represents the outcome of processing a webhook request.
/// </summary>
/// <typeparam name="TEvent">
/// The normalized event type produced by the webhook handler when processing succeeds.
/// </typeparam>
/// <remarks>
/// The response is always present because webhook hosts still need to return an HTTP response
/// even when authentication fails, payloads are malformed, or the event type is unknown.
/// </remarks>
public sealed class WebhookHandleResult<TEvent>
{
    /// <summary>
    /// Gets the HTTP response that should be returned to the webhook caller.
    /// </summary>
    public required WebhookResponse Response { get; init; }

    /// <summary>
    /// Gets a value indicating whether the inbound webhook passed its authentication check.
    /// </summary>
    /// <value>
    /// <see langword="true"/> when the provider-specific authentication or verification step
    /// succeeded; otherwise <see langword="false"/>.
    /// </value>
    public bool IsAuthenticated { get; init; }

    /// <summary>
    /// Gets a value indicating whether the inbound webhook matched a known event type.
    /// </summary>
    /// <value>
    /// <see langword="true"/> when the provider-specific event type is known and intentionally
    /// mapped; otherwise <see langword="false"/>.
    /// </value>
    public bool IsKnownEvent { get; init; }

    /// <summary>
    /// Gets the normalized event produced by the handler, if any.
    /// </summary>
    /// <value>
    /// A normalized event instance when processing produced an event; otherwise
    /// <see langword="null"/>.
    /// </value>
    public TEvent? Event { get; init; }

    /// <summary>
    /// Gets an optional machine-readable or human-readable failure reason.
    /// </summary>
    /// <value>
    /// A failure description when processing did not succeed fully; otherwise
    /// <see langword="null"/>.
    /// </value>
    public string? FailureReason { get; init; }
}