using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Services;
using System.Threading.Tasks;

namespace PizzaStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        #region VerifyPayment
        /// <summary>
        /// Verifies the payment details.
        /// </summary>
        /// <param name="paymentDTO">The payment details to verify.</param>
        /// <returns>Result of the payment verification.</returns>
        [HttpPost("verify")]
        [ProducesResponseType(typeof(PaymentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                _logger.LogWarning("Received null paymentDTO");
                return BadRequest("Invalid payment details.");
            }

            try
            {
                var result = await _paymentService.VarifyPayment(paymentDTO);

                if (result == null)
                {
                    _logger.LogWarning("Payment verification failed.");
                    return StatusCode(500, "Payment verification failed.");
                }

                _logger.LogInformation("Payment verified successfully.");
                return Ok(result);
            }
            catch (PaymentServiceException ex)
            {
                _logger.LogError(ex, "Error verifying payment.");
                return StatusCode(500, "An error occurred while verifying the payment.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        #endregion
    }
}
