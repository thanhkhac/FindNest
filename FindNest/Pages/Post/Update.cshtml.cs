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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FindNest.Pages.Post
{
    public class RentPostUpdateViewModel
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
        public long? Price { get; set; }

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
    }

    [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;
        private readonly IRentPostRepository _rentPostRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly UserManager<User> _userManager;

        public List<RentCategory> RentCategories;
        public List<Region> CityRegions = new List<Region>();
        public List<Region> DistrictRegions = new List<Region>();
        public List<Region> WardRegions = new List<Region>();
        public List<Region> SelectedRegions = new List<Region>();

        public UpdateModel(FindNest.Data.FindNestDbContext context, IRentPostRepository rentPostRepository, IRegionRepository regionRepository,
            UserManager<User> userManager)
        {
            _context = context;
            _rentPostRepository = rentPostRepository;
            _regionRepository = regionRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public RentPostUpdateViewModel RentPostUpdateVm { get; set; } = new RentPostUpdateViewModel();


        public void Load()
        {
            RentCategories = _rentPostRepository.GetAllRentCategories().ToList();
            CityRegions = _regionRepository.GetChildRegions(null);
            if (RentPostUpdateVm.RegionId != null)
            {
                SelectedRegions = _regionRepository.GetParentRegionsRecursive((int)RentPostUpdateVm.RegionId);
                var city = SelectedRegions.FirstOrDefault(x => x.Level == 1);
                var district = SelectedRegions.FirstOrDefault(x => x.Level == 2);
                var ward = SelectedRegions.FirstOrDefault(x => x.Level == 3);
                if (city != null) { DistrictRegions = _regionRepository.GetChildRegions(city.Id); }
                if (district != null) { WardRegions = _regionRepository.GetChildRegions(district.Id); }
            }
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rentpost = _rentPostRepository.GetById(id.Value);
            
            Load();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Load();
            if (!ModelState.IsValid) { return Page(); }
            List<string> listPaths = new List<string>();
            var imageFiles = RentPostUpdateVm.Images;
            foreach (var file in imageFiles)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // Thêm phần mở rộng từ file gốc
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload", fileName);
            
                    using (var image = await Image.LoadAsync(file.OpenReadStream()))
                    {
                        var quality = 75; 

                        var encoder = new JpegEncoder
                        {
                            Quality = quality 
                        };

                        await image.SaveAsync(path, encoder); 
                    }

                    listPaths.Add(Path.Combine("/Upload", fileName));
                }
            }

            var mediaList = listPaths.Select((x, index) => new Media
            {
                MediaType = "IMG",
                Path = x,
                Order = RentPostUpdateVm.FileIndex.ElementAt(index),
            }).ToList();
            var user = await _userManager.GetUserAsync(User);
            List<RentPostRoom> postRooms = new List<RentPostRoom>();
            if (RentPostUpdateVm.BathroomQuantity > 0)
            {
                postRooms.Add(new RentPostRoom
                {
                    RoomId = RoomConst.Bathroom,
                    Quantity = RentPostUpdateVm.BathroomQuantity
                });
            }
            
            if (RentPostUpdateVm.BedroomQuantity > 0)
            {
                postRooms.Add(new RentPostRoom
                {
                    RoomId = RoomConst.Bedroom,
                    Quantity = RentPostUpdateVm.BedroomQuantity
                });
            }
            RentPost newRentPost = new RentPost
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = user.Id,
                UpdatedBy = user.Id,
                IsDeleted = false,
                Title = RentPostUpdateVm.Title,
                RegionId = RentPostUpdateVm.RegionId,
                RentCategoryId = RentPostUpdateVm.RentCategoryId,
                Price = RentPostUpdateVm.Price.Value,
                Area = RentPostUpdateVm.Area.Value,
                Address = RentPostUpdateVm.Address,
                IsNegotiatedPrice = RentPostUpdateVm.IsNegotiatedPrice,
                Thumbnail = RentPostUpdateVm.Thumbnail,
                IsHidden = false,
                RentPostRooms = postRooms,
                Mediae = mediaList,
                RegionAddress = RentPostUpdateVm.Address
            };
            if (newRentPost.Thumbnail == null && listPaths != null && listPaths.Count > 0) { newRentPost.Thumbnail = listPaths.First(); }


            _context.RentPosts.Add(newRentPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("Details", new { id = newRentPost.Id });
        }
    }
}