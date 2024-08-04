using Microsoft.EntityFrameworkCore;
using PizzaStoreApp.Contexts;
using PizzaStoreApp.Interfaces;
using PizzaStoreApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaStoreApp.Exceptions.RepositoriesExceptions;

namespace PizzaStoreApp.Repositories
{
    /// <summary>
    /// Repository for managing payments.
    /// </summary>
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly PizzaAppContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger instance.</param>
        public PaymentRepository(PizzaAppContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Add Method

        /// <summary>
        /// Adds a new payment to the repository.
        /// </summary>
        /// <param name="item">The payment to add.</param>
        /// <returns>The added payment.</returns>
        /// <exception cref="PaymentRepositoryException">Thrown when an error occurs while adding the payment.</exception>
        public async Task<Payment> Add(Payment item)
        {
            try
            {
                _logger.LogInformation("Adding Payment to the database");
                _context.Add(item);
                await _context.SaveChangesAsync(true);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in adding Payment to the database: {Message}", ex.Message);
                throw new PaymentRepositoryException("Error in adding Payment to the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region DeleteByKey Method

        /// <summary>
        /// Deletes a payment by its key.
        /// </summary>
        /// <param name="key">The key of the payment to delete.</param>
        /// <returns>The deleted payment.</returns>
        /// <exception cref="PaymentNotFoundException">Thrown when the payment is not found.</exception>
        /// <exception cref="PaymentRepositoryException">Thrown when an error occurs while deleting the payment.</exception>
        public async Task<Payment> DeleteByKey(int key)
        {
            try
            {
                _logger.LogInformation("Deleting Payment from the database");
                var payment = await GetByKey(key);
                if (payment == null)
                {
                    throw new PaymentNotFoundException("Payment not found");
                }
                _context.Remove(payment);
                await _context.SaveChangesAsync(true);
                _logger.LogInformation("Payment deleted from the database");
                return payment;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError("Payment not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in deleting Payment from the database: {Message}", ex.Message);
                throw new PaymentRepositoryException("Error in deleting Payment from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetAll Method

        /// <summary>
        /// Gets all payments from the repository.
        /// </summary>
        /// <returns>A list of all payments.</returns>
        /// <exception cref="PaymentRepositoryException">Thrown when an error occurs while getting all payments.</exception>
        public async Task<IEnumerable<Payment>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all Payments from the database");

                var payments = await _context.Payments.ToListAsync();

                if (payments.Count <= 0)
                {
                    _logger.LogWarning("No Payments found in the database");
                    return Enumerable.Empty<Payment>();
                }

                _logger.LogInformation("Retrieved all Payments successfully");
                return payments;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Payments from the database: {Message}", ex.Message);
                throw new PaymentRepositoryException("Error in getting all Payments from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region GetByKey Method

        /// <summary>
        /// Gets a payment by its key.
        /// </summary>
        /// <param name="key">The key of the payment to retrieve.</param>
        /// <returns>The retrieved payment, or null if not found.</returns>
        /// <exception cref="PaymentRepositoryException">Thrown when an error occurs while getting the payment.</exception>
        public async Task<Payment> GetByKey(int key)
        {
            try
            {
                _logger.LogInformation("Getting Payment by key from the database");

                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == key);

                if (payment == null)
                {
                    _logger.LogWarning("Payment with key {Key} not found", key);
                    return null;
                }

                _logger.LogInformation("Payment with key {Key} retrieved successfully", key);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting Payment by key from the database: {Message}", ex.Message);
                throw new PaymentRepositoryException("Error in getting Payment by key from the database: " + ex.Message, ex);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Updates a payment in the repository.
        /// </summary>
        /// <param name="item">The payment to update.</param>
        /// <returns>The updated payment.</returns>
        /// <exception cref="PaymentNotFoundException">Thrown when the payment is not found.</exception>
        /// <exception cref="PaymentRepositoryException">Thrown when an error occurs while updating the payment.</exception>
        public async Task<Payment> Update(Payment item)
        {
            try
            {
                _logger.LogInformation("Updating Payment in the database");

                var payment = await GetByKey(item.PaymentId);
                if (payment == null)
                {
                    throw new PaymentNotFoundException("Payment not found");
                }

                _context.Entry(payment).State = EntityState.Detached;
                _context.Update(item);
                await _context.SaveChangesAsync(true);

                _logger.LogInformation("Payment updated in the database");
                return item;
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogError("Payment not found in the database: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in updating Payment in the database: {Message}", ex.Message);
                throw new PaymentRepositoryException("Error in updating Payment in the database: " + ex.Message, ex);
            }
        }

        #endregion
    }
}
