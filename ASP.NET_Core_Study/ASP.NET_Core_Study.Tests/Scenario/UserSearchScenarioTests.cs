using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace ASP.NET_Core_Study.Tests.Scenario
{
    public class UserSearchScenarioTests
    {
        private const string BaseUrl = "http://localhost:5000";

        [Fact(DisplayName = "ブラウザE2E: 検索フォームで '開発部' を検索すると 2 件表示される (E2E)")]
        public async Task Browser_SearchByDepartment_ShowsTwoResults()
        {
            using var playwright = await Playwright.CreateAsync();
            // デバッグ時は Headless = false にするとブラウザが見える
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{BaseUrl}/UserSearch");

            await page.FillAsync("input[name=SearchKeyword]", "開発部");

            // クリックでページ遷移するなら、クリックとナビゲーション待ちを同時に待つ
            await Task.WhenAll(
                page.ClickAsync("button[type=submit]"),
                page.WaitForLoadStateAsync(LoadState.NetworkIdle) // or WaitForNavigationAsync()
            );

            // 必要なら特定のセレクタが現れるまで待つ
            // await page.WaitForSelectorAsync("p:has-text(\"検索結果：\")");

            var text = await page.InnerTextAsync("body");
            Assert.Contains("検索結果：2 件", text);
        }
    }
}