using FindNest.Config;
using FindNest.Data;
using FindNest.Data.Models;
using FindNest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace FindNest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 104857600; // 100MB
            });
            
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<FindNestDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<IEmailSender, SendEmail>();
            
            builder.Services.ConfigServices();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });


            //Cấu hình Identity
            builder.Services.AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FindNestDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();
            builder.Services.AddRazorPages();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddDirectoryBrowser();
            builder.Services.AddAuthentication().AddGoogle(opts =>
            {
                opts.ClientId = builder.Configuration["Google:ClientId"];
                opts.ClientSecret = builder.Configuration["Google:ClientSecret"];
            });
            
            //Cấu hình API
            builder.Services.AddControllers();
            

            builder.Services.AddEndpointsApiExplorer(); // Tự động khám phá API endpoints
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FindNest API",
                    Version = "v1",
                    Description = "API for FindNest project",
                });
            });


            var app = builder.Build();


            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) { app.UseMigrationsEndPoint(); }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCors("AllowAllOrigins");
            var fileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "Upload"));
            var requestPath = "/Upload";
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = fileProvider,
                RequestPath = requestPath
            });
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                // c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindNest API v1");
            });
            app.Run();
        }
    }
}