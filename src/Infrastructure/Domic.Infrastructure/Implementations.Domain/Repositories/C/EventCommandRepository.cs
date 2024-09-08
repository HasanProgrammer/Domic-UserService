using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Core.Domain.Enumerations;
using Domic.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.C;

//Config
public partial class EventCommandRepository : IEventCommandRepository
{
    private readonly SQLContext _Context;

    public EventCommandRepository(SQLContext Context) => _Context = Context;
}

//Command
public partial class EventCommandRepository
{
    public async Task AddAsync(Event entity, CancellationToken cancellationToken) 
        => await _Context.Events.AddAsync(entity, cancellationToken);

    public void Change(Event entity) => _Context.Events.Update(entity);

    public void Remove(Event entity) => _Context.Events.Remove(entity);
}

//Query
public partial class EventCommandRepository
{
    public IEnumerable<Event> FindAll() => _Context.Events.ToList();

    public IEnumerable<Event> FindAllWithOrdering(Order order, bool accending = true)
    {
        var entity = _Context.Events;

        return order switch {
            Order.Date => entity.OrderBy(@event => @event.CreatedAt_EnglishDate).ToList(),
            Order.Id   => entity.OrderBy(@event => @event.Id).ToList(),
            _ => null
        };
    }

    public async Task<IEnumerable<Event>> FindAllAsync(CancellationToken cancellationToken) 
        => await _Context.Events.ToListAsync(cancellationToken);

    public async Task<IEnumerable<Event>> FindAllWithOrderingAsync(Order order, bool accending = true,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var entity = _Context.Events;

        return order switch {
            Order.Date => await entity.OrderBy(@event => @event.CreatedAt_EnglishDate).ToListAsync(cancellationToken),
            Order.Id   => await entity.OrderBy(@event => @event.Id).ToListAsync(cancellationToken),
            _ => null
        };
    }
}