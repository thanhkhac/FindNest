using FindNest.Constants;
using FindNest.Data.Models;
using FindNest.Pages.Shared.PartialModels;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Admin.Post
{
    [Authorize(Roles = RoleConst.Administrator)]
    public class Waiting : PageModel
    {
        private readonly IRentPostService _rentPostService;
        private readonly UserManager<User> _userManager;

        public Waiting(IRentPostService rentPostService, UserManager<User> userManager)
        {
            _rentPostService = rentPostService;
            _userManager = userManager;
        }

        public IList<RentPost> RentPosts { get; set; } = default!;
        public PaginationPm PaginationPm { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public RentPostSearchParams Params { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public List<int> Ids { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Action { get; set; }

        public void OnGet()
        {
            Load();
        }

        private void Load()
        {
            Params.IsApproved = null;
            Params.IsWaiting = true;
            RentPosts = _rentPostService.Search(Params, out int count).ToList();

            PaginationPm = new PaginationPm
            {
                ParamName = nameof(Params) + "." + nameof(Params.CurrentPage),
                PageCount = (int)Math.Ceiling((double)count / Params.PageSize),
                CurrentPage = Params.CurrentPage,
                TotalCount = count,
                FormName = "paginationForm"
            };
        }

        public IActionResult OnPostDeleteOrProcess()
        {
            if (Action.Equals("delete")) { _rentPostService.Delete(Ids); }
            if (Action.Equals("approve")) { _rentPostService.ApproveDeny(Ids, true); }
            if (Action.Equals("deny")) { _rentPostService.ApproveDeny(Ids, false); }
            if (Action.Equals("approveAll")) { _rentPostService.ApproveDenyAll(true); }
            Load();

            return RedirectToPage(null, new { CurrentPage = Params.CurrentPage });
        }

    }
}