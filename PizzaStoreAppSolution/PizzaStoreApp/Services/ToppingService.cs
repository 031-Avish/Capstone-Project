using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Services
{
    public class ToppingService : IToppingService
    {
        private readonly IRepository<int,Topping> _toppingRepository;

        public ToppingService(IRepository<int, Topping> toppingRepository)
        {
            _toppingRepository = toppingRepository;
        }

        public async Task<List<ToppingReturnDTO>> GetAllToppings()
        {
            try
            {
                var Toppings = await _toppingRepository.GetAll();
                if (Toppings == null)
                {
                    throw new NotFoundException("No Toppings found");
                }
                List<ToppingReturnDTO> ToppingReturnDTOs = new List<ToppingReturnDTO>();
                foreach (var Topping in Toppings)
                {
                    ToppingReturnDTOs.Add(MapToppingWithToppingReturnDTO(Topping));
                }
                return ToppingReturnDTOs;
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch(ToppingRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ToppingServiceException("Error : " + ex.Message);
            }
        }

        private ToppingReturnDTO MapToppingWithToppingReturnDTO(Topping topping)
        {
            return new ToppingReturnDTO
            {
                ToppingId = topping.ToppingId,
                Name = topping.ToppingName,
                Price = topping.Price,
                Image = topping.Image,
                IsAvailable = topping.IsAvailable,
                IsVegetarian = topping.IsVegetarian
            };
        }

        public async Task<ToppingReturnDTO> GetToppingByToppingId(int toppingId)
        {
            try
            {
                var Topping = await _toppingRepository.GetByKey(toppingId);
                if (Topping == null)
                {
                    throw new NotFoundException("No Topping found");
                }
                return MapToppingWithToppingReturnDTO(Topping);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (ToppingRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ToppingServiceException("Error : " + ex.Message);
            }
        }
    }
}
