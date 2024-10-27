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
    public class ListModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public ListModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        public IList<RentPost> RentPost { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RentPost = await _context.RentPosts
                .Include(r => r.Region)
                .Include(r => r.RentCategory).ToListAsync();
        }
    }
}
