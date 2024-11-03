using FindNest.Repositories;
using FindNest.Services;

namespace FindNest.Config
{
    public static class ConfigService
    {
        public static void ConfigServices (this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            
            services.AddScoped<IRentPostRepository, RentPostRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
        }
    }
}