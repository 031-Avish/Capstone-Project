using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing beverages.
    /// </summary>
    public class BeverageService : IBeverageService
    {
        private readonly IRepository<int, Beverage> _beverageRepository;
        private readonly IBlobService _blobService;
        private readonly ILogger<BeverageService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageService"/> class.
        /// </summary>
        /// <param name="beverageRepository">The beverage repository.</param>
        /// <param name="blobService">The blob service for image storage.</param>
        /// <param name="logger">The logger instance.</param>
        public BeverageService(IRepository<int, Beverage> beverageRepository, IBlobService blobService, ILogger<BeverageService> logger)
        {
            _beverageRepository = beverageRepository;
            _blobService = blobService;
            _logger = logger;
        }

        #region GetAllBeverages Method

        /// <summary>
        /// Retrieves all beverages from the repository.
        /// </summary>
        /// <returns>A list of all beverages in DTO form.</returns>
        /// <exception cref="BeverageServiceException">Thrown when an error occurs while retrieving beverages.</exception>
        public async Task<List<BeverageReturnDTO>> GetAllBeverages()
        {
            try
            {
                _logger.LogInformation("Retrieving all beverages...");
                var beverages = await _beverageRepository.GetAll();
                if (beverages == null)
                {
                    _logger.LogWarning("No beverages found.");
                    throw new NotFoundException("No beverages found");
                }

                List<BeverageReturnDTO> beverageReturnDTOs = new List<BeverageReturnDTO>();
                foreach (var beverage in beverages)
                {
                    beverageReturnDTOs.Add(MapBeverageWithBeverageReturnDTO(beverage));
                }

                _logger.LogInformation("Beverages retrieved successfully.");
                return beverageReturnDTOs;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving beverages.");
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError(ex, "Repository error occurred while retrieving beverages.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving beverages.");
                throw new BeverageServiceException("Error: " + ex.Message);
            }
        }

        #endregion

        #region AddBeverage Method

        /// <summary>
        /// Adds a new beverage to the repository.
        /// </summary>
        /// <param name="beverageDTO">The beverage details.</param>
        /// <returns>The added beverage in DTO form.</returns>
        /// <exception cref="BeverageServiceException">Thrown when an error occurs while adding the beverage.</exception>
        public async Task<BeverageReturnDTO> AddBeverage(BeverageDTO beverageDTO)
        {
            try
            {
                _logger.LogInformation("Adding new beverage...");
                var beverage = new Beverage
                {
                    Name = beverageDTO.Name,
                    Price = beverageDTO.Price,
                    Image = await _blobService.UploadFileAsync(beverageDTO.ImageUrl),
                    IsAvailable = true
                };
                var addedBeverage = await _beverageRepository.Add(beverage);

                _logger.LogInformation("Beverage added successfully.");
                return MapBeverageWithBeverageReturnDTO(addedBeverage);
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError(ex, "Repository error occurred while adding beverage.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding beverage.");
                throw new BeverageServiceException("Error: " + ex.Message);
            }
        }

        #endregion

        #region GetBeverageByBeverageId Method

        /// <summary>
        /// Retrieves a beverage by its ID.
        /// </summary>
        /// <param name="beverageId">The ID of the beverage.</param>
        /// <returns>The beverage in DTO form.</returns>
        /// <exception cref="BeverageServiceException">Thrown when an error occurs while retrieving the beverage.</exception>
        public async Task<BeverageReturnDTO> GetBeverageByBeverageId(int beverageId)
        {
            try
            {
                _logger.LogInformation($"Retrieving beverage with ID: {beverageId}");
                var beverage = await _beverageRepository.GetByKey(beverageId);
                if (beverage == null)
                {
                    _logger.LogWarning($"No beverage found with ID: {beverageId}");
                    throw new NotFoundException("No beverage found with id " + beverageId);
                }

                _logger.LogInformation("Beverage retrieved successfully.");
                return MapBeverageWithBeverageReturnDTO(beverage);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving beverage.");
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError(ex, "Repository error occurred while retrieving beverage.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving beverage.");
                throw new BeverageServiceException("Error: " + ex.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Maps a <see cref="Beverage"/> to a <see cref="BeverageReturnDTO"/>.
        /// </summary>
        /// <param name="beverage">The beverage entity.</param>
        /// <returns>The beverage DTO.</returns>
        private BeverageReturnDTO MapBeverageWithBeverageReturnDTO(Beverage beverage)
        {
            return new BeverageReturnDTO
            {
                BeverageId = beverage.BeverageId,
                Name = beverage.Name,
                Price = beverage.Price,
                Image = beverage.Image,
                IsAvailable = beverage.IsAvailable
            };
        }

        #endregion
    }
}
