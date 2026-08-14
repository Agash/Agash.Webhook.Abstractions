using System.Text;

namespace Agash.Webhook.Abstractions.Tests;

[TestClass]
public sealed class WebhookResponseTests
{
    [TestMethod]
    public void StatusCode_DefaultsTo200()
        => Assert.AreEqual(200, new WebhookResponse().StatusCode);

    [TestMethod]
    public void Headers_WhenNotSupplied_DefaultToEmpty()
        => Assert.IsEmpty(new WebhookResponse().Headers);

    [TestMethod]
    public void Empty_ProducesStatusOnlyResponse()
    {
        var response = WebhookResponse.Empty(204);

        Assert.AreEqual(204, response.StatusCode);
        Assert.IsNull(response.Body);
        Assert.IsNull(response.ContentType);
    }

    [TestMethod]
    public void PlainText_EncodesBodyAsUtf8()
    {
        var response = WebhookResponse.PlainText(200, "hello wörld");

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Body);
        Assert.AreEqual("hello wörld", Encoding.UTF8.GetString(response.Body));
    }

    [TestMethod]
    public void PlainText_SetsUtf8TextContentType()
    {
        var response = WebhookResponse.PlainText(200, "ok");

        Assert.IsNotNull(response.ContentType);
        Assert.StartsWith("text/plain", response.ContentType);
        Assert.Contains("charset=utf-8", response.ContentType);
    }

    [TestMethod]
    public void PlainText_WhenContentIsNull_Throws()
        => Assert.Throws<ArgumentNullException>(() => WebhookResponse.PlainText(200, null!));
}
