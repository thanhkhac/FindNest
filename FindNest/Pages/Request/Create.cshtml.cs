using FindNest.Data;
using FindNest.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Develop
{
    public class Create : PageModel
    {
        private readonly FindNestDbContext _dbContext;

        public Create(FindNestDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult OnGet()
        {
            string value = String.Join("|", Request.Query.Select(kv => $"{kv.Key}: {kv.Value}"));
            ReceivedRequest receivedRequest = new ReceivedRequest { Value = value };
            _dbContext.Requests.Add(receivedRequest);
            _dbContext.SaveChanges();
            return RedirectToPage("./Index");
        }
        public IActionResult OnPost()
        {
            string value = String.Join("|", Request.Form.Select(kv => $"{kv.Key}: {kv.Value}"));
            ReceivedRequest receivedRequest = new ReceivedRequest { Value = value };
            _dbContext.Requests.Add(receivedRequest);
            _dbContext.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}