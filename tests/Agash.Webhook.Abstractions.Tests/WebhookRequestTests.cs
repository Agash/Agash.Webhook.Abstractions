namespace Agash.Webhook.Abstractions.Tests;

[TestClass]
public sealed class WebhookRequestTests
{
    private static WebhookRequest Build(
        string? contentType = null,
        Dictionary<string, string[]>? headers = null) =>
        new()
        {
            Method = "POST",
            Path = "/webhooks/test",
            ContentType = contentType,
            Body = [1, 2, 3],
            Headers = headers is null
                ? new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string[]>(headers, StringComparer.OrdinalIgnoreCase),
        };

    [TestMethod]
    public void Headers_WhenNotSupplied_DefaultToEmpty()
    {
        WebhookRequest request = new()
        {
            Method = "POST",
            Path = "/webhooks/test",
            Body = [],
        };

        Assert.IsEmpty(request.Headers);
    }

    [TestMethod]
    public void GetFirstHeaderValue_WhenHeaderHasValues_ReturnsFirst()
    {
        WebhookRequest request = Build(headers: new() { ["X-Signature"] = ["sig-one", "sig-two"] });

        Assert.AreEqual("sig-one", request.GetFirstHeaderValue("X-Signature"));
    }

    [TestMethod]
    public void GetFirstHeaderValue_IsCaseInsensitive()
    {
        WebhookRequest request = Build(headers: new() { ["X-Signature"] = ["sig"] });

        Assert.AreEqual("sig", request.GetFirstHeaderValue("x-signature"));
    }

    [TestMethod]
    public void GetFirstHeaderValue_WhenHeaderMissing_ReturnsNull()
    {
        Assert.IsNull(Build().GetFirstHeaderValue("X-Absent"));
    }

    [TestMethod]
    public void GetFirstHeaderValue_WhenHeaderPresentButEmpty_ReturnsNull()
    {
        WebhookRequest request = Build(headers: new() { ["X-Signature"] = [] });

        Assert.IsNull(request.GetFirstHeaderValue("X-Signature"));
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void GetFirstHeaderValue_WhenNameIsNullOrEmpty_Throws(string? name)
        => Assert.Throws<ArgumentException>(() => Build().GetFirstHeaderValue(name!));

    [TestMethod]
    [DataRow("application/json", "application/json")]
    [DataRow("APPLICATION/JSON", "application/json")]
    [DataRow("application/json; charset=utf-8", "application/json")]
    public void HasContentType_WhenPrefixMatches_ReturnsTrue(string contentType, string mediaType)
        => Assert.IsTrue(Build(contentType).HasContentType(mediaType));

    [TestMethod]
    public void HasContentType_WhenContentTypeDiffers_ReturnsFalse()
        => Assert.IsFalse(Build("text/plain").HasContentType("application/json"));

    [TestMethod]
    public void HasContentType_WhenContentTypeIsNull_ReturnsFalse()
        => Assert.IsFalse(Build().HasContentType("application/json"));
}
