using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ASP.NET_Core_Study.Tests.Web
{
    public class FeedbackIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public FeedbackIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET /Feedback: ページが正常に返る（統合）")]
        public async Task Get_Feedback_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var res = await client.GetAsync("/Feedback");

            // Assert
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            var html = await res.Content.ReadAsStringAsync();
            Assert.Contains("フィードバック", html);
        }

        [Fact(DisplayName = "POST /Feedback: フォーム送信で成功メッセージが表示される（統合）")]
        public async Task Post_Feedback_ShowsSuccessMessage()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("Email", "user@example.com"),
                new KeyValuePair<string,string>("Message", "統合テスト用の十分な長さのメッセージです。")
            });

            // Act
            var res = await client.PostAsync("/Feedback", content);

            // Assert
            // Redirect が発生する想定（RedirectToPage）
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Feedback")).StatusCode);
            var html = await (await client.GetAsync("/Feedback")).Content.ReadAsStringAsync();
            Assert.Contains("ご意見を受け付けました。ありがとうございました。", html);
        }
    }
}