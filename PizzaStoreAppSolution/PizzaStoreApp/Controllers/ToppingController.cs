using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToppingController : ControllerBase
    {
        private readonly IToppingService _toppingService;
        private readonly ILogger<ToppingController> _logger;

        public ToppingController(IToppingService toppingService, ILogger<ToppingController> logger)
        {
            _toppingService = toppingService;
            _logger = logger;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<ToppingReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllToppings()
        {
            try
            {
                var toppings = await _toppingService.GetAllToppings();
                _logger.LogInformation("Retrieved all toppings successfully.");
                return Ok(toppings);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No toppings found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (ToppingRepositoryException ex)
            {
                _logger.LogError("Error in Topping repository: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (ToppingServiceException ex)
            {
                _logger.LogError("Error in Topping service: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("{toppingId}")]
        [ProducesResponseType(typeof(ToppingReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetToppingById(int toppingId)
        {
            try
            {
                var topping = await _toppingService.GetToppingByToppingId(toppingId);
                _logger.LogInformation("Retrieved topping with id {ToppingId} successfully.", toppingId);
                return Ok(topping);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("No topping found with id {ToppingId}: {Message}", toppingId, ex.Message);
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (ToppingRepositoryException ex)
            {
                _logger.LogError("Error in Topping repository: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (ToppingServiceException ex)
            {
                _logger.LogError("Error in Topping service: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
    }
}
