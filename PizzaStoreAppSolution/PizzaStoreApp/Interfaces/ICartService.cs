using PizzaStoreApp.Services;

namespace PizzaStoreApp.Interfaces
{
    public interface ICartService
    {
        public Task<CartReturnDTO> GetCartByUserId(int id);
        public Task<CartReturnDTO> AddToCart(AddToCartDTO addToCartDTO);
        public Task<CartReturnDTO> UpdateCart(UpdateCartItemDTO updateCartDTO );
        public Task<CartReturnDTO> RemoveFromCart(RemoveFromCartDTO removeFromCartDTO);
    }
}
