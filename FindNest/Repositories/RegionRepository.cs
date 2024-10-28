using FindNest.Data;
using FindNest.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FindNest.Repositories
{
    public interface IRegionRepository
    {
        List<Region> GetChildRegionsRecursive(int? parentId);
        string GetAddress(int id);
        List<Region> GetChildRegions(int? parentId);
        List<Region> GetParentRegionsRecursive(int id);
    }

    public class RegionRepository : IRegionRepository
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        
        public RegionRepository(FindNestDbContext context)
        {
            _context = context;
        }
        
        public List<Region> GetChildRegionsRecursive(int? parentId)
        {
            var IdParam = new SqlParameter("@Id", parentId);
            var regions = _context.Regions
                .FromSqlRaw("EXEC GetChildRegions @Id", IdParam)
                .ToList();
            return regions;
        }
        
        public List<Region> GetChildRegions(int? parentId)
        {
            var regions = _context.Regions.AsQueryable();
            if(parentId == null) regions = regions.Where(x=>x.Level == 1);
            else regions = regions.Where(x=>x.ParentId == parentId);
            return regions.ToList();
        }
        
        public string GetAddress(int id)
        {
            var IdParam = new SqlParameter("@Id", id);
            var regions = _context.Regions
                .FromSqlRaw("EXEC GetAddress @Id", IdParam)
                .ToList();
            // Console.WriteLine(regions.Count);
            return string.Join(", ", regions.Select(x => x.FullName));
        }
        
        public List<Region> GetParentRegionsRecursive(int id)
        {
            var IdParam = new SqlParameter("@Id", id);
            var regions = _context.Regions
                .FromSqlRaw("EXEC GetAddress @Id", IdParam).AsEnumerable()
                .OrderBy(x=>x.Level).ToList();
            return regions;
        }

    }
}