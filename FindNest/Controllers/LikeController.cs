using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindNest.Data.Models;
using FindNest.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FindNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly FindNestDbContext _context;
        private readonly UserManager<User> _userManager;

        public LikesController(FindNestDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("{rentPostId}")]
        public async Task<IActionResult> ToggleLike(int rentPostId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var userId =user.Id;


            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.RentPostId == rentPostId);

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return Ok("Post unliked.");
            }
            else
            {
                var like = new Like { UserId = userId, RentPostId = rentPostId, LikedDate = DateTime.Now };
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return Ok("Post liked.");
            }
        }
    }
}