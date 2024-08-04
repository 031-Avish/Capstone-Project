using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing pizza operations, including retrieving, adding, and analyzing pizzas.
    /// </summary>
    public class PizzaService : IPizzaService
    {
        private readonly IRepository<int, Pizza> _pizzaRepo;
        private readonly IRepository<int, Size> _sizeRepo;
        private readonly IRepository<int, Crust> _crustRepo;
        private readonly IRepository<int, OrderDetail> _orderDetailRepo;
        private readonly IBlobService _blobService;
        private readonly ILogger<PizzaService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaService"/> class.
        /// </summary>
        /// <param name="pizza">Repository for managing pizza data.</param>
        /// <param name="size">Repository for managing size data.</param>
        /// <param name="crust">Repository for managing crust data.</param>
        /// <param name="blobService">Service for handling blob storage operations.</param>
        /// <param name="orderDetail">Repository for managing order details.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        public PizzaService(
            IRepository<int, Pizza> pizza,
            IRepository<int, Size> size,
            IRepository<int, Crust> crust,
            IBlobService blobService,
            IRepository<int, OrderDetail> orderDetail,
            ILogger<PizzaService> logger)
        {
            _pizzaRepo = pizza;
            _sizeRepo = size;
            _crustRepo = crust;
            _blobService = blobService;
            _orderDetailRepo = orderDetail;
            _logger = logger;
        }

        #region GetAllPizzawithSizeAndCrust Method

        /// <summary>
        /// Retrieves all pizzas along with their sizes and crusts.
        /// </summary>
        /// <returns>List of <see cref="PizzaReturnDTO"/> containing pizza, size, and crust details.</returns>
        /// <exception cref="PizzaServiceException">Thrown when an error occurs while retrieving pizzas, sizes, or crusts.</exception>
        public async Task<List<PizzaReturnDTO>> GetAllPizzawithSizeAndCrust()
        {
            try
            {
                _logger.LogInformation("Retrieving all pizzas with sizes and crusts.");

                var pizzas = await _pizzaRepo.GetAll();
                var sizes = await _sizeRepo.GetAll();
                var crusts = await _crustRepo.GetAll();

                if (pizzas == null)
                {
                    throw new NotFoundException("No pizzas found");
                }
                if (sizes == null)
                {
                    throw new NotFoundException("No sizes found");
                }
                if (crusts == null)
                {
                    throw new NotFoundException("No crusts found");
                }

                List<PizzaReturnDTO> pizzaReturnDTOs = new List<PizzaReturnDTO>();
                foreach (var pizza in pizzas)
                {
                    pizzaReturnDTOs.Add(MapPizzaWithPizzaReturnDTO(pizza, sizes, crusts));
                }
                return pizzaReturnDTOs;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetAllPizzawithSizeAndCrust.");
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in GetAllPizzawithSizeAndCrust.");
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError(ex, "Size repository exception in GetAllPizzawithSizeAndCrust.");
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError(ex, "Crust repository exception in GetAllPizzawithSizeAndCrust.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetAllPizzawithSizeAndCrust.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion

        #region MapPizzaWithPizzaReturnDTO Method

        /// <summary>
        /// Maps a pizza entity to a <see cref="PizzaReturnDTO"/>.
        /// </summary>
        /// <param name="pizza">The pizza entity.</param>
        /// <param name="sizes">The list of sizes.</param>
        /// <param name="crusts">The list of crusts.</param>
        /// <returns>A <see cref="PizzaReturnDTO"/> object.</returns>
        private PizzaReturnDTO MapPizzaWithPizzaReturnDTO(Pizza pizza, IEnumerable<Size> sizes, IEnumerable<Crust> crusts)
        {
            return new PizzaReturnDTO
            {
                PizzaId = pizza.PizzaId,
                Name = pizza.Name,
                Description = pizza.Description,
                BasePrice = pizza.BasePrice,
                Image = pizza.ImageUrl,
                IsAvailable = pizza.IsAvailable,
                IsVegetarian = pizza.IsVegetarian,
                CreatedAt = pizza.CreatedAt,
                sizes = sizes.Select(s => new SizeReturnDTO
                {
                    SizeId = s.SizeId,
                    SizeName = s.SizeName,
                    SizeMultiplier = s.SizeMultiplier,
                }).ToList(),
                crusts = crusts.Select(c => new CrustReturnDTO
                {
                    CrustId = c.CrustId,
                    CrustName = c.CrustName,
                    PriceMultiplier = c.PriceMultiplier,
                }).ToList()
            };
        }

        #endregion

        #region GetPizzaByPizzaId Method

        /// <summary>
        /// Retrieves a pizza by its ID along with sizes and crusts.
        /// </summary>
        /// <param name="pizzaId">The pizza ID.</param>
        /// <returns>A <see cref="PizzaReturnDTO"/> with pizza, size, and crust details.</returns>
        /// <exception cref="PizzaServiceException">Thrown when an error occurs while retrieving the pizza, sizes, or crusts.</exception>
        public async Task<PizzaReturnDTO> GetPizzaByPizzaId(int pizzaId)
        {
            try
            {
                _logger.LogInformation("Retrieving pizza by ID: {PizzaId}", pizzaId);

                var pizza = await _pizzaRepo.GetByKey(pizzaId);
                var sizes = await _sizeRepo.GetAll();
                var crusts = await _crustRepo.GetAll();

                if (pizza == null)
                {
                    throw new NotFoundException("No pizza found with ID " + pizzaId);
                }
                if (sizes == null)
                {
                    throw new NotFoundException("No sizes found");
                }
                if (crusts == null)
                {
                    throw new NotFoundException("No crusts found");
                }

                return MapPizzaWithPizzaReturnDTO(pizza, sizes, crusts);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError(ex, "Size repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError(ex, "Crust repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetPizzaByPizzaId.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAllNewPizza Method

        /// <summary>
        /// Retrieves all pizzas created in the last 7 days.
        /// </summary>
        /// <returns>List of new <see cref="PizzaReturnDTO"/> objects.</returns>
        public async Task<List<PizzaReturnDTO>> GetAllNewPizza()
        {
            try
            {
                var pizzas = await GetAllPizzawithSizeAndCrust();
                var newPizzas = pizzas.Where(p => p.CreatedAt >= DateTime.Now.AddDays(-7)).ToList();
                return newPizzas;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError(ex, "Size repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError(ex, "Crust repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetPizzaByPizzaId.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAllVegetarianPizza Method

        /// <summary>
        /// Retrieves all vegetarian pizzas.
        /// </summary>
        /// <returns>List of vegetarian <see cref="PizzaReturnDTO"/> objects.</returns>
        public async Task<List<PizzaReturnDTO>> GetAllVegetarianPizza()
        {
            try
            {
                var pizzas = await GetAllPizzawithSizeAndCrust();
                var vegetarianPizzas = pizzas.Where(p => p.IsVegetarian == true).ToList();
                return vegetarianPizzas;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError(ex, "Size repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError(ex, "Crust repository exception in GetPizzaByPizzaId.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetPizzaByPizzaId.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion

        #region GetMostSoldPizza Method

        /// <summary>
        /// Retrieves the most sold pizzas based on order count.
        /// </summary>
        /// <returns>List of top-selling <see cref="PizzaReturnDTO"/> objects.</returns>
        /// <exception cref="PizzaServiceException">Thrown when an error occurs while retrieving or processing the data.</exception>
        public async Task<List<PizzaReturnDTO>> GetMostSoldPizza()
        {
            try
            {
                _logger.LogInformation("Retrieving most sold pizzas.");

                var orders = await _orderDetailRepo.GetAll();
                var pizzaCount = orders
                    .Where(o => o.PizzaId != null) // Filter out orders with null PizzaId
                    .GroupBy(o => o.PizzaId)
                    .Select(g => new { PizzaId = g.Key, Count = g.Count() })
                    .OrderByDescending(pc => pc.Count)
                    .ToList();

                var pizzas = await GetAllPizzawithSizeAndCrust();
                var mostSoldPizzas = new List<PizzaReturnDTO>();
                int count = 0;
                foreach (var pizza in pizzaCount)
                {
                    count += 1;
                    var pizzaDetails = pizzas.FirstOrDefault(p => p.PizzaId == pizza.PizzaId);
                    if (pizzaDetails != null)
                    {
                        mostSoldPizzas.Add(pizzaDetails);
                    }
                    if (count == 5)
                        break;
                }

                return mostSoldPizzas;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in GetMostSoldPizza.");
                throw;
            }
            catch (OrderDetailRepositoryException ex)
            {
                _logger.LogError(ex, "OrderDetail repository exception in GetMostSoldPizza.");
                throw;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Not found exception in GetMostSoldPizza.");
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError(ex, "Size repository exception in GetMostSoldPizza.");
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError(ex, "Crust repository exception in GetMostSoldPizza.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in GetMostSoldPizza.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion

        #region AddPizzaByAdmin Method

        /// <summary>
        /// Adds a new pizza by admin.
        /// </summary>
        /// <param name="pizzaDTO">The pizza data transfer object.</param>
        /// <returns>A <see cref="AddPizzaReturnDTO"/> with the added pizza details.</returns>
        /// <exception cref="PizzaServiceException">Thrown when an error occurs while adding the pizza.</exception>
        public async Task<AddPizzaReturnDTO> AddPizzaByAdmin(PizzaDTO pizzaDTO)
        {
            try
            {
                _logger.LogInformation("Adding new pizza by admin.");

                var pizza = await _pizzaRepo.Add(new Pizza
                {
                    Name = pizzaDTO.Name,
                    Description = pizzaDTO.Description,
                    BasePrice = pizzaDTO.BasePrice,
                    ImageUrl = await _blobService.UploadFileAsync(pizzaDTO.ImageUrl),
                    IsVegetarian = pizzaDTO.IsVegetarian,
                });

                return new AddPizzaReturnDTO
                {
                    Name = pizza.Name,
                    Description = pizza.Description,
                    BasePrice = pizza.BasePrice,
                    ImageUrl = pizza.ImageUrl,
                    IsVegetarian = pizza.IsVegetarian,
                    IsAvailable = pizza.IsAvailable,
                    CreatedAt = pizza.CreatedAt,
                };
            }
            catch (BlobServiceException ex)
            {
                _logger.LogError(ex, "Blob service exception in AddPizzaByAdmin.");
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError(ex, "Pizza repository exception in AddPizzaByAdmin.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error in AddPizzaByAdmin.");
                throw new PizzaServiceException("Error : " + ex.Message, ex);
            }
        }

        #endregion
    }
}
