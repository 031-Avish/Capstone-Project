using Microsoft.AspNetCore.Mvc;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Services;


namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }
        //[Authorize(Roles = "User")]
        [HttpPost("add")]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderDTO addOrderDTO)
        {
            try
            {
                var order = await _orderService.AddOrder(addOrderDTO);
                _logger.LogInformation("Order with id {OrderId} created successfully.", order.OrderId);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);
            }
            catch (OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartNotFoundException ex)
            {
                _logger.LogError("Cart not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart not found: " + ex.Message));
            }
            catch (CartItemNotFoundException ex)
            {
                _logger.LogError("Cart item not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Cart item not found: " + ex.Message));
            }
            catch (OrderDetailRepositoryException ex)
            {
                _logger.LogError("Order detail repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (OrderToppingRepositoryException ex)
            {
                _logger.LogError("Order topping repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartItemRepositoryException ex)
            {
                _logger.LogError("Cart item repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (CartItemToppingRepositoryException ex)
            {
                _logger.LogError("Cart item topping repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        //[Authorize(Roles = "User")]
        [HttpPut("cancel/{orderId}")]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = await _orderService.CancelOrder(orderId);
                _logger.LogInformation("Order with id {OrderId} canceled successfully.", orderId);
                return Ok(order);
            }
            catch (OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (OrderRepositoryException ex)
            {
                _logger.LogError("Order repository error: {Message}", ex.Message);
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
        [ProducesResponseType(typeof(List<OrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrdersForUser(int userId)
        {
            try
            {
                var orders = await _orderService.GetAllOrderForUser(userId);
                if (orders.Count == 0)
                {
                    _logger.LogWarning("No orders found for user with id {UserId}", userId);
                    return NotFound(new ErrorModel(404, "No Orders found for user"));
                }
                _logger.LogInformation("Retrieved all orders for user with id {UserId}.", userId);
                return Ok(orders);
            }
            catch (OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (OrderRepositoryException ex)
            {
                _logger.LogError("Order repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                if (orders.Count == 0)
                {
                    _logger.LogWarning("No orders found.");
                    return NotFound(new ErrorModel(404, "No Orders found"));
                }
                _logger.LogInformation("Retrieved all orders.");
                return Ok(orders);
            }
            catch (OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (OrderRepositoryException ex)
            {
                _logger.LogError("Order repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    _logger.LogWarning("Order with id {OrderId} not found.", orderId);
                    return NotFound(new ErrorModel(404, "Order not found"));
                }
                _logger.LogInformation("Retrieved order with id {OrderId}.", orderId);
                return Ok(order);
            }
            catch (OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (OrderRepositoryException ex)
            {
                _logger.LogError("Order repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("updateStatus")]
        [ProducesResponseType(typeof(OrderReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatusByAdmin(UpdateOrderDTO updateDTO)
        {
            try
            {
                var returnDTO = await _orderService.UpdateOrderStatusByAdmin(updateDTO);
                return  Ok(returnDTO);
            }
            catch(OrderServiceException ex)
            {
                _logger.LogError("Order service error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch(OrderRepositoryException ex)
            {
                _logger.LogError("Order repository error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Internal Server Error: " + ex.Message));
            }
            catch(OrderNotFoundException ex)
            {
                _logger.LogWarning("Order not found: {Message}", ex.Message);
                return NotFound(new ErrorModel(404, "Order not found: " + ex.Message));
            }
            catch(Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                return StatusCode(500, new ErrorModel(500, "Unexpected Error: " + ex.Message));
            }
        }
    }
}
