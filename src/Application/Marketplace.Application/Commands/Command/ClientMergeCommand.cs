using Marketplace.Application.Commands.ICommand;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;

namespace Marketplace.Application.Commands.Command;

public class ClientMergeCommand : IClientMergeCommand
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IGenericRepository<Seller> _sellerRepository;
    private readonly IGenericRepository<Clients?> _clientRepository;

    public ClientMergeCommand(
        IGenericRepository<User> userRepository,
        IGenericRepository<Customer> customerRepository,
        IGenericRepository<Seller> sellerRepository, IGenericRepository<Clients?> clientRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _sellerRepository = sellerRepository;
        _clientRepository = clientRepository;
    }

    public async Task Merge()
    {
        var users = await _userRepository.GetAllAsync();
        var customers = await _customerRepository.GetAllAsync();
        var sellers = await _sellerRepository.GetAllAsync();
        var clients = await _clientRepository.GetAllAsync();
        if (clients.Count() != (users.Count() + customers.Count() + sellers.Count()))
        {
            foreach (var user in users)
            {
                var client = await _clientRepository.GetAsync(c => c.Email == user.Email);
                if (client is null)
                {
                    await _clientRepository.AddWithoutSaveAsync(
                        new Clients(
                            user.Id,
                            user.PasswordHash,
                            user.PhoneNumber,
                            user.Email,
                            user.Role,
                            user.FirstName,
                            user.LastName));
                }
            }

            foreach (var customer in customers)
            {
                var client = await _clientRepository.GetAsync(c => c.Email == customer.Email);
                if (client is null)
                {
                    await _clientRepository.AddWithoutSaveAsync(
                        new Clients(
                            customer.Id,
                            customer.PasswordHash,
                            customer.PhoneNumber,
                            customer.Email,
                            Roles.Customer,
                            customer.FirstName,
                            customer.LastName));
                }
            }

            foreach (var seller in sellers)
            {
                var client = await _clientRepository.GetAsync(c => c.Email == seller.Email);
                if (client is null)
                {
                    await _clientRepository.AddWithoutSaveAsync(
                        new Clients(
                            seller.Id,
                            seller.PasswordHash,
                            seller.PhoneNumber,
                            seller.Email,
                            Roles.Seller,
                            seller.FirstName,
                            seller.LastName));
                }
            }

            await _clientRepository.SaveChangesAsync();
        }
    }

    public Clients? GetClientsById(Guid? id) => _clientRepository.Get(c => c.Id == id);
}