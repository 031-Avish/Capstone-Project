using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<int,Cart> _cartRepository;
        private readonly IRepository<int, CartItem> _cartItemRepository;
        private readonly IPizzaService _pizzaService;
        private readonly IRepository<int, Topping> _toppingRepository;
        private readonly IRepository<int,CartItemTopping> _cartItemToppingRepository;
        private readonly IRepository<int, Beverage> _beverageRepository;
        private PizzaAppContext _dbContext;

        public CartService(IRepository<int, Cart> cartRepository, IRepository<int, CartItem> cartItemRepository , PizzaAppContext dbContext,
            IPizzaService pizzaService,IRepository<int,Topping> toppingRepository, 
            IRepository<int,CartItemTopping> cartItemToppingRepository,IRepository<int,Beverage> beverageRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _dbContext = dbContext;
            _pizzaService = pizzaService;
            _toppingRepository = toppingRepository;
            _cartItemToppingRepository = cartItemToppingRepository;
            _beverageRepository = beverageRepository;
        }
        public async Task<CartReturnDTO> AddToCart(AddToCartDTO addToCartDTO)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // check if cart exists
                var carts = await _cartRepository.GetAll();
                Cart cart = null;
                if(carts!=null)
                {
                    cart = carts.Where(c => c.UserId == addToCartDTO.UserId).FirstOrDefault();
                }
                if(cart==null)
                {
                    // create new cart
                    cart = new Cart()
                    {
                        UserId = addToCartDTO.UserId,
                        TotalPrice = 0,
                        CartItems = new List<CartItem>()
                    };
                    await _cartRepository.Add(cart);
                }
                // get cart by key in detail
                var cart1 = await _cartRepository.GetByKey(cart.CartId);

                // check if cartitem has pizza id or beverage id
                if (addToCartDTO.PizzaId != null)
                {
                    await AddPizzaToCart(addToCartDTO, cart1);
                }
                else if (addToCartDTO.BeverageId != null)
                {
                    await AddBeverageToCart(addToCartDTO, cart1);
                }
                // get the cart by key in detail after adding item
                var returnCart = await _cartRepository.GetByKey(cart1.CartId);

                CartReturnDTO returnDTO = await MapCartToReturnDTO(returnCart);
                transaction.Commit();
                return returnDTO;
            }
            catch(CartItemRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (CartRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (ToppingRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (CartItemToppingRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch(PizzaServiceException ex )
            {
                transaction.Rollback();
                throw;
            }
            catch (NotFoundException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (CartItemAlreadyExistsException ex)
            {
                transaction.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new CartServiceException("Error in adding to cart"+ex.Message, ex);
            }
        }

        private async Task<CartReturnDTO> MapCartToReturnDTO(Cart returnCart)
        {
            return new CartReturnDTO
            {
                CartId = returnCart.CartId,

                CartItems = returnCart.CartItems.Select(ci => new CartItemReturnDTO
                {
                    CartId=returnCart.CartId,
                    CartItemId = ci.CartItemId,
                    Quantity = ci.Quantity,
                    Price = ci.SubTotal,
                    

                    Crust = ci.Crust != null ? new CrustReturnDTO
                    {
                        CrustId = (int)ci.CrustId,
                        CrustName = ci.Crust.CrustName,
                        PriceMultiplier = ci.Crust.PriceMultiplier
                    } : null,

                    Size = ci.Size != null ? new SizeReturnDTO
                    {
                        SizeId = (int)ci.SizeId,
                        SizeName = ci.Size.SizeName,
                        SizeMultiplier = ci.Size.SizeMultiplier
                    } : null,

                    Pizza = ci.Pizza != null ? new CartPizzaReturnDTO
                    {
                        PizzaId = (int)ci.PizzaId,
                        Name = ci.Pizza.Name,
                        Description = ci.Pizza.Description,
                        BasePrice = ci.Pizza.BasePrice,
                        IsVegetarian = ci.Pizza.IsVegetarian,
                        CreatedAt = ci.Pizza.CreatedAt,
                        Quantity = ci.Quantity,
                        imageUrl= ci.Pizza.ImageUrl,
                    } : null,

                    Beverage = ci.Beverage != null ? new BeverageReturnDTO
                    {
                        BeverageId = (int)ci.BeverageId,
                        Name = ci.Beverage.Name,
                        Price = ci.Beverage.Price,
                        Image = ci.Beverage.Image,
                    } : null,

                    Topping = ci.CartItemToppings != null ? ci.CartItemToppings.Select(cit => new CartToppingReturnDTO
                    {
                        ToppingId = cit.ToppingId,
                        Quantity = cit.Quantity,
                        Name = cit.Topping.ToppingName,
                        Price = cit.Topping.Price
                    }).ToList() : new List<CartToppingReturnDTO>()
                }).ToList(),
                Total = returnCart.TotalPrice
            };
        }

        private async Task AddBeverageToCart(AddToCartDTO addToCartDTO, Cart cart1)
        {
            var cartItem = cart1.CartItems.Where(ci => ci.BeverageId == addToCartDTO.BeverageId).FirstOrDefault();
            if (cartItem != null)
            {
                throw new CartItemAlreadyExistsException("Item already exists in the cart");
            }
            else
            {
                var cartItem1 = new CartItem()
                {
                    CartId = cart1.CartId,
                    BeverageId = addToCartDTO.BeverageId,
                    Quantity = addToCartDTO.Quantity,
                    SubTotal = 0
                };
                await _cartItemRepository.Add(cartItem1);
                var totalcost = (await _beverageRepository.GetByKey((int)addToCartDTO.BeverageId)).Price * addToCartDTO.Quantity;
                cartItem1.SubTotal = totalcost;
                await _cartItemRepository.Update(cartItem1);

                cart1.TotalPrice += totalcost;
                await _cartRepository.Update(cart1);
            }
        }

        private async Task AddPizzaToCart(AddToCartDTO addToCartDTO, Cart cart1)
        {
            //var cartItem = cart1.CartItems.Where(ci => ci.PizzaId == addToCartDTO.PizzaId).FirstOrDefault();
            // check if pizza crust size and toppings are exactly same as the one in cart

            var cartItem = cart1.CartItems.Where(ci => ci.PizzaId == addToCartDTO.PizzaId
                                                && ci.SizeId == addToCartDTO.SizeId 
                                                && ci.CrustId == addToCartDTO.CrustId).FirstOrDefault();
            if (cartItem!= null)
            {
                if(cartItem.CartItemToppings?.Count == addToCartDTO.ToppingIds?.Count )
                {
                    if(cartItem.CartItemToppings==null && addToCartDTO.ToppingIds == null)
                    {
                        throw new CartItemAlreadyExistsException("Item already exists in the cart");
                    }
                    cartItem = cart1.CartItems
                        .Where(ci => ci.CartItemToppings.OrderBy(cit => cit.ToppingId).
                        Select(cit => cit.ToppingId).SequenceEqual(addToCartDTO.ToppingIds.Keys.OrderBy(id => id))
                        && ci.CartItemToppings.OrderBy(cit => cit.ToppingId).
                        Select(cit => cit.Quantity).SequenceEqual(addToCartDTO.ToppingIds.OrderBy(kvp => kvp.Key)
                        .Select(kvp => kvp.Value))).FirstOrDefault();
                    if (cartItem != null)
                    {
                        throw new CartItemAlreadyExistsException("Item already exists in the cart");
                    }
                }  
            }
            
            
            var cartItem1 = new CartItem()
            {
                CartId = cart1.CartId,
                PizzaId = addToCartDTO.PizzaId,
                SizeId = addToCartDTO.SizeId,
                CrustId = addToCartDTO.CrustId,
                Quantity = addToCartDTO.Quantity,
                SubTotal = 0,
                CartItemToppings = new List<CartItemTopping>()
            };
            await _cartItemRepository.Add(cartItem1);

            if (addToCartDTO.ToppingIds != null)
            {
                foreach (var toppingId in addToCartDTO.ToppingIds)
                {
                    var cartItemTopping = new CartItemTopping()
                    {
                        CartItemId = cartItem1.CartItemId,
                        ToppingId = toppingId.Key,
                        Quantity = toppingId.Value
                    };
                    await _cartItemToppingRepository.Add(cartItemTopping);
                }
            }
                
            var totalcost = await CalculatePrice((int)addToCartDTO.PizzaId, (int)addToCartDTO.SizeId, 
                                                (int)addToCartDTO.CrustId, addToCartDTO.Quantity, addToCartDTO.ToppingIds);

            // check is discount avaialble if pizza is new then discount available

            cartItem1.SubTotal = totalcost;
            await _cartItemRepository.Update(cartItem1);

            cart1.TotalPrice += totalcost;
            await _cartRepository.Update(cart1);
            
        }

        private async Task<decimal> CalculatePrice(int PizzaId , int SizeId , int CrustId , int Quantity , Dictionary<int, int> ToppingIds)
        {
            var pizza = await _pizzaService.GetPizzaByPizzaId((int)PizzaId);
            var sizemultiplier = pizza.sizes.Where(s => s.SizeId == SizeId).FirstOrDefault().SizeMultiplier;
            var crustmultiplier = pizza.crusts.Where(c => c.CrustId ==CrustId).FirstOrDefault().PriceMultiplier;

            if (sizemultiplier == 0 || crustmultiplier == 0)
            {
                throw new CartServiceException("Size or Crust not found");
            }
            var price = pizza.BasePrice * sizemultiplier * crustmultiplier*Quantity;

            if (ToppingIds != null)
            {
                foreach (var toppingId in ToppingIds)
                {
                    var topping = await _toppingRepository.GetByKey(toppingId.Key);
                    if(topping == null)
                    {
                        throw new CartServiceException("Topping not found");
                    }
                    price += (topping.Price*toppingId.Value);
                }
            }  
            return price;
        }

        public async Task<CartReturnDTO> GetCartByUserId(int id)
        {
            try
            {
                var cart = (await _cartRepository.GetAll()).Where(c => c.UserId == id).FirstOrDefault();
                if (cart == null)
                {
                    throw new NotFoundException("Cart not found");
                }
                var returnCart = await _cartRepository.GetByKey(cart.CartId);
                return await MapCartToReturnDTO(returnCart);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (CartRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CartServiceException("Error in getting cart"+ex.Message, ex);
            }   
        }

        public async Task<CartReturnDTO> RemoveFromCart(RemoveFromCartDTO removeFromCartDTO)
        {
            try
            {
                
                // get cartItem by cartitemid
                var cartItem = await _cartItemRepository.GetByKey(removeFromCartDTO.CartItemId);
                if (cartItem == null)
                {
                    throw new NotFoundException("Cart item not found");
                }
                var totalPrice = cartItem.SubTotal;
                // remove all cartItemsToppings
                foreach (var topping in cartItem.CartItemToppings)
                {
                    await _cartItemToppingRepository.DeleteByKey(topping.CartItemToppingId);
                }
                // remove cartItem
                await _cartItemRepository.DeleteByKey(removeFromCartDTO.CartItemId);
                // get cart by key
                var cart = await _cartRepository.GetByKey(cartItem.CartId);
                // update total price
                cart.TotalPrice -= totalPrice;
                await _cartRepository.Update(cart);
                return await MapCartToReturnDTO(cart);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (CartItemRepositoryException ex)
            {
                throw;
            }
            catch (CartRepositoryException ex)
            {
                throw;
            }
            catch(CartItemToppingRepositoryException ex)
            {
                throw;
            }
            catch(CartItemNotFoundException ex)
            {
                throw;
            }
            catch(CartItemToppingNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CartServiceException("Error in removing from cart" + ex.Message, ex);
            }
        }

        public async Task<CartReturnDTO> UpdateCart(UpdateCartItemDTO updateCartDTO)
        {
            try
            {
                // get the cartitem by cartitemid
                var cartItem1 = await _cartItemRepository.GetByKey(updateCartDTO.CartItemId);

                if (updateCartDTO.PizzaId != null)
                {
                    await UpdatePizzaInCart(updateCartDTO, cartItem1);
                }
                else if (updateCartDTO.BeverageId != null)
                {
                    await UpdateBeverageInCart(updateCartDTO, cartItem1);
                }
                Cart cart1 = await _cartRepository.GetByKey(cartItem1.CartId);
                return await MapCartToReturnDTO(cart1);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (CartItemRepositoryException ex)
            {
                throw;
            }
            catch (CartRepositoryException ex)
            {
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                throw;
            }
            catch (ToppingRepositoryException ex)
            {
                throw;
            }
            catch (CartItemToppingRepositoryException ex)
            {
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                throw;
            }
            catch (PizzaServiceException ex)
            {
                throw;
            }
            catch (SizeRepositoryException ex)
            {
                throw;
            }
            catch (CrustRepositoryException ex)
            {
                throw;
            }
            catch(CartItemAlreadyExistsException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CartServiceException("Error in updating cart" + ex.Message, ex);
            }
        }

        private async Task UpdateBeverageInCart(UpdateCartItemDTO updateCartDTO, CartItem cartItem)
        {
            cartItem.Quantity = updateCartDTO.Quantity;
            var totalcost = (await _beverageRepository.GetByKey((int)cartItem.BeverageId)).Price * updateCartDTO.Quantity;
            var oldPrice = cartItem.SubTotal;
            cartItem.SubTotal = totalcost;
            await _cartItemRepository.Update(cartItem);
            var cart = await _cartRepository.GetByKey(cartItem.CartId);
            cart.TotalPrice = cart.TotalPrice - oldPrice + totalcost;
            await _cartRepository.Update(cart);
        }

        private async Task UpdatePizzaInCart(UpdateCartItemDTO updateCartDTO, CartItem cartItem1)
        {
            var cart = await _cartRepository.GetByKey(cartItem1.CartId);

            var cartItem = cart.CartItems.Where(ci => ci.PizzaId == updateCartDTO.PizzaId
                                            && ci.SizeId == updateCartDTO.SizeId
                                            && ci.CartItemId != updateCartDTO.CartItemId 
                                            && ci.CrustId == updateCartDTO.CrustId).FirstOrDefault();

            if (cartItem != null && cartItem.CartItemId != updateCartDTO.CartItemId)
            {
                if (cartItem.CartItemToppings?.Count == updateCartDTO.ToppingIds?.Count)
                {
                    if (cartItem.CartItemToppings == null && updateCartDTO.ToppingIds == null)
                    {
                        throw new CartItemAlreadyExistsException("This item already exists in the cart ");
                    }
                    cartItem = cart.CartItems
                        .Where(ci => ci.CartItemToppings.OrderBy(cit => cit.ToppingId).
                        Select(cit => cit.ToppingId).SequenceEqual(updateCartDTO.ToppingIds.Keys.OrderBy(id => id))
                        && ci.CartItemToppings.OrderBy(cit => cit.ToppingId).
                        Select(cit => cit.Quantity).SequenceEqual(updateCartDTO.ToppingIds.OrderBy(kvp => kvp.Key)
                        .Select(kvp => kvp.Value))).FirstOrDefault();
                    if (cartItem != null)
                    {
                        throw new CartItemAlreadyExistsException("This item already exists in the cart ");
                    }
                }
            }


            cartItem1.Quantity = updateCartDTO.Quantity;
            cartItem1.PizzaId = updateCartDTO.PizzaId;
            cartItem1.SizeId = updateCartDTO.SizeId;
            cartItem1.CrustId = updateCartDTO.CrustId;
            foreach (var topping in cartItem1.CartItemToppings)
            {
                await _cartItemToppingRepository.DeleteByKey(topping.CartItemToppingId);
            }
            if (updateCartDTO.ToppingIds != null)
            {
                foreach (var toppingId in updateCartDTO.ToppingIds)
                {
                    var cartItemTopping = new CartItemTopping()
                    {
                        CartItemId = cartItem1.CartItemId,
                        ToppingId = toppingId.Key,
                        Quantity = toppingId.Value
                    };
                    await _cartItemToppingRepository.Add(cartItemTopping);
                }
            }
            var oldPrice = cartItem1.SubTotal;
            var totalcost = await CalculatePrice((int)updateCartDTO.PizzaId, (int)updateCartDTO.SizeId,
                                                               (int)updateCartDTO.CrustId, updateCartDTO.Quantity, updateCartDTO.ToppingIds);
            cartItem1.SubTotal = totalcost;
            await _cartItemRepository.Update(cartItem1);

            var cart1 = await _cartRepository.GetByKey(cartItem1.CartId);
            cart1.TotalPrice = cart1.TotalPrice - oldPrice + totalcost;
            await _cartRepository.Update(cart1);
        }

        //private async Task updateAlreadyExistingPizza(CartItem oldCartItem, CartItem newCartItem,UpdateCartItemDTO updateCartItemDTO)
        //{
        //    oldCartItem.Quantity += updateCartItemDTO.Quantity;
        //    var oldPrice = oldCartItem.SubTotal;
        //    var totalcost = await CalculatePrice((int)updateCartItemDTO.PizzaId, (int)updateCartItemDTO.SizeId,
        //                                                                      (int)updateCartItemDTO.CrustId, oldCartItem.Quantity, updateCartItemDTO.ToppingIds);
        //    oldCartItem.SubTotal = totalcost;
        //    await _cartItemRepository.Update(oldCartItem);

        //    var cart = await _cartRepository.GetByKey(oldCartItem.CartId);
        //    cart.TotalPrice = cart.TotalPrice - oldPrice + totalcost;
        //    await _cartRepository.Update(cart);


        //    await RemoveFromCart(new RemoveFromCartDTO { CartItemId = newCartItem.CartItemId, UserId = updateCartItemDTO.UserId });
        //}
    }
}
