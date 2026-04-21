// Pages/RoleSelect.cshtml.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

public class RoleSelectModel : PageModel
{
    public List<string> MyRoles { get; set; } = new List<string>();

    public void OnGet()
    {
        // DBから再度、自分の権限リストを取得して画面に渡す
        var ssoEmail = User.Identity.Name;
        // （省略：DBから userRoles を取得する処理）
        MyRoles = new List<string> { "Admin", "normal" };
    }

    // ユーザーが画面で「この権限で入る！」とボタンを押した時の処理
    public IActionResult OnPostSelectRole(string selectedRole)
    {
        // 選ばれた権限をセッション（サーバーの安全な金庫）に保存
        HttpContext.Session.SetString("Role", selectedRole);

        // トップ画面へGo！
        return RedirectToPage("/Dashboard");
    }

    public IActionResult OnPostLogout()
    {
        // 1. 自分のサーバーのセッション（金庫）を空にする
        HttpContext.Session.Clear();

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



        // 2. 「自社のWebシステムのクッキー」だけを破棄する！（ローカルログアウト）
        return SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme

        // ▼ こいつを消すことで、SSO全体のログアウト（グローバルログアウト）を防ぎます！
        // , OpenIdConnectDefaults.AuthenticationScheme 
        );
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