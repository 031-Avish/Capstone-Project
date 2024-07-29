using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using PizzaStoreApp.Repositories;

namespace PizzaStoreApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<int,Order> _orderRepository;
        private readonly IRepository<int,OrderDetail> _orderDetailRepository;
        private readonly IRepository<int,OrderTopping> _orderToppingRepository;
        private readonly IRepository<int, Cart> _cartRepository;
        private readonly PizzaAppContext _context;
        private readonly IRepository<int, CartItem> _cartItemRepository;
        private readonly IRepository<int,CartItemTopping> _cartItemToppingRepository;

        public OrderService(PizzaAppContext context, IRepository<int, Order> orderRepository, 
            IRepository<int, OrderDetail> orderDetailRepository, IRepository<int, OrderTopping> orderToppingRepository,
            IRepository<int, Cart> cartRepository, IRepository<int, CartItem> cartItemRepository, IRepository<int, CartItemTopping> cartItemToppingRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderToppingRepository = orderToppingRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _cartItemToppingRepository = cartItemToppingRepository;
        }
        public async Task<OrderReturnDTO> AddOrder(AddOrderDTO addOrderDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fetch the cart based on the provided cartId
                var cart = await _cartRepository.GetByKey(addOrderDTO.CartId);
                if (cart == null)
                {
                    throw new OrderServiceException("Cart not found");
                }

                // Create a new order
                var newOrder = new Order
                {
                    UserId = addOrderDTO.UserId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = cart.TotalPrice,
                    DeliveryAddress = addOrderDTO.DeliveryAddress,
                    IsPickup = addOrderDTO.IsPickup,
                    OrderDetails = new List<OrderDetail>()
                };

                // Add the new order to the database
                var addedOrder = await _orderRepository.Add(newOrder);

                // Iterate through each CartItem and create OrderDetails and OrderToppings
                foreach (var cartItem in cart.CartItems)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        OrderId = addedOrder.OrderId,
                        PizzaId = cartItem.PizzaId,
                        CrustId = cartItem.CrustId,
                        SizeId = cartItem.SizeId,
                        BeverageId = cartItem.BeverageId,
                        Quantity = cartItem.Quantity,
                        SubTotal = cartItem.SubTotal,
                        OrderToppings = new List<OrderTopping>(),
                    };

                    // Add the new order detail to the database
                    var addedOrderDetail = await _orderDetailRepository.Add(newOrderDetail);

                    // If the cart item is a pizza and has toppings, add them to OrderToppings
                    if (cartItem.PizzaId.HasValue && cartItem.CartItemToppings.Any())
                    {
                        foreach (var cartItemTopping in cartItem.CartItemToppings)
                        {
                            var newOrderTopping = new OrderTopping
                            {
                                OrderDetailId = addedOrderDetail.OrderDetailId,
                                ToppingId = cartItemTopping.ToppingId,
                                Quantity = cartItemTopping.Quantity
                            };

                            // Add the new order topping to the database
                            await _orderToppingRepository.Add(newOrderTopping);
                            await _cartItemRepository.DeleteByKey(cartItemTopping.ToppingId);
                        }
                    }
                    await _cartItemRepository.DeleteByKey(cartItem.CartItemId);
                }
                var orderToReturn = await _orderRepository.GetByKey(addedOrder.OrderId);
                var orderReturnDto = await MapOrderToReturnDTO(orderToReturn);
                // Commit the transaction
                await transaction.CommitAsync();


                // Return the created order details

                

                return orderReturnDto;
            }
            catch (CartRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (OrderRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (OrderDetailRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (OrderToppingRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (CartItemRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (CartItemToppingRepositoryException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (CartItemNotFoundException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (CartNotFoundException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (OrderServiceException ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new OrderServiceException("Error in adding Order to the database: " + ex.Message, ex);
            }
        }

        public async Task<OrderReturnDTO> CancelOrder(int orderId)
        {
            try
            {
                var order =await _orderRepository.GetByKey(orderId);
                if (order == null)
                {
                    throw new OrderServiceException("Order not found");
                }
                order.Status= "Canceled";
                var updatedOrder =await _orderRepository.Update(order);
                var orderToReturn = await _orderRepository.GetByKey(updatedOrder.OrderId);
                return await MapOrderToReturnDTO(orderToReturn);
            }
            catch(OrderRepositoryException ex)
            {
                throw;
            }
            catch(OrderServiceException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new OrderServiceException("Error in canceling Order: " + ex.Message, ex);
            }
        }

        public async Task<List<OrderReturnDTO>> GetAllOrderForUser(int userId)
        {
            List<OrderReturnDTO> ordersToReturn = new List<OrderReturnDTO>();
            try
            {
                var orders=(await _orderRepository.GetAll()).Where(o => o.UserId == userId).ToList();
                if (orders.Count <= 0)
                {
                    throw new OrderServiceException("No Orders found for user");
                }
                foreach (var order in orders)
                {
                    ordersToReturn.Add(await MapOrderToReturnDTO(order));
                }
                return ordersToReturn;
            }
            catch (OrderRepositoryException ex)
            {
                throw;
            }
            catch (OrderServiceException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new OrderServiceException("Error in getting all Orders for user: " + ex.Message, ex);
            }
        }

        public async Task<List<OrderReturnDTO>> GetAllOrders()
        {
            List<OrderReturnDTO> ordersToReturn = new List<OrderReturnDTO>();
            try
            {
                var orders = await _orderRepository.GetAll();
                if (orders == null)
                {
                    throw new OrderServiceException("No Orders found");
                }
                foreach (var order in orders)
                {
                    ordersToReturn.Add(await MapOrderToReturnDTO(order));
                }
                return ordersToReturn;
            }
            catch (OrderRepositoryException ex)
            {
                throw;
            }
            catch (OrderServiceException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new OrderServiceException("Error in getting all Orders: " + ex.Message, ex);
            }
        }
        public async Task<OrderReturnDTO> GetOrderById(int orderId)
        {
            try
            {
                var order =await  _orderRepository.GetByKey(orderId);
                if (order == null)
                {
                    throw new OrderServiceException("Order not found");
                }
                var orderToReturn = await _orderRepository.GetByKey(order.OrderId);
                return await MapOrderToReturnDTO(orderToReturn);
            }
            catch (OrderRepositoryException ex)
            {
                throw;
            }
            catch (OrderServiceException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new OrderServiceException("Error in getting Order by Id: " + ex.Message, ex);
            }
        }

        private async Task<OrderReturnDTO> MapOrderToReturnDTO(Order order)
        {
            return new OrderReturnDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
     
                OrderItems = order.OrderDetails.Select(od => new OrderItemReturnDTO
                {
                    OrderItemId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    Pizza = od.PizzaId.HasValue ? new CartPizzaReturnDTO
                    {
                        PizzaId = od.PizzaId.Value,
                        Name = od.Pizza.Name,
                        BasePrice = od.Pizza.BasePrice,
                        Description = od.Pizza.Description,
                        IsVegetarian = od.Pizza.IsVegetarian,
                        CreatedAt = od.Pizza.CreatedAt,
                   
                    } : null,
                    Crust = od.CrustId.HasValue ? new CrustReturnDTO
                    {
                        CrustId = od.CrustId.Value,
                        CrustName = od.Crust.CrustName,
                        PriceMultiplier = od.Crust.PriceMultiplier
                    } : null,
                    Size = od.SizeId.HasValue ? new SizeReturnDTO
                    {
                        SizeId = od.SizeId.Value,
                        SizeMultiplier = od.Size.SizeMultiplier,
                        SizeName = od.Size.SizeName,
                    } : null,
                    Beverage = od.BeverageId.HasValue ? new BeverageReturnDTO
                    {
                        BeverageId = od.BeverageId.Value,
                        Name = od.Beverage.Name,
                        Price = od.Beverage.Price
                    } : null,
                    Quantity = od.Quantity,
                    Price = od.SubTotal,
                    DiscountPercent = 0,
                    Topping = od.OrderToppings.Select(ot => new OrderToppingReturnDTO
                    {
                        OrderToppingId = ot.OrderToppingId,
                        Name = ot.Topping.ToppingName,
                        ToppingId = ot.ToppingId,
                        Quantity = ot.Quantity,
                        Price = ot.Topping.Price
                    }).ToList()
                }).ToList(),
                Total = order.TotalPrice,
                OrderDate = order.OrderDate,
                OrderStatus = order.Status
            };
        }

    }
}
