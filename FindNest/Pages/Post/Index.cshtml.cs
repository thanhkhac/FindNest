using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using FindNest.Repositories;

namespace FindNest.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRentPostRepository _rentPostRepository;

        
        public IndexModel(FindNest.Data.FindNestDbContext context, IRentPostRepository rentPostRepository)
        {
            _context = context;
            _rentPostRepository = rentPostRepository;
        }

        public RentPostSearchParams Params { get; set; }

        public IList<RentPost> RentPost { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RentPost = _rentPostRepository.Search(Params).ToList();
        }
    }
}
