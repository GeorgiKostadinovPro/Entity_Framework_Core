using System.Collections;

namespace MiniORM;

public class DbSet<TEntity> : ICollection<TEntity>
    where TEntity : class, new()
{
    internal DbSet(IEnumerable<TEntity> entities)
    {
        this.ChangeTracker = new ChangeTracker<TEntity>(entities);

        this.Entities = entities.ToList();
    }

    internal ChangeTracker<TEntity> ChangeTracker { get; set; }

    internal ICollection<TEntity> Entities { get; set; }

    public int Count => this.Entities.Count;

    public bool IsReadOnly => this.Entities.IsReadOnly;

    public void Add(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
        }

        this.Entities.Add(entity);
        this.ChangeTracker.Add(entity); // Log added entity
    }

    public void Clear()
    {
        while (this.Entities.Any())
        {
            TEntity entityToRemove = this.Entities.First();
            this.Remove(entityToRemove);
        }
    }

    public bool Contains(TEntity entity)
    {
        return this.Entities.Contains(entity);
    }

    // We will not use this method, but it comes from the ICollection<T> interface
    public void CopyTo(TEntity[] array, int arrayIndex)
    {
        this.Entities.CopyTo(array, arrayIndex);
    }

    public bool Remove(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), ExceptionMessages.EntityNullException);
        }

        bool isRemoved = this.Entities.Remove(entity);

        if (isRemoved)
        {
            this.ChangeTracker.Remove(entity);
        }

        return isRemoved;
    }

    public void RemoveRange(IEnumerable<TEntity> entitiesToRemove)
    {
        foreach (TEntity entity in entitiesToRemove)
        {
            this.Remove(entity);
        }
    }

    public IEnumerator<TEntity> GetEnumerator()
    {
        foreach (TEntity entity in this.Entities)
        {
            yield return entity;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}