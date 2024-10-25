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
            var query = _context.RentPosts.Include(x => x.RentPostRooms).OrderByDescending(x => x.CreatedAt).AsQueryable();
            if (searchParams != null)
            {
                Console.WriteLine(searchParams.ToString());
                if (!string.IsNullOrWhiteSpace(searchParams.Title)) { query = query.Where(x => x.Title.Contains(searchParams.Title.Trim())); }

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

                if (!string.IsNullOrWhiteSpace(searchParams.Address)) { query = query.Where(x => x.Address.Contains(searchParams.Address.Trim())); }
                if (searchParams.RentCategoryIds != null)
                {
                    query = query.Where(x =>
                        x.RentCategoryId != null &&
                        (searchParams.RentCategoryIds.Contains((int)x.RentCategoryId) || searchParams.RentCategoryIds.Count == 0));
                }

                if (searchParams.MaxArea != null)
                {
                    query = query.Where(x => x.Area <= searchParams.MaxArea);
                }
                if (searchParams.MinArea != null)
                {
                    query = query.Where(x => x.Area >= searchParams.MinArea);
                }
                if (searchParams.RegionId != null)
                {
                    int regionId = (int)searchParams.RegionId;
                    var childRegions = _regionRepository.GetChildRegionsRecursive(regionId).Select(x => x.Id).ToList();
                    query = query.Where(x => childRegions.Contains((int)x.RegionId));
                }
            }
            TotalCount = query.Count();
            // Console.WriteLine(GetAddress(32867));
            // Console.WriteLine("HEllo");
            var result = query.Skip(searchParams.CurrentPage - 1).Take(searchParams.PageSize).ToList();
            foreach (var rentPost in result)
            {
                if (rentPost.RegionId != null) { rentPost.RegionAddress = _regionRepository.GetAddress((int)rentPost.RegionId); }
            }
            return result;
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