namespace FindNest.Params
{
    public class UserSearchParam : SearchParams
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? FullName { get; set; }
        public bool IsBanned { get; set; }
    }
}