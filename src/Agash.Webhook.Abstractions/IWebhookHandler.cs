namespace Agash.Webhook.Abstractions;

/// <summary>
/// Defines a transport-neutral webhook handler that processes an inbound request and returns
/// a normalized event result plus an HTTP response.
/// </summary>
/// <typeparam name="TEvent">
/// The normalized event type produced by the handler.
/// </typeparam>
public interface IWebhookHandler<TEvent>
{
    /// <summary>
    /// Processes the specified webhook request.
    /// </summary>
    /// <param name="request">The inbound webhook request to process.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used to observe cancellation.
    /// </param>
    /// <returns>
    /// A task that completes with the processing result, including the response to return to
    /// the caller and any normalized event produced by the handler.
    /// </returns>
    Task<WebhookHandleResult<TEvent>> HandleAsync(
        WebhookRequest request,
        CancellationToken cancellationToken = default);
}