using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindNest.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FindNest.Pages.Post
{
    public class DetailsModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRentPostService _rentPostService;
        private readonly UserManager<User> _userManager;


        public DetailsModel(FindNest.Data.FindNestDbContext context, IRentPostService rentPostService, UserManager<User> userManager)
        {
            _context = context;
            _rentPostService = rentPostService;
            _userManager = userManager;
        }

        public RentPost RentPost { get; set; } = default!;


        public List<RentPost> RentPostsInRegion = new List<RentPost>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) { return NotFound(); }
            var rentpost = _rentPostService.GetById(id.Value);
            if (rentpost == null) { return NotFound(); }
            RentPost = rentpost;
            if (rentpost.RegionId != null)
            {
                RentPostSearchParams searchParams = new RentPostSearchParams();
                searchParams.RegionId = rentpost.RegionId;
                searchParams.PageSize = 5;
                RentPostsInRegion = _rentPostService.Search(searchParams, out int count).ToList();
            }
            if (rentpost.IsApproved != true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) { return NotFound(); }
                bool isOwner = user.Id.Equals(rentpost.CreatedBy);
                bool isAdmin = await _userManager.IsInRoleAsync(user, RoleConst.Administrator);
                if (isAdmin || isOwner) { return Page(); }
                return NotFound();
            }


            return Page();
        }
    }
}