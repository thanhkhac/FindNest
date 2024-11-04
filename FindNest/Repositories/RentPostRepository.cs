using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Repositories
{
    public interface IRentPostRepository
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

        IEnumerable<RentCategory> GetAllRentCategories();
        IEnumerable<RentPost> GetLikedPost(LikedPostSearchParam searchParams, out int TotalCount);
    }

    public class RentPostRepository : IRentPostRepository
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRegionRepository _regionRepository;

        public RentPostRepository(FindNestDbContext context, IRegionRepository regionRepository)
        {
            _context = context;
            _regionRepository = regionRepository;
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
                rentPost.RegionAddress = rentPost.RegionId != null ? _regionRepository.GetAddress((int)rentPost.RegionId) : null;
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
                    var childRegions = _regionRepository.GetChildRegionsRecursive(regionId).Select(x => x.Id).ToList();
                    query = query.Where(x => childRegions.Contains((int)x.RegionId));
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
                if (rentPost.RegionId != null) { rentPost.RegionAddress = _regionRepository.GetAddress((int)rentPost.RegionId); }
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
                ? _regionRepository.GetAddress(rentPost.RegionId.Value)
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