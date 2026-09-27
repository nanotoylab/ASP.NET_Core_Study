using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Study.Pages
{
    public class FeedbackModel : PageModel
    {
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [MinLength(10)]
        public string Message { get; set; }

        // テストや処理確認用に公開プロパティを追加（単体テストで参照）
        public bool SubmissionSucceeded { get; private set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // 初期表示時の既定値（任意）
            Email = string.Empty;
            Message = string.Empty;
            SubmissionSucceeded = false;
        }

        public IActionResult OnPost()
        {
            // バリデーションを簡潔に行う（属性による検証はモデルバインディング時に通常実行されるが、
            // 単体テストでは PageModel を直接作ることが多いため手動でも検査）
            if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
            {
                ModelState.AddModelError(nameof(Email), "正しいメールアドレスを入力してください。");
            }

            if (string.IsNullOrWhiteSpace(Message) || Message.Length < 10)
            {
                ModelState.AddModelError(nameof(Message), "メッセージは10文字以上で入力してください。");
            }

            if (!ModelState.IsValid)
            {
                SubmissionSucceeded = false;
                return Page();
            }

            // 正常処理（例：DB 保存などの代わりにメッセージを設定）
            SubmissionSucceeded = true;
            SuccessMessage = "ご意見を受け付けました。ありがとうございました。";

            // 実アプリでは RedirectToPage 後に確認画面へ遷移することが多い
            return RedirectToPage();
        }
    }
}