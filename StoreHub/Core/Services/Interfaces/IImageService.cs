namespace StoreHub.Core.Services.Interfaces
{
    public interface IImageService
    {
        public Task<string?> SaveImageAsync(IFormFile Image);
        public Task<bool> DeleteImageAsync(string ImagePath);
    }
}
