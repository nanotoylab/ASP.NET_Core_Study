using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Study.Pages
{

    public class ChildModel : PageModel
    {
        // ユーザーが入力するデータ
        [BindProperty]
        public string InputText { get; set; }

        public void OnGet()
        {
        }

        // 決定ボタンが押された時の処理
        public IActionResult OnPost()
        {
            // ★ポイント：TempDataに値をセットして、親画面に託す！
            TempData["ReturnedMessage"] = InputText;

            // 親画面（Parent）にリダイレクト（強制移動）する
            return RedirectToPage("/Parent");
        }
    }
}
