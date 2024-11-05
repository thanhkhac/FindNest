using EFCore.BulkExtensions;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using FindNest.Services;
using FindNest.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Repositories
{
    public interface IRentPostService
    {
        // Task<IEnumerable<RentPost>> SearchAsync();
        // Task<RentPost?> GetByIdAsync();
        // Task AddAsync(RentPost rentPost);
        // Task UpdateAsync(RentPost rentPost);
        // Task DeleteAsync(int id);

        IEnumerable<RentPost> Search(RentPostSearchParams? searchParams, out int TotalCount);
        RentPost? GetById(int id);
        void Add(RentPost rentPost);
        void Update(RentPost rentPost);
        void Delete(int id);
        void Delete(List<int> ids);
        void DeleteByUserId(List<string> ids);
        void Delete(List<int> ids, string userId);

        void ApproveDeny(List<int> ids, bool action);
        void ApproveDenyAll(bool action);

        IEnumerable<RentCategory> GetAllRentCategories();
        IEnumerable<RentPost> GetLikedPost(LikedPostSearchParam searchParams, out int TotalCount);
    }

    public class RentPostService : IRentPostService
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRegionService _regionService;
        private readonly IFileService _fileService;

        public RentPostService(FindNestDbContext context, IRegionService regionService, IFileService fileService)
        {
            _context = context;
            _regionService = regionService;
            _fileService = fileService;
        }

        public IEnumerable<RentPost> GetLikedPost(LikedPostSearchParam searchParams, out int TotalCount)
        {
            var query = _context.Likes
                .Include(x => x.RentPost.RentPostRooms)
                .Include(x => x.RentPost.CreatedUser)
                .Include(x => x.RentPost.RentCategory)
                .OrderByDescending(x => x.LikedDate)
                .Where(x => x.UserId.Equals(searchParams.UserId))
                .Select(x => x.RentPost);

            TotalCount = query.Count();

            var rentPosts = query.Skip((searchParams.CurrentPage - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToList();

            foreach (var rentPost in rentPosts)
            {
                rentPost.RegionAddress = rentPost.RegionId != null ? _regionService.GetAddress((int)rentPost.RegionId) : null;
            }
            return rentPosts;
        }

        public IEnumerable<RentPost> Search(RentPostSearchParams? searchParams, out int totalCount)
        {
            // Join with Users from the start
            var query = _context.RentPosts
                .Include(x => x.RentPostRooms)
                .Include(x => x.CreatedUser)
                .Include(x => x.RentCategory)
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking().AsQueryable();

            if (searchParams != null)
            {
                // Console.WriteLine(searchParams.ToString());
                // Title
                if (!string.IsNullOrWhiteSpace(searchParams.Title)) { query = query.Where(x => x.Title.Contains(searchParams.Title.Trim())); }

                // Price 
                if (searchParams.IsPriceMinMaxFilter && searchParams.IsNegotiatedPrice)
                {
                    query = query.Where(x =>
                        (x.IsNegotiatedPrice == true) ||
                        (((searchParams.MinPrice == null || x.Price >= searchParams.MinPrice) &&
                          (searchParams.MaxPrice == null || x.Price <= searchParams.MaxPrice)))
                    );
                }
                if (searchParams.IsNegotiatedPrice && !searchParams.IsPriceMinMaxFilter) { query = query.Where(x => (x.IsNegotiatedPrice == true)); }
                else if (!searchParams.IsNegotiatedPrice && searchParams.IsPriceMinMaxFilter)
                {
                    query = query.Where(x =>
                        (x.IsNegotiatedPrice == false) &&
                        (((searchParams.MinPrice == null || x.Price >= searchParams.MinPrice) &&
                          (searchParams.MaxPrice == null || x.Price <= searchParams.MaxPrice)))
                    );
                }

                // Address 
                if (!string.IsNullOrWhiteSpace(searchParams.Address)) { query = query.Where(x => x.Address.Contains(searchParams.Address.Trim())); }

                // Rent 
                query = query.Where(x =>
                    x.RentCategoryId != null &&
                    (searchParams.RentCategoryIds.Contains((int)x.RentCategoryId) || searchParams.RentCategoryIds.Count == 0));

                // Area 
                if (searchParams.MaxArea != null) { query = query.Where(x => x.Area <= searchParams.MaxArea); }
                if (searchParams.MinArea != null) { query = query.Where(x => x.Area >= searchParams.MinArea); }

                // Region 
                if (searchParams.RegionId != null)
                {
                    int regionId = (int)searchParams.RegionId;
                    var childRegions = _regionService.GetChildRegionsRecursive(regionId).Select(x => x.Id).ToList();
                    query = query.Where(x => childRegions.Contains((int)x.RegionId));
                }

                if (searchParams.IsApproved != null) { query = query.Where(x => x.IsApproved == searchParams.IsApproved); }
                if (searchParams.IsWaiting)
                {
                    query = query.Where(x => x.IsApproved == null);
                }
                //UserId
                if (searchParams.UserId != null) { query = query.Where(x => x.CreatedBy != null && x.CreatedBy.Equals(searchParams.UserId)); }
            }
            totalCount = query.Count();
            Console.WriteLine(totalCount);


            // Pagination
            var rentPosts = query.Skip((searchParams!.CurrentPage - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToList();

            foreach (var rentPost in rentPosts)
            {
                if (rentPost.RegionId != null) { rentPost.RegionAddress = _regionService.GetAddress((int)rentPost.RegionId); }
            }

            return rentPosts;
        }


        public void Delete(List<int> ids)
        {
            var deletePosts = _context.RentPosts.Where(x => ids.Contains(x.Id));
            var deleteRentPostRooms = _context.RentPostRooms.Where(x => ids.Contains(x.RentPostId));
            var deleteMedias = _context.Media.Where(x => ids.Contains(x.RentPostId)).ToList();
            var deleteMediaPaths = deleteMedias.Select(x => x.Path).ToList();
            var deleteLikes = _context.Likes.Where(x=>ids.Contains(x.RentPostId)).ToList();

            _context.Media.RemoveRange(deleteMedias);
            _context.RentPostRooms.RemoveRange(deleteRentPostRooms);
            _context.RentPosts.RemoveRange(deletePosts);
            _context.Likes.RemoveRange(deleteLikes);
            _context.SaveChanges();

            _fileService.DeleteFileAsync(deleteMediaPaths);
        }
        public void DeleteByUserId(List<string> ids)
        {
            var deletePostCount = _context.RentPosts
                .Where(r => ids.Contains(r.CreatedBy))
                .ExecuteDelete(); 
        }

        public void Delete(List<int> ids, string userId)
        {
            var deletePosts = _context.RentPosts.Where(x => x.CreatedBy != null && ids.Contains(x.Id) && x.CreatedBy.Equals(userId));
            var deleteRentPostRooms = _context.RentPostRooms.Where(x => ids.Contains(x.RentPostId));
            var deleteMedias = _context.Media.Where(x => ids.Contains(x.RentPostId)).ToList();
            var deleteMediaPaths = deleteMedias.Select(x => x.Path).ToList();

            _context.Media.RemoveRange(deleteMedias);
            _context.RentPostRooms.RemoveRange(deleteRentPostRooms);
            _context.RentPosts.RemoveRange(deletePosts);
            _context.SaveChanges();

            _fileService.DeleteFileAsync(deleteMediaPaths);
        }
        public void ApproveDeny(List<int> ids, bool value)
        {
            var posts = _context.RentPosts.Where(x => ids.Contains(x.Id) && x.IsApproved == null)
                .ExecuteUpdate(x => x.SetProperty(b => b.IsApproved, b => value));
            _context.SaveChanges();
        }
        public void ApproveDenyAll(bool value)
        {
            var posts = _context.RentPosts
                .Where(x => x.IsApproved == null)
                .ExecuteUpdate(x => x.SetProperty(b => b.IsApproved, b => value));
            _context.SaveChanges();
        }

        public List<RentPost> GetByUserId(string id)
        {
            var rentPosts = _context.RentPosts
                .Include(post => post.Mediae)
                .Include(post => post.RentPostRooms)
                .Include(post => post.RentCategory)
                .Include(post => post.Region)
                .Where(post => post.CreatedBy == id).ToList();
            foreach (var rentPost in rentPosts)
            {
                if (rentPost.RegionId != null) { rentPost.RegionAddress = _regionService.GetAddress((int)rentPost.RegionId); }
            }
            return rentPosts;
        }

        public IEnumerable<RentCategory> GetAllRentCategories()
        {
            return _context.RentCategories.ToList();
        }


        public RentPost? GetById(int id)
        {
            var rentPost = _context.RentPosts
                .Include(post => post.Mediae)
                .Include(post => post.RentPostRooms)
                .Include(post => post.RentCategory)
                .Include(post => post.Region)
                .FirstOrDefault(post => post.Id == id);
            if (rentPost == null) { return null; }
            rentPost.RegionAddress = rentPost.RegionId.HasValue
                ? _regionService.GetAddress(rentPost.RegionId.Value)
                : null;
            return rentPost;
        }


        public void Add(RentPost rentPost)
        {
            throw new NotImplementedException();
        }

        public void Update(RentPost rentPost)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}