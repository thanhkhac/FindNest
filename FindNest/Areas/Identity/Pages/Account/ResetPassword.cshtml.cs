// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using FindNest.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FindNest.Areas.Identity.Pages.Account
{

    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Sai định dạng email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Mật khẩu yêu cầu tối thiểu {2} và tối đa {1} ký tự ", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Code { get; set; }
        }


        public IActionResult OnGet(string code = null)
        {
            if (code == null) { return BadRequest("A code must be supplied for password reset."); }
            Input = new InputModel
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded) { return RedirectToPage("./ResetPasswordConfirmation"); }

            foreach (var error in result.Errors) { ModelState.AddModelError(string.Empty, error.Description); }
            return Page();
        }
    }
}