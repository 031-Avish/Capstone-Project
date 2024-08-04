using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing topping operations, including retrieving all toppings and topping by ID.
    /// </summary>
    public class ToppingService : IToppingService
    {
        private readonly IRepository<int, Topping> _toppingRepository;
        private readonly ILogger<ToppingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToppingService"/> class.
        /// </summary>
        /// <param name="toppingRepository">Repository for managing topping data.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        public ToppingService(IRepository<int, Topping> toppingRepository, ILogger<ToppingService> logger)
        {
            _toppingRepository = toppingRepository;
            _logger = logger;
        }

        #region GetAllToppings Method

        /// <summary>
        /// Retrieves all toppings.
        /// </summary>
        /// <returns>List of <see cref="ToppingReturnDTO"/> objects.</returns>
        /// <exception cref="ToppingServiceException">Thrown when an error occurs while retrieving toppings.</exception>
        public async Task<List<ToppingReturnDTO>> GetAllToppings()
        {
            try
            {
                _logger.LogInformation("Retrieving all toppings.");

                var toppings = await _toppingRepository.GetAll();
                if (toppings == null)
                {
                    throw new NotFoundException("No toppings found");
                }

                List<ToppingReturnDTO> toppingReturnDTOs = new List<ToppingReturnDTO>();
                foreach (var topping in toppings)
                {
                    toppingReturnDTOs.Add(MapToppingWithToppingReturnDTO(topping));
                }
                return toppingReturnDTOs;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetAllToppings.");
                throw;
            }
            catch (ToppingRepositoryException ex)
            {
                _logger.LogError(ex, "Topping repository exception in GetAllToppings.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetAllToppings.");
                throw new ToppingServiceException("Error: " + ex.Message, ex);
            }
        }

        #endregion

        #region MapToppingWithToppingReturnDTO Method

        /// <summary>
        /// Maps a topping entity to a <see cref="ToppingReturnDTO"/>.
        /// </summary>
        /// <param name="topping">The topping entity.</param>
        /// <returns>A <see cref="ToppingReturnDTO"/> object.</returns>
        private ToppingReturnDTO MapToppingWithToppingReturnDTO(Topping topping)
        {
            return new ToppingReturnDTO
            {
                ToppingId = topping.ToppingId,
                Name = topping.ToppingName,
                Price = topping.Price,
                Image = topping.Image,
                IsAvailable = topping.IsAvailable,
                IsVegetarian = topping.IsVegetarian
            };
        }

        #endregion

        #region GetToppingByToppingId Method

        /// <summary>
        /// Retrieves a topping by its ID.
        /// </summary>
        /// <param name="toppingId">The topping ID.</param>
        /// <returns>A <see cref="ToppingReturnDTO"/> with the topping details.</returns>
        /// <exception cref="ToppingServiceException">Thrown when an error occurs while retrieving the topping.</exception>
        public async Task<ToppingReturnDTO> GetToppingByToppingId(int toppingId)
        {
            try
            {
                _logger.LogInformation("Retrieving topping by ID: {ToppingId}", toppingId);

                var topping = await _toppingRepository.GetByKey(toppingId);
                if (topping == null)
                {
                    throw new NotFoundException("No topping found with ID " + toppingId);
                }
                return MapToppingWithToppingReturnDTO(topping);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetToppingByToppingId.");
                throw;
            }
            catch (ToppingRepositoryException ex)
            {
                _logger.LogError(ex, "Topping repository exception in GetToppingByToppingId.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetToppingByToppingId.");
                throw new ToppingServiceException("Error: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
