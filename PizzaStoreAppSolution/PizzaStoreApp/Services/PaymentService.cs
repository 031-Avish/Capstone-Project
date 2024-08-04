using PizzaStoreApp.Interfaces;
using Razorpay.Api;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Payment = PizzaStoreApp.Models.Payment;

namespace PizzaStoreApp.Services
{
    /// <summary>
    /// Service for managing payment operations, including verifying payments and updating order statuses.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<int, Payment> _paymentRepository;
        private readonly ILogger<PaymentService> _logger;
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentService"/> class.
        /// </summary>
        /// <param name="paymentRepository">Repository for managing payment data.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        /// <param name="orderService">Service for managing orders.</param>
        public PaymentService(IRepository<int, Payment> paymentRepository, ILogger<PaymentService> logger, IOrderService orderService)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
            _orderService = orderService;
        }

        #region VarifyPayment Method

        /// <summary>
        /// Verifies the payment using Razorpay and updates the payment and order status.
        /// </summary>
        /// <param name="paymentDTO">DTO containing payment details.</param>
        /// <returns>PaymentReturnDTO with payment details.</returns>
        /// <exception cref="PaymentServiceException">Thrown when an error occurs during payment verification.</exception>
        public async Task<PaymentReturnDTO> VarifyPayment(PaymentDTO paymentDTO)
        {
            try
            {
                _logger.LogInformation("Verifying payment...");

                string rakey = "rzp_test_Wdjw0UYgMSS5xq";
                string raksecret = "gIv7e7Ft6eAXFKGkXcxEneWJ";
                var options = new Dictionary<string, string>
                {
                    { "razorpay_payment_id", paymentDTO.RazorpayPaymentId },
                    { "razorpay_order_id", paymentDTO.RazorpayOrderId },
                    { "razorpay_signature", paymentDTO.RazorpaySignature }
                };
                var client = new RazorpayClient(rakey, raksecret);
                Utils.verifyPaymentSignature(options);

                var payment = new Payment
                {
                    OrderId = paymentDTO.OrderId,
                    Amount = paymentDTO.Amount,
                    RazorpayPaymentId = paymentDTO.RazorpayPaymentId,
                    RazorpayOrderId = paymentDTO.RazorpayOrderId,
                    RazorpaySignature = paymentDTO.RazorpaySignature,
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Success"
                };

                var addedPayment = await _paymentRepository.Add(payment);
                await _orderService.UpdateOrderStatus(paymentDTO.cartId, paymentDTO.OrderId, "Success");

                return new PaymentReturnDTO
                {
                    PaymentId = addedPayment.PaymentId,
                    OrderId = addedPayment.OrderId,
                    Amount = addedPayment.Amount,
                    PaymentDate = addedPayment.PaymentDate,
                    PaymentStatus = addedPayment.PaymentStatus
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while verifying payment.");

                var payment = new Payment
                {
                    OrderId = paymentDTO.OrderId,
                    Amount = paymentDTO.Amount,
                    RazorpayPaymentId = paymentDTO.RazorpayPaymentId,
                    RazorpayOrderId = paymentDTO.RazorpayOrderId,
                    RazorpaySignature = paymentDTO.RazorpaySignature,
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Failed"
                };

                var addedPayment = await _paymentRepository.Add(payment);
                await _orderService.UpdateOrderStatus(paymentDTO.cartId, paymentDTO.OrderId, "Failed");

                throw new PaymentServiceException("Error in verifying payment: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
