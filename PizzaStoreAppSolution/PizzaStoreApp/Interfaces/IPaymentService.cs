using PizzaStoreApp.Models;

namespace PizzaStoreApp.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentReturnDTO> VarifyPayment(PaymentDTO paymentDTO);

    }
}
