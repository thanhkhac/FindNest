namespace FindNest.Config
{
    public static class Message
    {
        public static string UserIdNotFound(string? userId)
        {
            return "Không tìm thấy người dùng với Id " + userId;
        }

        public static string IncorrectPassword = "Sai mật khẩu";
        public static string ErrorMessage = "Có lỗi xảy ra";
    }
}