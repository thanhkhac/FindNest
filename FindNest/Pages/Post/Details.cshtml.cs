using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;

namespace FindNest.Pages.Post
{
    public class DetailsModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public DetailsModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        public RentPost RentPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentpost = await _context.RentPosts.FirstOrDefaultAsync(m => m.Id == id);
            if (rentpost == null)
            {
                return NotFound();
            }
            else
            {
                RentPost = rentpost;
            }
            return Page();
        }
    }
}
