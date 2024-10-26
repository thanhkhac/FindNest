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
        RentPost? GetById();
        void Add(RentPost rentPost);
        void Update(RentPost rentPost);
        void Delete(int id);

        IEnumerable<RentCategory> GetAllRentCategories();
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


        public IEnumerable<RentPost> Search(RentPostSearchParams? searchParams, out int TotalCount)
        {
            // Join with Users from the start
            var query = from rp in _context.RentPosts.Include(x => x.RentPostRooms)
                join u in _context.Users on rp.CreatedBy equals u.Id into userGroup
                select new { RentPost = rp, User = userGroup.DefaultIfEmpty() };

            // Apply ordering
            query = query.OrderByDescending(x => x.RentPost.CreatedAt).AsQueryable();

            if (searchParams != null)
            {
                Console.WriteLine(searchParams.ToString());

                // Apply Title filter
                if (!string.IsNullOrWhiteSpace(searchParams.Title)) { query = query.Where(x => x.RentPost.Title.Contains(searchParams.Title.Trim())); }

                // Price filter
                if (searchParams.IsPriceMinMaxFilter && searchParams.IsNegotiatedPrice)
                {
                    query = query.Where(x =>
                        (x.RentPost.IsNegotiatedPrice == true) ||
                        (((searchParams.MinPrice == null || x.RentPost.Price >= searchParams.MinPrice) &&
                          (searchParams.MaxPrice == null || x.RentPost.Price <= searchParams.MaxPrice)))
                    );
                }
                if (searchParams.IsNegotiatedPrice && !searchParams.IsPriceMinMaxFilter)
                {
                    query = query.Where(x => (x.RentPost.IsNegotiatedPrice == true));
                }
                else if (!searchParams.IsNegotiatedPrice && searchParams.IsPriceMinMaxFilter)
                {
                    query = query.Where(x =>
                        (x.RentPost.IsNegotiatedPrice == false) &&
                        (((searchParams.MinPrice == null || x.RentPost.Price >= searchParams.MinPrice) &&
                          (searchParams.MaxPrice == null || x.RentPost.Price <= searchParams.MaxPrice)))
                    );
                }

                // Address filter
                if (!string.IsNullOrWhiteSpace(searchParams.Address))
                {
                    query = query.Where(x => x.RentPost.Address.Contains(searchParams.Address.Trim()));
                }

                // Rent category filter
                if (searchParams.RentCategoryIds != null)
                {
                    query = query.Where(x =>
                        x.RentPost.RentCategoryId != null &&
                        (searchParams.RentCategoryIds.Contains((int)x.RentPost.RentCategoryId) || searchParams.RentCategoryIds.Count == 0));
                }

                // Area filters
                if (searchParams.MaxArea != null) { query = query.Where(x => x.RentPost.Area <= searchParams.MaxArea); }
                if (searchParams.MinArea != null) { query = query.Where(x => x.RentPost.Area >= searchParams.MinArea); }

                // Region filter
                if (searchParams.RegionId != null)
                {
                    int regionId = (int)searchParams.RegionId;
                    var childRegions = _regionRepository.GetChildRegionsRecursive(regionId).Select(x => x.Id).ToList();
                    query = query.Where(x => childRegions.Contains((int)x.RentPost.RegionId));
                }
            }

            TotalCount = query.Count();

            // Pagination
            var rentPosts = query.Skip((searchParams.CurrentPage - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToList();

            // Select the final result including user information
            var result = from x in rentPosts
                select new RentPost
                {
                    CreatedAt = x.RentPost.CreatedAt,
                    UpdatedAt = x.RentPost.UpdatedAt,
                    CreatedBy = x.RentPost.CreatedBy,
                    IsDeleted = x.RentPost.IsDeleted,
                    User = x.User.FirstOrDefault(), // Getting the user from the group
                    Id = x.RentPost.Id,
                    Title = x.RentPost.Title,
                    RegionId = x.RentPost.RegionId,
                    RentCategoryId = x.RentPost.RentCategoryId,
                    Price = x.RentPost.Price,
                    Area = x.RentPost.Area,
                    Address = x.RentPost.Address,
                    IsNegotiatedPrice = x.RentPost.IsNegotiatedPrice,
                    Thumbnail = x.RentPost.Thumbnail,
                    IsHidden = x.RentPost.IsHidden,
                    Region = x.RentPost.Region,
                    RentCategory = x.RentPost.RentCategory,
                    Utilities = x.RentPost.Utilities,
                    RentPostRooms = x.RentPost.RentPostRooms,
                    RegionAddress = x.RentPost.RegionId != null ? _regionRepository.GetAddress((int)x.RentPost.RegionId) : null,
                };

            return result.ToList();
        }


        public IEnumerable<RentCategory> GetAllRentCategories()
        {
            return _context.RentCategories.ToList();
        }


        public RentPost? GetById()
        {
            throw new NotImplementedException();
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