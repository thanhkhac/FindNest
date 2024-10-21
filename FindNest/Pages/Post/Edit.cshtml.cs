using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;

namespace FindNest.Pages.Post
{
    public class EditModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public EditModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RentPost RentPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentpost =  await _context.RentPosts.FirstOrDefaultAsync(m => m.Id == id);
            if (rentpost == null)
            {
                return NotFound();
            }
            RentPost = rentpost;
           ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Id");
           ViewData["RentCategoryId"] = new SelectList(_context.RentCategories, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RentPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentPostExists(RentPost.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RentPostExists(int id)
        {
            return _context.RentPosts.Any(e => e.Id == id);
        }
    }
}
