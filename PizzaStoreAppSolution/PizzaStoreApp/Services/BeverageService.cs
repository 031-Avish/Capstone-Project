using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;

namespace PizzaStoreApp.Services
{
    public class BeverageService:IBeverageService
    {
        private readonly IRepository<int, Beverage> _beverageRepository;

        public BeverageService(IRepository<int, Beverage> beverageRepository)
        {
            _beverageRepository = beverageRepository;
        }

        public async Task<List<BeverageReturnDTO>> GetAllBeverages()
        {
            try
            {
                var beverages = await _beverageRepository.GetAll();
                if (beverages == null)
                {
                    throw new NotFoundException("No beverages found");
                }
                List<BeverageReturnDTO> beverageReturnDTOs = new List<BeverageReturnDTO>();
                foreach (var beverage in beverages)
                {
                    beverageReturnDTOs.Add(MapBeverageWithBeverageReturnDTO(beverage));
                }
                return beverageReturnDTOs;
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BeverageServiceException("Error : " + ex.Message);
            }
        }

        private BeverageReturnDTO MapBeverageWithBeverageReturnDTO(Beverage beverage)
        {
            return new BeverageReturnDTO
            {
                BeverageId = beverage.BeverageId,
                Name = beverage.Name,
                Price = beverage.Price,
                Image = beverage.Image,
                IsAvailable = beverage.IsAvailable,
                
            };
        }

        public async Task<BeverageReturnDTO> GetBeverageByBeverageId(int beverageId)
        {
            try
            {
                var beverage = await _beverageRepository.GetByKey(beverageId);
                if (beverage == null)
                {
                    throw new NotFoundException("No beverage found with id " + beverageId);
                }
                return MapBeverageWithBeverageReturnDTO(beverage);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (BeverageRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BeverageServiceException("Error : " + ex.Message);
            }
        }       
    }
}
