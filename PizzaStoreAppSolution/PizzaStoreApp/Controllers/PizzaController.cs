using Microsoft.AspNetCore.Mvc;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Services;

namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(IPizzaService pizzaService, ILogger<PizzaController> logger)
        {
            _pizzaService = pizzaService;
            _logger = logger;
        }

        #region GetAllPizzas
        /// <summary>
        /// Retrieves all pizzas along with their size and crust information.
        /// </summary>
        /// <returns>List of pizzas with size and crust information.</returns>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllPizzawithSizeAndCrust();
                _logger.LogInformation("Retrieved all pizzas with size and crust successfully.");
                return Ok(pizzas);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizzas found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region GetPizzaById
        /// <summary>
        /// Retrieves a pizza by its ID.
        /// </summary>
        /// <param name="pizzaId">ID of the pizza to retrieve.</param>
        /// <returns>Details of the pizza with the specified ID.</returns>
        [HttpGet("{pizzaId}")]
        [ProducesResponseType(typeof(PizzaReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPizzaById(int pizzaId)
        {
            try
            {
                var pizza = await _pizzaService.GetPizzaByPizzaId(pizzaId);
                _logger.LogInformation("Retrieved pizza with ID {PizzaId} successfully.", pizzaId);
                return Ok(pizza);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizza found with ID {PizzaId}: {Message}", pizzaId, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region GetAllNewPizzas
        /// <summary>
        /// Retrieves all newly added pizzas.
        /// </summary>
        /// <returns>List of newly added pizzas.</returns>
        [HttpGet("new")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNewPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllNewPizza();
                _logger.LogInformation("Retrieved all new pizzas successfully.");
                return Ok(pizzas);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizzas found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region GetAllVegetarianPizzas
        /// <summary>
        /// Retrieves all vegetarian pizzas.
        /// </summary>
        /// <returns>List of vegetarian pizzas.</returns>
        [HttpGet("vegetarian")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllVegetarianPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllVegetarianPizza();
                _logger.LogInformation("Retrieved all vegetarian pizzas successfully.");
                return Ok(pizzas);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizzas found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region GetMostSoldPizzas
        /// <summary>
        /// Retrieves the most sold pizzas.
        /// </summary>
        /// <returns>List of most sold pizzas.</returns>
        [HttpGet("most-sold")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMostSoldPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetMostSoldPizza();
                _logger.LogInformation("Retrieved most sold pizzas successfully.");
                return Ok(pizzas);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizzas found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region AddPizzaByAdmin
        /// <summary>
        /// Adds a new pizza by the admin.
        /// </summary>
        /// <param name="pizzaDTO">Details of the pizza to add.</param>
        /// <returns>Details of the added pizza.</returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PizzaReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPizzaByAdmin([FromForm] PizzaDTO pizzaDTO)
        {
            try
            {
                var pizza = await _pizzaService.AddPizzaByAdmin(pizzaDTO);
                _logger.LogInformation("Added pizza with ID  successfully.");
                return Ok(pizza);
            }
            catch (BlobServiceException ex)
            {
                _logger.LogError("Blob service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No pizzas found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                _logger.LogError("Size repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                _logger.LogError("Crust repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion
    }
}
