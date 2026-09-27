using System.Linq;
using Xunit;
using ASP.NET_Core_Study.Pages;

namespace ASP.NET_Core_Study.Tests.Application
{
    public class UserSearchUnitTests
    {
        private static UserSearchModel CreateModel(string? keyword = null, bool callPost = true)
        {
            var model = new UserSearchModel();
            if (keyword is not null) model.SearchKeyword = keyword;
            if (callPost) model.OnPost(); else model.OnGet();
            return model;
        }

        [Fact(DisplayName = "OnGet: すべてのユーザーを返す (単体)")]
        public void OnGet_ReturnsAllUsers()
        {
            var model = CreateModel(callPost: false);
            Assert.Equal(4, model.DisplayUsers.Count);
        }

        [Fact(DisplayName = "OnPost: 部署キーワードでフィルタリングされる (単体)")]
        public void OnPost_WithDepartmentKeyword_FiltersUsers()
        {
            var model = CreateModel("開発部");
            Assert.Equal(2, model.DisplayUsers.Count);
            Assert.All(model.DisplayUsers, u => Assert.Contains("開発部", u.Department));
        }
    }
}