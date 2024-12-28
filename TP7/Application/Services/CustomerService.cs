using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP7.Domain.Entities;
using TP7.Infrastructure.Persistence.Repositories;
using TP7.Infrastructure.Persistence.DBContexts;
public class CustomerService 
{
    private readonly SQLiteContext _context;

    public CustomerService(SQLiteContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Membershiptype)
            .Include(c => c.Movies)
            .ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Membershiptype)
            .Include(c => c.Movies)
            .ThenInclude(m => m.Genre)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateCustomerAsync(Customer customer, Guid[] selectedMovies)
    {
        if (selectedMovies != null)
        {
            customer.Movies = await _context.Movies
                .Where(m => selectedMovies.Contains(m.Id))
                .ToListAsync();
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task EditCustomerAsync(Customer customer, Guid[] selectedMovies)
    {
        var existingCustomer = await _context.Customers
            .Include(c => c.Movies)
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        if (existingCustomer == null)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        existingCustomer.Name = customer.Name;
        existingCustomer.MembershiptypeId = customer.MembershiptypeId;

        // Update the associated movies
        existingCustomer.Movies.Clear();
        var selectedMovieEntities = await _context.Movies
            .Where(m => selectedMovies.Contains(m.Id))
            .ToListAsync();

        foreach (var movie in selectedMovieEntities)
        {
            existingCustomer.Movies.Add(movie);
        }        
        _context.Update(existingCustomer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null) return;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Membershiptype>> GetAllMembershipTypesAsync()
    {
        return await _context.Membershiptypes.ToListAsync();
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await _context.Movies.ToListAsync();
    }
}
