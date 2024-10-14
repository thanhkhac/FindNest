using Microsoft.AspNetCore.Identity;

namespace FindNest.Config
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = $"Có lỗi xảy ra." };
        }
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Optimistic concurrency failure, object has been modified." };
        }
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(PasswordMismatch), Description = "Sai mật khẩu." };
        }
        public override IdentityError InvalidToken()
        {
            return new IdentityError { Code = nameof(InvalidToken), Description = "Token không hợp lệ" };
        }
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Người dùng với thông tin đăng nhập này đã tồn tại" };
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
                { Code = nameof(InvalidUserName), Description = $"User name '{userName}' không hợp lệ" };
        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' không hợp lệ." };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Tên người dùng '{userName}' đã tồn tại." };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' đã tồn tại." };
        }
        // public override IdentityError InvalidRoleName(string role)
        // {
        //     return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Role name '{role}' is invalid." };
        // }
        // public override IdentityError DuplicateRoleName(string role)
        // {
        //     return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Role name '{role}' is already taken." };
        // }
        // public override IdentityError UserAlreadyHasPassword()
        // {
        //     return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "User already has a password set." };
        // }
        // public override IdentityError UserLockoutNotEnabled()
        // {
        //     return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Lockout is not enabled for this user." };
        // }
        // public override IdentityError UserAlreadyInRole(string role)
        // {
        //     return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"User already in role '{role}'." };
        // }
        // public override IdentityError UserNotInRole(string role)
        // {
        //     return new IdentityError { Code = nameof(UserNotInRole), Description = $"User is not in role '{role}'." };
        // }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Mật khẩu yêu cầu tối thiểu {length} ký tự." };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
                { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Mật khẩu phải có ít nhất một ký tự không phải chữ và số." };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Mật khẩu phải có ít nhất một chữ số" };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Mật khẩu phải có ít nhất một chữ cái thường" };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Mật khẩu phải có ít nhất một chữ cái in hoa" };
        }
    }
}