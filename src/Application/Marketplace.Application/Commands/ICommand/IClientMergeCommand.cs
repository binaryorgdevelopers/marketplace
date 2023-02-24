
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.ICommand;

public interface IClientMergeCommand
{
    Task Merge();
    Clients GetClientsById(Guid? id);
}