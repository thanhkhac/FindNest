using FindNest.Data;

namespace FindNest.Repositories
{

    public interface ILikeRepository
    {
        List<int> GetUserLikedPostId(string userId);
        bool DeleteLike(string userId, int postId); 
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
        
        public bool DeleteLike(string userId, int postId)
        {
            var like = _context.Likes
                .FirstOrDefault(x => x.UserId == userId && x.RentPostId == postId);
        
            if (like != null)
            {
                _context.Likes.Remove(like); 
                _context.SaveChanges(); 
                return true; 
            }

            return false; 
        }
    }
}