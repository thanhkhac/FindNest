using FindNest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FindNest.Attributes
{
    public class AuthorizeRentPostOwnerAttribute() : TypeFilterAttribute(typeof(AuthorizeRentPostOwnerFilter))
    {

    }

    internal class AuthorizeRentPostOwnerFilter(UserManager<User> userManager, Data.FindNestDbContext context) : IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            var user = await userManager.GetUserAsync(filterContext.HttpContext.User);
            if (user == null)
            {
                filterContext.Result = new ForbidResult();
                return;
            }

            if (filterContext.RouteData.Values.TryGetValue("id", out var idObj) && int.TryParse(idObj.ToString(), out var id))
            {
                var rentPost = await context.RentPosts.FindAsync(id);

                if (rentPost == null || rentPost.CreatedBy != user.Id) { filterContext.Result = new ForbidResult(); }
            }        
        }
    }
}