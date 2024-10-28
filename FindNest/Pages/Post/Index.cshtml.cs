using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Params;
using FindNest.Repositories;

namespace FindNest.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRentPostRepository _rentPostRepository;
        private readonly IRegionRepository _regionRepository;

        
        public IndexModel(FindNest.Data.FindNestDbContext context, IRentPostRepository rentPostRepository, IRegionRepository regionRepository)
        {
            _context = context;
            _rentPostRepository = rentPostRepository;
            _regionRepository = regionRepository;
        }

        [BindProperty(SupportsGet = true)]
        public RentPostSearchParams Params { get; set; } = new RentPostSearchParams();

        public IList<RentPost> RentPost { get;set; } = default!;
        
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        
        public List<RentCategory> RentCategories;
        public List<Region> CityRegions = new List<Region>();
        public List<Region> DistrictRegions = new List<Region>();
        public List<Region> WardRegions = new List<Region>();
        public List<Region> SelectedRegions = new List<Region>();
        public void Load()
        {
            RentCategories = _rentPostRepository.GetAllRentCategories().ToList();
            CityRegions = _regionRepository.GetChildRegions(null);
            if (Params.RegionId != null)
            {
                SelectedRegions = _regionRepository.GetParentRegionsRecursive((int)Params.RegionId);
                var city = SelectedRegions.FirstOrDefault(x=>x.Level == 1);
                var district = SelectedRegions.FirstOrDefault(x=>x.Level == 2);
                var ward = SelectedRegions.FirstOrDefault(x=>x.Level == 3);
                if (city != null)
                {
                    DistrictRegions = _regionRepository.GetChildRegions(city.Id);
                }
                if (district != null)
                {
                    WardRegions = _regionRepository.GetChildRegions(district.Id);
                }
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Load();
            RentPost = _rentPostRepository.Search(Params, out int count).ToList();
            TotalCount = count;
            PageCount = (int)Math.Ceiling((double)count /Params.PageSize);
            return Page();
        }        
    }
}
