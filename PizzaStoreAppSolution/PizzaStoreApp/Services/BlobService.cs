using Azure.Storage.Blobs;
using PizzaStoreApp.Interfaces;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing blob storage operations.
    /// </summary>
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<BlobService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobService"/> class.
        /// </summary>
        /// <param name="configuration">Configuration instance to retrieve connection strings.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        public BlobService(IConfiguration configuration, ILogger<BlobService> logger)
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = "pizzaapp";
            _logger = logger;
        }

        #region UploadFileAsync Method

        /// <summary>
        /// Uploads a file to Azure Blob Storage and returns the file's URI.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI of the uploaded file.</returns>
        /// <exception cref="BlobServiceException">Thrown when an error occurs during file upload.</exception>
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                _logger.LogInformation("Starting file upload to blob storage...");

                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, true);

                var fileUri = blobClient.Uri.ToString();
                _logger.LogInformation($"File uploaded successfully. URI: {fileUri}");

                return fileUri;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading file to blob storage.");
                throw new BlobServiceException("Error in uploading file to blob storage: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
