using System.Linq;
using Xunit;
using ASP.NET_Core_Study.Pages;

namespace ASP.NET_Core_Study.Tests
{
    public class UserSearchTests
    {
        // ヘルパー：モデルを作成し、必要なハンドラを呼ぶ（Arrange の共通化）
        private static UserSearchModel CreateModel(string? keyword = null)
        {
            var model = new UserSearchModel();
            if (keyword is not null) model.SearchKeyword = keyword;
            return model;
        }

        [Fact(DisplayName = "OnGet: すべてのユーザーを返す")]
        public void OnGet_ReturnsAllUsers()
        {
            // Arrange: モデルを作る
            var model = CreateModel();

            // Act: OnGet ハンドラを呼ぶ（画面初期表示）
            model.OnGet();

            // Assert: 期待する件数が返ること
            Assert.Equal(4, model.DisplayUsers.Count);
        }

        [Fact(DisplayName = "OnPost: 部署キーワードでフィルタリングされる")]
        public void OnPost_WithDepartmentKeyword_FiltersUsers()
        {
            // Arrange
            var model = CreateModel("開発部");

            // Act
            model.OnPost();

            // Assert
            Assert.Equal(2, model.DisplayUsers.Count);
            Assert.All(model.DisplayUsers, u => Assert.Contains("開発部", u.Department));
        }

        [Fact(DisplayName = "OnPost: 名前キーワードでフィルタリングされる")]
        public void OnPost_WithNameKeyword_FiltersUsers()
        {
            // Arrange
            var model = CreateModel("山田");

            // Act
            model.OnPost();

            // Assert
            Assert.Single(model.DisplayUsers);
            Assert.Equal("山田 太郎", model.DisplayUsers.First().Name);
        }

        [Fact(DisplayName = "OnPost: キーワード未指定で全件返す")]
        public void OnPost_EmptyKeyword_ReturnsAll()
        {
            // Arrange & Act
            var model = CreateModel(string.Empty);

            // Assert
            Assert.Equal(4, model.DisplayUsers.Count);
        }
    }
}
