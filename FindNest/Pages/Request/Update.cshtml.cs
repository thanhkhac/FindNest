using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindNest.Data;
using FindNest.Data.Models;

namespace FindNest.Pages.Request
{
    public class UpdateModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public UpdateModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ReceivedRequest ReceivedRequest { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Requests.Add(ReceivedRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
