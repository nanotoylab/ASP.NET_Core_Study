using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Study.Pages
{
    public class ItemDetailModel : PageModel
    {
        // ★ポイント1：[BindProperty(SupportsGet = true)] をつけると、
        // URLにくっついてきた "?id=〇〇" の数字を自動的にキャッチしてくれます！
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public int OrderQuantity { get; set; } = 1;

        public ProductData TargetProduct { get; set; }

        public void OnGet()
        {
            var mockDb = new List<ProductData>
            {
                new ProductData { Id = 1, Name = "高級ワイヤレスマウス", Price = 12000, ImageUrl = "https://placehold.co/400x300/007bff/white?text=Mouse" },
                new ProductData { Id = 2, Name = "メカニカルキーボード", Price = 18000, ImageUrl = "https://placehold.co/400x300/28a745/white?text=Keyboard" },
                new ProductData { Id = 3, Name = "ウルトラワイドモニター", Price = 65000, ImageUrl = "https://placehold.co/400x300/dc3545/white?text=Monitor" }
            };

            TargetProduct = mockDb?.FirstOrDefault(p => p.Id.Equals(Id));
        }


        public IActionResult OnPostOrder()
        {
            // ★ポイント2：TempDataに値をセットして、次の画面（Cart）に託す！
            TempData["SuccessMessage"] = $"商品ID：{Id} を {OrderQuantity} 個カートに入れました！";

            return RedirectToPage("/ItemDetail", new { id = Id });
        }



    }
}
