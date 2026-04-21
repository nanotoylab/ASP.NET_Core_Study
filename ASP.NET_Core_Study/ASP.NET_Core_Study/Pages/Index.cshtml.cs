using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace ASP.NET_Core_Study.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        // 画面に表示するための変数
        public string CurrentRole { get; set; } = "";
        public string CurrentTheme { get; set; } = "";

        // ① 画面が開かれた時の処理（取り出し）
        public IActionResult OnGet()
        {
            // ① Entra IDから送られてきた「メアド（またはユーザー名）」を取得する
            // ※User.Identity.Name には、SSO認証済みのメアド等が入っています
            var ssoEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(ssoEmail))
            {
                return RedirectToPage("/Error"); // 認証失敗
            }

            // ② 自社のデータベース（社員マスタ）と紐付ける
            var dbUser = "test@gmail.com";

            if (dbUser == null)
            {
                // Entra IDにはいるけど、社内システムに登録されていない人
                return RedirectToPage("/AccessDenied");
            }

            // ③ そのユーザーが持っている「権限リスト」をDBから取得する
            var userRoles = new List<string> { "Admin", "normal" };

            // ④ 権限の数に応じた「神分岐ロジック」
            if (userRoles.Count == 0)
            {
                // パターンA：権限なし ➔ エラー画面へ
                return RedirectToPage("/NoPermission");
            }
            else if (userRoles.Count == 1)
            {
                // パターンB：権限が1つだけ ➔ 自動設定してトップ画面へ直行！（UX最高）
                HttpContext.Session.SetString("Role", userRoles[0]);
                return RedirectToPage("/");
            }
            else
            {
                // パターンC：権限が複数ある ➔ ユーザーに選ばせる画面へ
                return RedirectToPage("/RoleSelect");
            }




            // セッション（ロッカー）から "Role" を取り出す
            CurrentRole = HttpContext.Session.GetString("Role") ?? "未ログイン（空っぽ）";

            // クッキー（ブラウザのメモ帳）から "Theme" を取り出す
            CurrentTheme = Request.Cookies["Theme"] ?? "ライト（デフォルト）";
        }

        // ② セッションへ保存するボタンが押された時
        public IActionResult OnPostSaveSession()
        {
            // サーバー側のロッカーに "Admin" を保存する
            HttpContext.Session.SetString("Role", "Admin");
            return RedirectToPage(); // 画面を再読み込み
        }

        // ③ クッキーへ保存するボタンが押された時
        public IActionResult OnPostSaveCookie()
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7), // 7日間有効
                HttpOnly = true
            };
            // ブラウザのクッキーに直接 "DarkMode" を書き込む
            Response.Cookies.Append("Theme", "DarkMode", options);
            return RedirectToPage();
        }

        // ④ ログアウト（リセット）ボタンが押された時
        public IActionResult OnPostClearAll()
        {
            // セッション（ロッカーの中身）を完全に破壊する
            HttpContext.Session.Clear();

            // クッキーの "Theme" を過去の日付にして強制削除する
            Response.Cookies.Append("Theme", "", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });

            return RedirectToPage();
        }

        public IActionResult OnPostCheck()
        {
            var roleValue = HttpContext.Session.GetString("Role");
            var themeValue = Request.Cookies["Theme"];

            if (roleValue != null)
            {
                // クッキーが存在する場合の処理（例：テーマが保存されていたら何かする）
                Debug.WriteLine($"現在の権限は {roleValue} です！");
            }
            else
            {
                Debug.WriteLine($"権限なし！");
            }

            if (themeValue != null)
            {
                // クッキーが存在する場合の処理（例：テーマが保存されていたら何かする）
                Debug.WriteLine($"現在のテーマは {themeValue} です！");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "他のユーザーによってデータが更新されました。画面を再読み込みしてください。");
                Debug.WriteLine($"なし！");
                return Page();
            }


            return RedirectToPage();
        }

    }
}
