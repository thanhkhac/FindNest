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

        IEnumerable<RentPost> Search(RentPostSearchParams searchParams);
        RentPost? GetById();
        void Add(RentPost rentPost);
        void Update(RentPost rentPost);
        void Delete(int id);
    }

    public class RentPostRepository : IRentPostRepository
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public RentPostRepository(FindNestDbContext context)
        {
            _context = context;
        }


        public IEnumerable<RentPost> Search(RentPostSearchParams searchParams)
        {
            var query = _context.RentPosts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchParams.Title)) { query = query.Where(x => x.Title.Contains(searchParams.Title.Trim())); }
            if (searchParams.IsNegotiatedPrice) { query = query.Where(x => x.IsNegotiatedPrice == true); }
            if (searchParams.FromPrice != null) { query = query.Where(x => x.Price >= searchParams.FromPrice); }
            if (searchParams.ToPrice != null) { query = query.Where(x => x.Price <= searchParams.ToPrice); }
            if (!string.IsNullOrWhiteSpace(searchParams.Address)) { query = query.Where(x => x.Address.Contains(searchParams.Address.Trim())); }
            if (searchParams.RegionId != null)
            {
                int regionId = (int)searchParams.RegionId;
                var childRegions = GetChildRegions(regionId).Select(x => x.Id).ToList();
                query = query.Where(x => childRegions.Contains((int)x.RegionId));
            }
            return query.ToList();
        }

        public List<Region> GetChildRegions(int? parentId)
        {
            var IdParam = new SqlParameter("@Id", 1);
            var regions = _context.Regions
                .FromSqlRaw("EXEC GetChildRegions @Id", IdParam)
                .ToList();
            return regions;
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