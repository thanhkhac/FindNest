using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FindNest.Utilities
{
    public interface IFileService
    {
        Task<List<string>> SaveImagesAsync(List<IFormFile> formFiles, string folder);
        Task<string?> SaveImageAsync(IFormFile file, string folder);
        Task DeleteFileAsync(List<string> imagePaths);
    }

    public class FileService : IFileService
    {
        public async Task<List<string>> SaveImagesAsync(List<IFormFile> formFiles, string folder)
        {
            var stopwatch = Stopwatch.StartNew(); 

            var tasks = formFiles
                .Where(file => file.Length > 0)
                .Select(file => SaveImageAsync(file, folder)); 
            var listPaths = await Task.WhenAll(tasks); 
            stopwatch.Stop(); 
            Console.WriteLine($"Thời gian chạy của SaveImagesAsync: {stopwatch.Elapsed.TotalSeconds} giây"); 
            return listPaths.Where(path => path != null).Select(path => path!).ToList(); 
        }

        public async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            if (file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; 
                var path = Path.Combine(Directory.GetCurrentDirectory(), folder, fileName);
                await SaveImageToFileAsync(file, path); 
                return Path.Combine("/" + folder, fileName);
            }
            return null;
        }

        private async Task SaveImageToFileAsync(IFormFile file, string path)
        {
            using var image = await Image.LoadAsync(file.OpenReadStream());
            var quality = 30;
            var encoder = new JpegEncoder { Quality = quality };
            await image.SaveAsync(path, encoder);
        }
        
        public Task DeleteFileAsync(List<string> imagePaths) // Phương thức xóa ảnh
        {
            foreach (var path in imagePaths)
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath); 
                }
            }
            return Task.CompletedTask;
        }
    }
}