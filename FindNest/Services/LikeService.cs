using FindNest.Data;

namespace FindNest.Repositories
{

    public interface ILikeRepository
    {
        List<int> GetUserLikedPostId(string userId);
    }
    public class LikeService : ILikeRepository
    {
        private readonly FindNestDbContext _context;
        
        public LikeService(FindNestDbContext context)
        {
            _context = context;
        }

        public List<int> GetUserLikedPostId(string userId)
        {
            return _context.Likes.Where(x=>x.UserId.Equals(userId)).Select(x=> x.RentPostId).ToList();
        }
    }
}