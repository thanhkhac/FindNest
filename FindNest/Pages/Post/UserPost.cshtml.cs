using FindNest.Data.Models;
using FindNest.Pages.Shared.PartialModels;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Post
{
    [Authorize]
    public class UserPost : PageModel
    {
        private readonly IRentPostRepository _rentPostRepository;
        private readonly UserManager<User> _userManager;
        
        public UserPost(IRentPostRepository rentPostRepository, UserManager<User> userManager)
        {
            _rentPostRepository = rentPostRepository;
            _userManager = userManager;
        }
        
        public IList<RentPost> RentPosts { get; set; } = default!;
        public PaginationPm PaginationPm { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public RentPostSearchParams Params { get; set; } = new();
        
        [BindProperty]
        public List<int> DeleteIds { get; set; } = new();


        public void OnGet()
        {
            Load();
        }
        
        private void Load()
        {
            var userId = _userManager.GetUserId(User);
            Console.WriteLine(userId);
            RentPosts = _rentPostRepository.Search(Params, out int count).ToList();
            
            PaginationPm = new PaginationPm
            {
                ParamName = nameof(Params) + "." + nameof(Params.CurrentPage),
                PageCount = (int)Math.Ceiling((double)count / Params.PageSize),
                CurrentPage = Params.CurrentPage,
                TotalCount = count,
                FormName = "paginationForm"
            };
        }
        
        public void  OnPostDelete()
        {
            Console.WriteLine(DeleteIds.Count);
            Load();
        }    
    }
}