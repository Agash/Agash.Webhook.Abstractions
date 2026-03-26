# Agash.Webhook.Abstractions

Transport-neutral webhook contracts for modern .NET applications and libraries.

This package keeps the core webhook handling surface intentionally small:

- request and response value types
- a normalized handler contract
- a result model for accepted, rejected, and ignored webhook processing

It is meant to be reused by platform-specific webhook libraries without forcing an ASP.NET Core dependency or a product-specific architecture.

## Package

```bash
dotnet add package Agash.Webhook.Abstractions
```

## What it provides

- `WebhookRequest` for normalized inbound webhook payloads
- `WebhookResponse` for transport-neutral HTTP-style responses
- `WebhookHandleResult` for explicit handler outcomes
- `IWebhookHandler` for library or host integrations

## Minimal example

```csharp
using Agash.Webhook.Abstractions;

public sealed class SampleWebhookHandler : IWebhookHandler
{
    public ValueTask<WebhookHandleResult> HandleAsync(WebhookRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Body.Length == 0)
        {
            return ValueTask.FromResult(
                WebhookHandleResult.Rejected(
                    WebhookResponse.BadRequest("Request body is required.")));
        }

        return ValueTask.FromResult(WebhookHandleResult.Accepted());
    }
}
```

## Design goals

- no web framework dependency
- explicit request and response modeling
- easy to compose into ASP.NET Core, worker services, desktop apps, or test hosts
- suitable as a base abstraction for higher-level webhook client libraries

## Development

```bash
dotnet test Agash.Webhook.Abstractions.slnx
```
