using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Pages.Shared.PartialModels;
using FindNest.Params;
using FindNest.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindNest.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly IRentPostService _rentPostService;
        private readonly IRegionService _regionService;


        public IndexModel(IRentPostService rentPostService, IRegionService regionService)
        {
            // _context = context;
            _rentPostService = rentPostService;
            _regionService = regionService;
        }

        [BindProperty(SupportsGet = true)]
        public RentPostSearchParams Params { get; set; } = new();
        public PaginationPm PaginationPm { get; set; } = default!;

        public IList<RentPost> RentPost { get; set; } = default!;
        public List<RentCategory> RentCategories = new();
        public List<Region> CityRegions = new();
        public List<Region> DistrictRegions = new();
        public List<Region> WardRegions = new();
        public List<Region> SelectedRegions = new();
        
        public Task<IActionResult> OnGetAsync()
        {
            Load();
            RentPost = _rentPostService.Search(Params, out int count).ToList();
            PaginationPm = new PaginationPm
            {
                ParamName = nameof(Params) + "." + nameof(Params.CurrentPage),
                PageCount = (int)Math.Ceiling((double)count / Params.PageSize),
                CurrentPage = Params.CurrentPage,
                TotalCount = count,
                FormName = "searchForm"
            };
            return Task.FromResult<IActionResult>(Page());
        }
        
        private void Load()
        {
            RentCategories = _rentPostService.GetAllRentCategories().ToList();
            CityRegions = _regionService.GetChildRegions(null);
            if (Params.RegionId != null)
            {
                SelectedRegions = _regionService.GetParentRegionsRecursive((int)Params.RegionId);
                var city = SelectedRegions.FirstOrDefault(x => x.Level == 1);
                var district = SelectedRegions.FirstOrDefault(x => x.Level == 2);
                var ward = SelectedRegions.FirstOrDefault(x => x.Level == 3);
                if (city != null) { DistrictRegions = _regionService.GetChildRegions(city.Id); }
                if (district != null) { WardRegions = _regionService.GetChildRegions(district.Id); }
            }
        }
    }
}