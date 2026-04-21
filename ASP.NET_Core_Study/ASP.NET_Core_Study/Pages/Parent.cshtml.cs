using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Study.Pages
{
    public class ParentModel : PageModel
    {
        [TempData]
        public string ReturnedMessage {  get; set; }

        public void OnGet()
        {
            //Mock
        }
    }
}
