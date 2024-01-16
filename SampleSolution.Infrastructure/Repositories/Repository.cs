using Microsoft.EntityFrameworkCore;
using SampleSolution.Core.Abstractions;
using SampleSolution.Domain.Entities;
using SampleSolution.Infrastructure.Contexts;

namespace SampleSolution.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    private readonly DbSet<TEntity> _entitySet;

    public Repository(AppDbContext context)
    {
        _entitySet = context.Set<TEntity>();
    }

    public async Task Add(TEntity entity)
    {
        await _entitySet.AddAsync(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _entitySet;
    }

    public async Task<TEntity?> FindById(string id)
    {
        return await _entitySet.FindAsync(id) ?? null;
    }

    public void Update(TEntity entity)
    {
        _entitySet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _entitySet.Remove(entity);
    }
}