using FindNest.Repositories;

namespace FindNest.Config
{
    public static class ConfigService
    {
        public static void ConfigServices (this IServiceCollection services)
        {
            services.AddScoped<IRentPostRepository, RentPostRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
        }
    }
}