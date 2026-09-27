using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace ASP.NET_Core_Study.Tests.Scenario
{
    public class FeedbackScenarioTests
    {
        // 事前にアプリを `dotnet run` で起動しておく（例: http://localhost:5000）
        private const string BaseUrl = "http://localhost:5000";

        [Fact(DisplayName = "E2E: フォームを入力して送信すると成功メッセージが出る")]
        public async Task Browser_SubmitFeedback_ShowsSuccess()
        {
            // Arrange
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            // Act
            await page.GotoAsync($"{BaseUrl}/Feedback");
            await page.FillAsync("input[name=Email]", "user@example.com");
            await page.FillAsync("textarea[name=Message]", "E2Eテスト用の十分に長いメッセージです。");

            await Task.WhenAll(
                page.ClickAsync("button[type=submit]"),
                page.WaitForLoadStateAsync(LoadState.NetworkIdle)
            );

            // Assert
            var body = await page.InnerTextAsync("body");
            Assert.Contains("ご意見を受け付けました。ありがとうございました。", body);
        }
    }
}