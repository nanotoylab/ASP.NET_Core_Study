using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_Core_Study.Pages
{
    public class CalcModel : PageModel
    {

        [BindProperty]
        public int Number1 { get; set; }

        [BindProperty]
        public int Number2 { get; set; }

        [BindProperty]
        public int Result { get; set; }

        public void OnGet()
        {
            Number1 = 0;
            Number2 = 0;
        }

        public void Onpost()
        {
            Result = Number1 + Number2;
        }
    }
}
