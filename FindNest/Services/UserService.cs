using FindNest.Constants;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Repositories
{
    public interface IUserService
    {
        IEnumerable<User> SearchUser(UserSearchParam searchParams, out int totalCount);
        void DeleteUsers(List<string> ids);
        void SetAdmin(List<string> ids);
        void DisableAdmin(List<string> ids);
        void Ban(List<string> ids);
        void UnBan(List<string> ids);
    }

    public class UserService : IUserService
    {
        private readonly FindNestDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IRentPostService _rentPostService;

        public UserService(FindNestDbContext context, UserManager<User> userManager, IRentPostService rentPostService)
        {
            _context = context;
            _userManager = userManager;
            _rentPostService = rentPostService;
        }

        public IEnumerable<User> SearchUser(UserSearchParam searchParams, out int totalCount)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(x=>x.IsBanned == searchParams.IsBanned);

            if (!string.IsNullOrEmpty(searchParams.Email)) { query = query.Where(u => u.Email.Contains(searchParams.Email.Trim())); }
            if (!string.IsNullOrEmpty(searchParams.UserId)) { query = query.Where(u => u.Id.Equals(searchParams.UserId)); }
            if (!string.IsNullOrEmpty(searchParams.FullName)) { query = query.Where(u => u.FullName.Contains(searchParams.FullName.Trim())); }
            if (!string.IsNullOrEmpty(searchParams.ContactPhoneNumber)) { query = query.Where(u => u.ContactPhoneNumber.Contains(searchParams.ContactPhoneNumber.Trim())); }

            totalCount = query.Select(x=>x.Id).Count();


            query = query.Skip((searchParams.CurrentPage - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize);

            return query.ToList();
        }

        public void DeleteUsers(List<string> ids)
        {
            var usersToDelete = _context.Users.Where(u => ids.Contains(u.Id)).ToList();

            var userIdsToDelete = usersToDelete.Select(u => u.Id).ToList();

            var deletePostIds = _context.RentPosts
                .Where(x => userIdsToDelete.Contains(x.CreatedBy))
                .Select(x => x.Id)
                .ToList();

            var deleteLikes = _context.Likes
                .Where(x => ids.Contains(x.UserId))
                .ToList();


            if (deletePostIds.Any()) { _rentPostService.Delete(deletePostIds); }

            if (deleteLikes.Any()) { _context.Likes.RemoveRange(deleteLikes); }

            _context.SaveChanges();

            foreach (var user in usersToDelete)
            {
                var result = _userManager.DeleteAsync(user).Result;
            }
        }
        public void SetAdmin(List<string> ids)
        {
            foreach (var userId in ids)
            {
                var user = _userManager.FindByIdAsync(userId).Result;
                if (user != null)
                {
                    if (!_userManager.IsInRoleAsync(user, RoleConst.Administrator).Result)
                    {
                        var result = _userManager.AddToRoleAsync(user, RoleConst.Administrator).Result;
                    }
                }
            }
        }

        public void DisableAdmin(List<string> ids)
        {
            foreach (var userId in ids)
            {
                var user = _userManager.FindByIdAsync(userId).Result;
                if (user != null)
                {
                    if (_userManager.IsInRoleAsync(user, RoleConst.Administrator).Result)
                    {
                        var result = _userManager.RemoveFromRoleAsync(user, RoleConst.Administrator).Result;
                    }
                }
            }
        }

        public void Ban(List<string> ids)
        {
            var userUpdateTask = _context.Users
                .Where(x => ids.Contains(x.Id))
                .ExecuteUpdate(x => x.SetProperty(b => b.LockoutEnabled, b => true)
                    .SetProperty(b => b.LockoutEnd, b => DateTime.UtcNow.AddYears(100))
                    .SetProperty(b => b.SecurityStamp, b => Guid.NewGuid().ToString())
                    .SetProperty(b => b.IsBanned, b => true)
                    );
            var deletePostIds = _context.RentPosts
                                          .Where(x => ids.Contains(x.CreatedBy))
                                          .Select(x => x.Id)
                                          .ToList();
            // _rentPostService.DeleteByUserId(ids);    
        }

        public void UnBan(List<string> ids)
        {
            var userUpdateTask = _context.Users
                .Where(x => ids.Contains(x.Id))
                .ExecuteUpdate(x => x
                    .SetProperty(b => b.LockoutEnd, b => DateTime.UtcNow.AddYears(-1))
                    .SetProperty(b => b.IsBanned, b => false)
                    );
        }


    }
}