﻿using FindNest.Data.Models;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Like
{
    [Authorize]
    public class Index : PageModel
    {
        private readonly IRentPostRepository _rentPostRepository;
        private readonly UserManager<User> _userManager;
        public Index(IRentPostRepository rentPostRepository, UserManager<User> userManager)
        {
            _rentPostRepository = rentPostRepository;
            _userManager = userManager;
        }
        
        public List<RentPost> RentPosts { get; set; }
        
        public int PageCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public LikedPostSearchParam SearchParam { get; set; } = new LikedPostSearchParam();
        
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            SearchParam.UserId = user.Id;
            RentPosts = _rentPostRepository.GetLikedPost(SearchParam, out int count).ToList();
            PageCount = (int)Math.Ceiling((double)count / SearchParam.PageSize);
        }
    }
}