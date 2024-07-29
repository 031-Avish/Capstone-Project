using PizzaStoreApp.Exceptions.RepositoriesExceptions;
using PizzaStoreApp.Exceptions.ServiceException;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using System.Data;

namespace PizzaStoreApp.Services
{
    public class PizzaSevice : IPizzaService
    {
        private readonly IRepository<int, Pizza> _pizzaRepo;
        private readonly IRepository<int, Size> _sizeRepo;
        private readonly IRepository<int, Crust> _crustRepo;
        public PizzaSevice(IRepository<int,Pizza> pizza, IRepository<int,Size> size , IRepository<int,Crust> crust) {
            _pizzaRepo = pizza;
            _sizeRepo = size;
            _crustRepo = crust;
        }

        public async Task<List<PizzaReturnDTO>> GetAllPizzawithSizeAndCrust()
        {
            try
            {
                var pizzas = await _pizzaRepo.GetAll();
                var sizes = await _sizeRepo.GetAll();
                var crusts = await _crustRepo.GetAll();
                if (pizzas == null)
                {
                    throw new NotFoundException("No pizzas found");
                }
                if (sizes == null)
                {
                    throw new NotFoundException("No sizes found");
                }
                if (crusts == null)
                {
                    throw new NotFoundException("No crusts found");
                }
                List<PizzaReturnDTO> pizzaReturnDTOs = new List<PizzaReturnDTO>();
                foreach (var pizza in pizzas)
                {
                    pizzaReturnDTOs.Add(MapPizzaWithPizzaReturnDTO(pizza, sizes, crusts));
                }
                return pizzaReturnDTOs;
            }
            catch(NotFoundException ex)
            {
                throw;
            }
            catch (PizzaRepositoryException ex)
            {
                throw;
            }
            catch(SizeRepositoryException ex)
            {
                throw;
            }
            catch(CrustRepositoryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PizzaServiceException("Error : "+ex.Message);
            }

        }

        private PizzaReturnDTO MapPizzaWithPizzaReturnDTO(Pizza pizza, IEnumerable<Size> sizes, IEnumerable<Crust> crusts)
        {
            
            return new PizzaReturnDTO
            {
                PizzaId = pizza.PizzaId,
                Name = pizza.Name,
                Description = pizza.Description,
                BasePrice = pizza.BasePrice,
                Image = pizza.ImageUrl,
                IsAvailable = pizza.IsAvailable,
                IsVegetarian = pizza.IsVegetarian,
                CreatedAt = pizza.CreatedAt,
                sizes = sizes.Select(s => new SizeReturnDTO
                {
                    SizeId = s.SizeId,
                    SizeName = s.SizeName,
                    SizeMultiplier = s.SizeMultiplier,
                }).ToList(),
                crusts = crusts.Select(c => new CrustReturnDTO
                {
                    CrustId = c.CrustId,
                    CrustName = c.CrustName,
                    PriceMultiplier = c.PriceMultiplier,
                }).ToList()
            };
              
        }

        public async Task<PizzaReturnDTO> GetPizzaByPizzaId(int pizzaId)
        {
            try
            {
                var pizza = await _pizzaRepo.GetByKey(pizzaId);
                var sizes = await _sizeRepo.GetAll();
                var crusts = await _crustRepo.GetAll();
                if (pizza == null)
                {
                    throw new NotFoundException("No pizza found");
                }
                if (sizes == null)
                {
                    throw new NotFoundException("No sizes found");
                }
                if (crusts == null)
                {
                    throw new NotFoundException("No crusts found");
                }

                return MapPizzaWithPizzaReturnDTO(pizza, sizes, crusts);
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (PizzaRepositoryException ex)
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
            catch (Exception ex)
            {
                throw new PizzaServiceException("Error : " + ex.Message);
            }
        }

        public async Task<List<PizzaReturnDTO>> GetAllNewPizza()
        {
            // get all pizzas that are created in the last 7 days
            var pizzas = await GetAllPizzawithSizeAndCrust();
            var newPizzas = pizzas.Where(p => p.CreatedAt >= DateTime.Now.AddDays(-7)).ToList();
            return newPizzas;

        }

        public async Task<List<PizzaReturnDTO>> GetAllVegetarianPizza()
        {
            // get all pizzas that are vegetarian
            var pizzas = await GetAllPizzawithSizeAndCrust();
            var vegetarianPizzas = pizzas.Where(p => p.IsVegetarian == true).ToList();
            return vegetarianPizzas;
        }

        public Task<List<PizzaReturnDTO>> GetMostSoldPizza()
        {
            // get most sold pizzas based on the number of times in the orderdetails table 
            var pizzas = GetAllPizzawithSizeAndCrust();
            // group by pizzaId and count the number of times it appears in the orderdetails table
            // order by the count in descending order
            return null;
            
        }
    }
}
