using TP4.Models;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(Guid id);
    Task CreateCustomerAsync(Customer customer, Guid[] selectedMovies);
    Task EditCustomerAsync(Customer customer, Guid[] selectedMovies);
    Task DeleteCustomerAsync(Guid id);
    Task<List<Membershiptype>> GetAllMembershipTypesAsync();
    Task<List<Movie>> GetAllMoviesAsync();
}
