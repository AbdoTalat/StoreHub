using StoreHub.Core.Services.Interfaces;

namespace StoreHub.Core.Services.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageService(IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
        }
        public async Task<string?> SaveImageAsync(IFormFile ImageFile)
        {

            if (ImageFile == null || ImageFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
            var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            return $"/Images/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string ImagePath)
        {
            var filePath = Path.Combine(webHostEnvironment.WebRootPath, ImagePath.TrimStart('/'));

            if (!File.Exists(filePath))
            {
                return false;
            }
            try
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw new Exception(ex.Message);
            }

        }
    }
}
