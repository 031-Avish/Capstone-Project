using Microsoft.AspNetCore.Mvc;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllPizzawithSizeAndCrust();
                return Ok(pizzas);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("{pizzaId}")]
        [ProducesResponseType(typeof(PizzaReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPizzaById(int pizzaId)
        {
            try
            {
                var pizza = await _pizzaService.GetPizzaByPizzaId(pizzaId);
                return Ok(pizza);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (SizeRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CrustRepositoryException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("new")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNewPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllNewPizza();
                return Ok(pizzas);
            }
            catch (PizzaServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("vegetarian")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllVegetarianPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetAllVegetarianPizza();
                return Ok(pizzas);
            }
            catch (PizzaServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("most-sold")]
        [ProducesResponseType(typeof(List<PizzaReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMostSoldPizzas()
        {
            try
            {
                var pizzas = await _pizzaService.GetMostSoldPizza();
                return Ok(pizzas);
            }
            catch (PizzaServiceException ex)
            {
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
    }
}
