using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Models.Constants;
using Marketplace.Domain.Shared;

namespace Marketplace.Application.Commands.Command;

public class ClientMergeCommandHandler : ICommandHandler<ClientMergeCommand>
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IGenericRepository<Seller> _sellerRepository;
    private readonly IGenericRepository<Clients?> _clientRepository;

    public ClientMergeCommandHandler(
        IGenericRepository<User> userRepository,
        IGenericRepository<Customer> customerRepository,
        IGenericRepository<Seller> sellerRepository, IGenericRepository<Clients?> clientRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _sellerRepository = sellerRepository;
        _clientRepository = clientRepository;
    }

    public async Task<Result> Handle(ClientMergeCommand request, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetAllAsync()).ToArray();
        var customers = (await _customerRepository.GetAllAsync()).ToArray();
        var sellers = (await _sellerRepository.GetAllAsync()).ToArray();
        var clients = (await _clientRepository.GetAllAsync()).ToArray();
        if (clients.Length != (users.Length + customers.Length + sellers.Length))
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

        return Result.Success();
    }
}