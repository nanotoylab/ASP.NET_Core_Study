using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_Study.Pages
{
    public class ProductData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string ImageUrl { get; set; }
    }


    public class ItemListModel : PageModel
    {

        public List<ProductData> Products { get; set; } = new List<ProductData>();

        public void OnGet()
        {
            Products = new List<ProductData>
            {
                new ProductData { Id = 1, Name = "高級ワイヤレスマウス", Price = 12000, ImageUrl = "https://placehold.co/200x150/007bff/white?text=Mouse" },
                new ProductData { Id = 2, Name = "メカニカルキーボード", Price = 18000, ImageUrl = "https://placehold.co/200x150/28a745/white?text=Keyboard" },
                new ProductData { Id = 3, Name = "ウルトラワイドモニター", Price = 65000, ImageUrl = "https://placehold.co/200x150/dc3545/white?text=Monitor" }
            };
        }
    }
}
