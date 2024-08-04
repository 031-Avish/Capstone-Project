using Microsoft.AspNetCore.Authorization;
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
    public class BeverageController : ControllerBase
    {
        private readonly IBeverageService _beverageService;
        private readonly ILogger<BeverageController> _logger;

        public BeverageController(IBeverageService beverageService, ILogger<BeverageController> logger)
        {
            _beverageService = beverageService;
            _logger = logger;
        }

        #region GetAllBeverages
        /// <summary>
        /// Retrieves all beverages.
        /// </summary>
        /// <returns>A list of all beverages.</returns>
        //[Authorize(Roles = "User")]
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<BeverageReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBeverages()
        {
            try
            {
                var beverages = await _beverageService.GetAllBeverages();
                _logger.LogInformation("Retrieved all beverages successfully.");
                return Ok(beverages);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No beverages found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError("Error in Beverage repository: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (BeverageServiceException ex)
            {
                _logger.LogError("Error in Beverage service: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region AddBeverage
        /// <summary>
        /// Adds a new beverage.
        /// </summary>
        /// <param name="beverageDTO">The details of the beverage to be added.</param>
        /// <returns>The added beverage.</returns>
        //[Authorize(Roles="Admin")]
        [HttpPost("add")]
        [ProducesResponseType(typeof(BeverageReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddBeverage([FromForm] BeverageDTO beverageDTO)
        {
            try
            {
                var beverage = await _beverageService.AddBeverage(beverageDTO);
                _logger.LogInformation("Added beverage with id {BeverageId} successfully.", beverage.BeverageId);
                return Ok(beverage);
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError("Error in Beverage repository: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (BeverageServiceException ex)
            {
                _logger.LogError("Error in Beverage service: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        #endregion

        #region GetBeverageById
        /// <summary>
        /// Retrieves a beverage by its ID.
        /// </summary>
        /// <param name="beverageId">The ID of the beverage to retrieve.</param>
        /// <returns>The requested beverage.</returns>
        [HttpGet("{beverageId}")]
        [ProducesResponseType(typeof(BeverageReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBeverageById(int beverageId)
        {
            try
            {
                var beverage = await _beverageService.GetBeverageByBeverageId(beverageId);
                _logger.LogInformation("Retrieved beverage with id {BeverageId} successfully.", beverageId);
                return Ok(beverage);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No beverage found with id {BeverageId}: {Message}", beverageId, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError("Error in Beverage repository: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (BeverageServiceException ex)
            {
                _logger.LogError("Error in Beverage service: {Message}", ex.Message);
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
