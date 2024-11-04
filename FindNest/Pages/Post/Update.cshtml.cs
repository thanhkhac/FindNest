using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FindNest.Attributes;
using FindNest.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FindNest.Data.Models;
using FindNest.Repositories;
using FindNest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;


namespace FindNest.Pages.Post
{
    public class RentPostUpdateInput
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
        public long Price { get; set; } = 0;

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
        public List<IFormFile>? NewImages { get; set; } = new List<IFormFile>();

        public List<string> OldImagePaths { get; set; } = new List<string>();


        public List<int> NewFileIndex { get; set; } = new ();
        public List<int> OldFileIndex { get; set; } = new ();

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

        public int? Id { get; set; }
    }

    [Authorize]
    [AuthorizeRentPostOwner]
    public class UpdateModel : PageModel
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

        public UpdateModel(Data.FindNestDbContext context, IRentPostService rentPostService, IRegionService regionService,
            UserManager<User> userManager, IFileService fileService)
        {
            _context = context;
            _rentPostService = rentPostService;
            _regionService = regionService;
            _userManager = userManager;
            _fileService = fileService;
        }

        [BindProperty]
        public RentPostUpdateInput UpdateInput { get; set; } = new();


        public void Load()
        {
            RentCategories = _rentPostService.GetAllRentCategories().ToList();
            CityRegions = _regionService.GetChildRegions(null);
            if (UpdateInput.RegionId != null)
            {
                SelectedRegions = _regionService.GetParentRegionsRecursive((int)UpdateInput.RegionId);
                var city = SelectedRegions.FirstOrDefault(x => x.Level == 1);
                var district = SelectedRegions.FirstOrDefault(x => x.Level == 2);
                var ward = SelectedRegions.FirstOrDefault(x => x.Level == 3);
                if (city != null) { DistrictRegions = _regionService.GetChildRegions(city.Id); }
                if (district != null) { WardRegions = _regionService.GetChildRegions(district.Id); }
            }
            ViewData["RentCategoryId"] = new SelectList(_context.RentCategories, "Id", "Name");
        }

        public IActionResult OnGet(int id)
        {
            RentPost rentPost = _rentPostService.GetById(id)!;
            UpdateInput = new RentPostUpdateInput
            {
                Title = rentPost.Title,
                RegionId = rentPost.RegionId,
                RentCategoryId = rentPost.RentCategoryId,
                Price = rentPost.Price,
                Area = rentPost.Area,
                Address = rentPost.Address,
                IsNegotiatedPrice = rentPost.IsNegotiatedPrice,
                Thumbnail = rentPost.Thumbnail,
                BathroomQuantity = rentPost.BathRoomCount,
                BedroomQuantity = rentPost.BedRoomCount,
                Description = rentPost.Description,
                OldImagePaths = rentPost.Mediae.OrderBy(x=>x.Order).Select(x => x.Path).ToList(),
                Id = id
            };
            Load();
            return Page();
        }


        public async Task<IActionResult?> OnPostAsync()
        {
            Load();
            if (!ModelState.IsValid) { return Page(); }
            var formFiles = UpdateInput.NewImages;
            var user = await _userManager.GetUserAsync(User);

            //Ảnh mới
            List<string> listPaths = await _fileService.SaveImagesAsync(formFiles, FolderConst.UserUploadFolder);
            var mediaList = listPaths.Select((x, index) => new Media
            {
                MediaType = MediaTypeConst.Image,
                Path = x,
                Order = UpdateInput.NewFileIndex.ElementAt(index),
            }).ToList();

            List<RentPostRoom> postRooms = new List<RentPostRoom>();
            if (UpdateInput.BathroomQuantity > 0)
            {
                postRooms.Add(new RentPostRoom
                {
                    RoomId = RoomConst.Bathroom,
                    Quantity = UpdateInput.BathroomQuantity
                });
            }

            if (UpdateInput.BedroomQuantity > 0)
            {
                postRooms.Add(new RentPostRoom
                {
                    RoomId = RoomConst.Bedroom,
                    Quantity = UpdateInput.BedroomQuantity
                });
            }

            RentPost oldRentPost = _rentPostService.GetById(UpdateInput.Id.Value)!;
            oldRentPost.UpdatedAt = DateTime.Now;
            oldRentPost.UpdatedBy = user.Id;
            oldRentPost.Title = UpdateInput.Title;
            oldRentPost.RegionId = UpdateInput.RegionId;
            oldRentPost.RentCategoryId = UpdateInput.RentCategoryId;
            oldRentPost.Price = UpdateInput.Price;
            oldRentPost.Area = UpdateInput.Area.Value;
            oldRentPost.Address = UpdateInput.Address;
            oldRentPost.IsNegotiatedPrice = UpdateInput.IsNegotiatedPrice;
            oldRentPost.Thumbnail = UpdateInput.Thumbnail;
            oldRentPost.RentPostRooms = postRooms;
            oldRentPost.RegionAddress = UpdateInput.Address;

            var oldFilePaths = UpdateInput.OldImagePaths;
            var deleteFiles = oldRentPost.Mediae.Where(x => !oldFilePaths.Contains(x.Path)).ToList();
            var deleteFilePaths = deleteFiles.Select(x => x.Path).ToList();

            foreach (var file in deleteFiles)
            {
                oldRentPost.Mediae.Remove(file);
            }
            for (int i = 0; i < oldFilePaths.Count; i++)
            {
                var media = oldRentPost.Mediae.FirstOrDefault(x=>x.Path.Equals(oldFilePaths[i]));
                media.Order = UpdateInput.OldFileIndex[i];
            }
            oldRentPost.Mediae.AddRange(mediaList);

            if (oldRentPost.Thumbnail == null && listPaths.Count > 0) { oldRentPost.Thumbnail = listPaths.First(); }

            _context.RentPosts.Update(oldRentPost);
            await _context.SaveChangesAsync();
            await _fileService.DeleteFileAsync(deleteFilePaths);
            
            return RedirectToPage("Details", new { id = oldRentPost.Id });
        }
    }
}