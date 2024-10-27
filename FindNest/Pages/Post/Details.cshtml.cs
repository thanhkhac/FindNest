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
    public class DetailsModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRentPostRepository _rentPostRepository;
        
        

        public DetailsModel(FindNest.Data.FindNestDbContext context, IRentPostRepository rentPostRepository)
        {
            _context = context;
            _rentPostRepository = rentPostRepository;
        }

        public RentPost RentPost { get; set; } = default!;
        
        
        public List<RentPost> RentPostsInRegion = new List<RentPost>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rentpost = _rentPostRepository.GetById(id.Value);
            if (rentpost == null)
            {
                return NotFound();
            }
            else
            {
                RentPost = rentpost;
                if (rentpost.RegionId != null)
                {
                    RentPostSearchParams searchParams = new RentPostSearchParams();
                    searchParams.RegionId = rentpost.RegionId;
                    searchParams.PageSize = 5;
                    RentPostsInRegion = _rentPostRepository.Search(searchParams, out int count).ToList();
                }
            }
            return Page();
        }
    }
}
