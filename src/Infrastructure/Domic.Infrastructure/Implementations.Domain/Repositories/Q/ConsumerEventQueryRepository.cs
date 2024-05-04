using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Persistence.Contexts.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class ConsumerEventQueryRepository(SQLContext context) : IConsumerEventQueryRepository
{
    public ConsumerEventQuery FindById(object id) 
        => context.ConsumerEvents.FirstOrDefault(@event => @event.Id == id as string);
    
    public Task<ConsumerEventQuery> FindByIdAsync(object id, CancellationToken cancellationToken) 
        => context.ConsumerEvents.FirstOrDefaultAsync(@event => @event.Id == id as string, cancellationToken);

    public void Add(ConsumerEventQuery entity) => context.ConsumerEvents.Add(entity);
}