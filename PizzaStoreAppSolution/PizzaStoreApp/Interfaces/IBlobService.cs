namespace PizzaStoreApp.Interfaces
{
    public interface IBlobService
    {
        public Task<string> UploadFileAsync(IFormFile file);
    }
}
