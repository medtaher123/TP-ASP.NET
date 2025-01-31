using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ChronoLink.Repositories
{
    public abstract class BaseRepository<TEntity, TDto> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        // Abstract method that must be implemented in derived classes to define the projection logic
        protected abstract TDto ProjectToDto(TEntity entity);
        protected abstract IQueryable<TDto> ProjectToDto(IQueryable<TEntity> query);

        // ApplyProjection method to handle entity-to-DTO transformation
        private IQueryable<TOutput> ApplyProjection<TOutput>(IQueryable<TEntity> query, bool useProjection)
            where TOutput : class
        {
            if (!useProjection && typeof(TOutput) == typeof(TEntity))
            {
                return query.Cast<TOutput>();
            }
            if (useProjection && typeof(TOutput) == typeof(TDto))
            {
                return ProjectToDto(query).Cast<TOutput>();
                query.Select(e => ProjectToDto(e)).Cast<TOutput>();
            }

            throw new InvalidOperationException(
                $"Invalid type parameter {typeof(TOutput).Name}. Must be {typeof(TEntity).Name} (no projection) or {typeof(TDto).Name} (with projection)."
            );
        }

        // Apply projection on a single entity
        private TOutput ApplyProjection<TOutput>(TEntity entity, bool useProjection) where TOutput : class
        {
            if (!useProjection && typeof(TOutput) == typeof(TEntity))
            {
                return entity as TOutput ?? throw new InvalidCastException();
            }
            if (useProjection && typeof(TOutput) == typeof(TDto))
            {
                return ProjectToDto(entity) as TOutput ?? throw new InvalidCastException();
            }

            throw new InvalidOperationException(
                $"Invalid type parameter {typeof(TOutput).Name}. Must be {typeof(TEntity).Name} (no projection) or {typeof(TDto).Name} (with projection)."
            );
        }

        // Generic method to get all entities with optional projection
        public async Task<List<TOutput>> GetAllAsync<TOutput>(bool useProjection)
            where TOutput : class
        {
            return await ApplyProjection<TOutput>(_dbSet.AsQueryable(), useProjection).ToListAsync();
        }

        // Overload to use TEntity by default
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await GetAllAsync<TEntity>(false);
        }

        // Get entity by ID with optional projection
        public async Task<TOutput?> GetByIdAsync<TOutput>(object id, bool useProjection) where TOutput : class
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return null;

            return ApplyProjection<TOutput>(entity, useProjection);
        }

        // Create a new entity
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        // Update an entity
        public async Task<TEntity?> UpdateAsync(object id, TEntity updatedEntity)
        {
            var existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
                return null;

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await _dbContext.SaveChangesAsync();
            return existingEntity;
        }

        // Delete an entity
        public async Task<bool> DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
