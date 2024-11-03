using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FindNest.Services
{
    public interface IFileService
    {
        Task<List<string>> SaveImagesAsync(List<IFormFile> formFiles, string folder);
    }

    public class FileService : IFileService
    {
        public async Task<List<string>> SaveImagesAsync(List<IFormFile> formFiles, string folder)
        {
            List<string> listPaths = new List<string>();
            foreach (var file in formFiles)
            {
                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";  // Thêm extension
                    var path = Path.Combine(Directory.GetCurrentDirectory(), folder, fileName);

                    using (var image = await Image.LoadAsync(file.OpenReadStream()))
                    {
                        var quality = 75;
                        var encoder = new JpegEncoder { Quality = quality };
                        await image.SaveAsync(path, encoder);
                    }
                    listPaths.Add(Path.Combine("/" + folder, fileName));
                }
            }
            return listPaths;
        }

        public async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            List<string> listPaths = new List<string>();
            if (file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // Thêm extension
                var path = Path.Combine(Directory.GetCurrentDirectory(), folder, fileName);
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    var quality = 75;
                    var encoder = new JpegEncoder { Quality = quality };
                    await image.SaveAsync(path, encoder);
                }
                return (Path.Combine("/" + folder, fileName));
            }
            return null;
        }
    }
}