using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ASP.NET_Core_Study.Tests.Web
{
    public class UserSearchIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserSearchIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact(DisplayName = "GET /UserSearch: ページロードで 200 が返る (統合)")]
        public async Task Get_UserSearch_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var res = await client.GetAsync("/UserSearch");
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            var html = await res.Content.ReadAsStringAsync();
            Assert.Contains("社員検索システム", html);
        }

        [Fact(DisplayName = "POST /UserSearch: フォーム送信で結果がフィルタされる (統合)")]
        public async Task Post_UserSearch_FiltersResults()
        {
            var client = _factory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("SearchKeyword", "開発部")
            });
            var res = await client.PostAsync("/UserSearch", content);
            var html = await res.Content.ReadAsStringAsync();
            Assert.Contains("検索結果：2 件", html); // Razor の出力に合わせて確認
        }
    }
}