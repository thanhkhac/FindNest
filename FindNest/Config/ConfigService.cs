using FindNest.Repositories;
using FindNest.Services;
using FindNest.Utilities;
using Microsoft.Identity.Client;

namespace FindNest.Config
{
    public static class ConfigService
    {
        public static void ConfigServices (this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            
            services.AddScoped<IRentPostService, RentPostService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<ILikeRepository, LikeService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}