namespace PizzaStoreApp.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderReturnDTO>> GetAllOrders();
        public Task<OrderReturnDTO> GetOrderById(int orderId);
        public Task<OrderReturnDTO> AddOrder(AddOrderDTO addOrderDTO);
        public Task<OrderReturnDTO> CancelOrder(int orderId);

        public Task<List<OrderReturnDTO>> GetAllOrderForUser(int userId); 
    }
}
