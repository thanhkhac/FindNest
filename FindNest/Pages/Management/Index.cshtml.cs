using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Management
{

    [Authorize]
    public class Index : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}