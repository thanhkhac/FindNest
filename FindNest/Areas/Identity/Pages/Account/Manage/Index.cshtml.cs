// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FindNest.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindNest.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Họ và tên")]
            public string FullName { get; set; }

            [Phone]
            [Display(Name = "Số điện thoại liên hệ")]
            public string ContactPhoneNumber { get; set; }
            
            [FileExtensions(Extensions = "png,jpg,jpeg")]
            public IFormFile Avatar { get; set; }
        }

        private async Task LoadAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Username = user?.UserName;

            Input = new InputModel
            {
                ContactPhoneNumber = user?.ContactPhoneNumber,
                FullName = user?.FullName
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound($"ID '{_userManager.GetUserId(User)}' không tồn tại"); }

            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound($"Người dùng với ID '{_userManager.GetUserId(User)}' không tồn tại"); }

            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }
            user.ContactPhoneNumber = Input.ContactPhoneNumber;
            user.FullName = Input.FullName;

            await _userManager.UpdateAsync(user);
            //if (Input.ContactPhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.ContactPhoneNumber);
                
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Có lỗi xảy ra";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Cập nhật thông tin thành công";
            return RedirectToPage();
        }
    }
}