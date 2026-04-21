using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Study.Pages
{
    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
    }

    public class UserSearchModel : PageModel
    {
        [BindProperty]
        public string SearchKeyword { get; set; }

        public List<UserData> DisplayUsers { get; set; } = new List<UserData>();

        private readonly List<UserData> _mockDB = new List<UserData>
        {
            new UserData { Id = 1, Name = "山田 太郎", Department = "営業部" },
            new UserData { Id = 2, Name = "鈴木 花子", Department = "開発部" },
            new UserData { Id = 3, Name = "佐藤 次郎", Department = "総務部" },
            new UserData { Id = 4, Name = "田中 健太", Department = "開発部" }
        };

        public void OnGet()
        {
            DisplayUsers = _mockDB;
        }

        public void OnPost()
        {
            if (string.IsNullOrEmpty(SearchKeyword))
            {
                DisplayUsers = _mockDB;
            }
            else
            {
                DisplayUsers = _mockDB
                    .Where(u => u.Name.Contains(SearchKeyword) || u.Department.Contains(SearchKeyword))
                    .OrderBy(u => u.Id)
                    .ToList();
            }
        }
    }
}
