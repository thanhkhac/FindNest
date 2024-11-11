using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FindNest.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindNest.Data.Models;
using FindNest.Repositories;
using FindNest.Services;
using FindNest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FindNest.Pages.Post
{
    public class RentPostCreateInput
    {
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
        public string Title { get; set; } = "Cho thuê phòng";

        [DisplayName("Khu vực")]
        [Required(ErrorMessage = "Vui lòng chọn khu vực")]
        public int? RegionId { get; set; }

        [DisplayName("Loại nhà cho thuê")]
        [Required(ErrorMessage = "Vui lòng chọn loại nhà cho thuê")]
        public int? RentCategoryId { get; set; }

        [DisplayName("Giá")]
        [Range(0, 10_000_000_000, ErrorMessage = "Giá tiền không hợp lệ")]
        // [Required(ErrorMessage = "Vui lòng nhập giá phòng", AllowEmptyStrings = false)]
        public long? Price { get; set; } = 0;

        [DisplayName("Diện tích")]
        [Range(0, int.MaxValue, ErrorMessage = "Diện tích không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập diện tích phòng", AllowEmptyStrings = false)]

        public int? Area { get; set; }

        [DisplayName("Địa chỉ cụ thể")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ cụ thể", AllowEmptyStrings = false)]
        public string Address { get; set; }

        [DisplayName("Giá thỏa thuận")]
        public bool IsNegotiatedPrice { get; set; }
        public string? Thumbnail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public List<IFormFile>? Images { get; set; } = new List<IFormFile>();

        public List<int> FileIndex { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số phòng tắm, nhà vệ sinh không hợp lệ")]
        [DisplayName("Số phòng tắm, vệ sinh")]
        [Required(ErrorMessage = "Vui lòng nhập số phòng tắm, nhà vệ sinh")]
        public int BathroomQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số phòng ngủ không hợp lệ")]
        [DisplayName("Số phòng ngủ")]
        [Required(ErrorMessage = "Vui lòng nhập số phòng ngủ")]
        public int BedroomQuantity { get; set; }

        [MaxLength(10_000)]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        
        public double? Latitude { get; set; } = 21.0283334; //Vĩ độ
        
        public double? Longitude { get; set; } = 105.854041; //Kinh độ
    }

    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Data.FindNestDbContext _context;
        private readonly IRentPostService _rentPostService;
        private readonly IRegionService _regionService;
        private readonly UserManager<User> _userManager;
        private readonly IFileService _fileService;

        public List<RentCategory> RentCategories { get; set; } = new();
        public List<Region> CityRegions { get; set; } = new();
        public List<Region> DistrictRegions { get; set; } = new();
        public List<Region> WardRegions { get; set; } = new();
        public List<Region> SelectedRegions { get; set; } = new();

        public CreateModel(FindNest.Data.FindNestDbContext context, IRentPostService rentPostService, IRegionService regionService,
            UserManager<User> userManager, IFileService fileService)
        {
            _context = context;
            _rentPostService = rentPostService;
            _regionService = regionService;
            _userManager = userManager;
            _fileService = fileService;
        }

        [BindProperty]
        public RentPostCreateInput RentPost { get; set; } = new();


        public void Load()
        {
            RentCategories = _rentPostService.GetAllRentCategories().ToList();
            CityRegions = _regionService.GetChildRegions(null);
            if (RentPost.RegionId != null)
            {
                SelectedRegions = _regionService.GetParentRegionsRecursive((int)RentPost.RegionId);
                var city = SelectedRegions.FirstOrDefault(x => x.Level == 1);
                var district = SelectedRegions.FirstOrDefault(x => x.Level == 2);
                var ward = SelectedRegions.FirstOrDefault(x => x.Level == 3);
                if (city != null) { DistrictRegions = _regionService.GetChildRegions(city.Id); }
                if (district != null) { WardRegions = _regionService.GetChildRegions(district.Id); }
            }
            ViewData["RentCategoryId"] = new SelectList(_context.RentCategories, "Id", "Name");
        }

        public IActionResult OnGet()
        {
            Load();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Load();
            if (!ModelState.IsValid) { return Page(); }
            var formFiles = RentPost.Images;
            var user = await _userManager.GetUserAsync(User);
            if (formFiles.Count == 0)
            {
                ModelState.AddModelError("Images", "Vui lòng chọn ảnh");
                return Page();
            }
            if (formFiles != null)
            {
                if (formFiles.Count >10)
                {
                    ModelState.AddModelError("Images", "Bạn chỉ có thể đăng tối đa 10 ảnh");
                    return Page();
                }
                List<string> listPaths = await _fileService.SaveImagesAsync(formFiles, FolderConst.UserUploadFolder);
                var mediaList = listPaths.Select((x, index) => new Media
                {
                    MediaType = MediaTypeConst.Image,
                    Path = x,
                    Order = RentPost.FileIndex.ElementAt(index),
                }).ToList();

                List<RentPostRoom> postRooms = new List<RentPostRoom>();
                if (RentPost.BathroomQuantity > 0)
                {
                    postRooms.Add(new RentPostRoom
                    {
                        RoomId = RoomConst.Bathroom,
                        Quantity = RentPost.BathroomQuantity
                    });
                }

                if (RentPost.BedroomQuantity > 0)
                {
                    postRooms.Add(new RentPostRoom
                    {
                        RoomId = RoomConst.Bedroom,
                        Quantity = RentPost.BedroomQuantity
                    });
                }

                RentPost newRentPost = new RentPost
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = user!.Id,
                    UpdatedBy = user.Id,
                    IsDeleted = false,
                    Title = RentPost.Title,
                    RegionId = RentPost.RegionId,
                    RentCategoryId = RentPost.RentCategoryId,
                    Price = RentPost.Price.Value,
                    Area = RentPost.Area.Value,
                    Address = RentPost.Address,
                    IsNegotiatedPrice = RentPost.IsNegotiatedPrice,
                    Thumbnail = RentPost.Thumbnail,
                    IsHidden = false,
                    RentPostRooms = postRooms,
                    Mediae = mediaList,
                    RegionAddress = RentPost.Address,
                    Latitude = RentPost.Latitude,
                    Longitude = RentPost.Longitude,
                    Description = RentPost.Description
                };
                newRentPost.Thumbnail = listPaths.First();
                // if (newRentPost.Thumbnail == null && listPaths.Count > 0) { newRentPost.Thumbnail = listPaths.First(); }

                _context.RentPosts.Add(newRentPost);
                await _context.SaveChangesAsync();

                return RedirectToPage("Details", new { id = newRentPost.Id });
            }
            return Page();
        }
    }
}