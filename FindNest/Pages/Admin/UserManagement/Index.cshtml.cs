using FindNest.Constants;
using FindNest.Data.Models;
using FindNest.Pages.Shared.PartialModels;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Pages.Admin.UserManagement
{
    [Authorize(Roles = RoleConst.Administrator)]
    public class Index : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRentPostService _rentPostService;
        private readonly UserManager<Data.Models.User> _userManager;

        public Index(UserManager<User> userManager, IUserService userService, IRentPostService rentPostService)
        {
            _userManager = userManager;
            _userService = userService;
            _rentPostService = rentPostService;
        }

        public IList<User> Users { get; set; } = default!;
        public PaginationPm PaginationPm { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public SearchParams Params { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public List<string> Ids { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Action { get; set; }

        public void OnGet()
        {
            Load();
        }

        private void Load()
        {
            Users = _userService.SearchUser(Params, out int count).ToList();

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
            var user = _userManager.GetUserAsync(User).Result;
            if (Ids.Contains(user.Id) && Action.Equals("delete"))
            {
                ModelState.AddModelError(string.Empty, "Bạn không thể xóa tài khoản của mình.");
                Load();
                return Page();
            }
            if (Ids.Contains(user.Id) && Action.Equals("disableAdmin"))
            {
                ModelState.AddModelError(string.Empty, "Bạn không thể gỡ quyền quản trị viên của mình.");
                Load();
                return Page();
            }
            if (Ids.Contains(user.Id) && Action.Equals("ban"))
            {
                ModelState.AddModelError(string.Empty, "Bạn không thể cấm tài khoản của mình.");
                Load();
                return Page();
            }
            if (Action.Equals("delete")) { _userService.DeleteUsers(Ids); }
            if (Action.Equals("disableAdmin")) { _userService.DisableAdmin(Ids); }
            if (Action.Equals("setAdmin")) { _userService.SetAdmin(Ids); }
            if (Action.Equals("ban")) { _userService.Ban(Ids); }
            if (Action.Equals("unBan")) { _userService.UnBan(Ids); }
            if (Action.Equals("deleteAllPost")) { _rentPostService.DeleteByUserId(Ids); }

            return RedirectToPage(null, new { CurrentPage = Params.CurrentPage });
        }

    }
}