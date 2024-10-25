using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindNest.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindNest.Data;
using FindNest.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace FindNest.Pages.Post
{
    [Authorize(Roles = RoleConst.User)]
    public class CreateModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public CreateModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Id");
            ViewData["RentCategoryId"] = new SelectList(_context.RentCategories, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public RentPost RentPost { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            _context.RentPosts.Add(RentPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}