using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Services;
using System.Threading.Tasks;

namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }
        //[Authorize(Roles = "User")]
        [HttpPost("add")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO addToCartDTO)
        {
            try
            {
                var cart = await _cartService.AddToCart(addToCartDTO);
                _logger.LogInformation("Added item to cart for user with id {UserId}.", addToCartDTO.UserId);
                return CreatedAtAction(nameof(GetCartByUserId), new { userId = addToCartDTO.UserId }, cart);
            }
            catch (CartItemAlreadyExistsException ex)
            {
                _logger.LogWarning("Cart item already exists: {Message}", ex.Message);
                return BadRequest(new ErrorModel(400, "Cart item already exists: " + ex.Message));
            }
            catch (CartServiceException ex)
            {
                _logger.LogError("Cart service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartItemRepositoryException ex)
            {
                _logger.LogError("Cart item repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartRepositoryException ex)
            {
                _logger.LogError("Cart repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaRepositoryException ex)
            {
                _logger.LogError("Pizza repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (ToppingRepositoryException ex)
            {
                _logger.LogError("Topping repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartItemToppingRepositoryException ex)
            {
                _logger.LogError("Cart item topping repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (BeverageRepositoryException ex)
            {
                _logger.LogError("Beverage repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (PizzaServiceException ex)
            {
                _logger.LogError("Pizza service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Not found: " + ex.Message));
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
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        //[Authorize(Roles = "User")]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            try
            {
                var cart = await _cartService.GetCartByUserId(userId);
                _logger.LogInformation("Retrieved cart for user with id {UserId}.", userId);
                return Ok(cart);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Cart not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart not found: " + ex.Message));
            }
            catch(CartRepositoryException ex)
            {
                _logger.LogError("Cart repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartServiceException ex)
            {
                _logger.LogError("Cart service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        //[Authorize(Roles = "User")]
        [HttpPost("remove")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartDTO removeFromCartDTO)
        {
            try
            {
                var cart = await _cartService.RemoveFromCart(removeFromCartDTO);
                _logger.LogInformation("Removed item from cart with CartItemId {CartItemId}.", removeFromCartDTO.CartItemId);
                return Ok(cart);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Cart item not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item not found: " + ex.Message));
            }
            catch (CartServiceException ex)
            {
                _logger.LogError("Cart service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError("Cart item not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item not found: " + ex.Message));
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError("cart item toppings not found : {Message} ", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item toppings not found: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
        //[Authorize(Roles = "User")]
        [HttpPut("update")]
        [ProducesResponseType(typeof(CartReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCart( UpdateCartItemDTO updateCartDTO)
        {
            try
            {
                var cart = await _cartService.UpdateCart(updateCartDTO);
                _logger.LogInformation("Updated cart item with CartItemId {CartItemId}.", updateCartDTO.CartItemId);
                return Ok(cart);
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
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError("Cart item not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item not found: " + ex.Message));
            }
            catch (CartItemToppingNotFoundException ex)
            {
                _logger.LogError("cart item toppings not found : {Message} ", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item toppings not found: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
    }
}
