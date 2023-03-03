 using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MiniORM;

public class ChangeTracker<T>
            where T : class, new()
{
    private readonly ICollection<T> allEntities; // Tracks update of the enitites
    private readonly ICollection<T> added; // Tracks added entites
    private readonly ICollection<T> removed; // Tracks removed entities

    private ChangeTracker()
    {
        this.added = new List<T>();
        this.removed = new List<T>();
    }

    public ChangeTracker(IEnumerable<T> entities)
        : this()
    {
        this.allEntities = this.CloneEntities(entities);
    }

    public IReadOnlyCollection<T> AllEntities
        => (IReadOnlyCollection<T>)this.allEntities;

    public IReadOnlyCollection<T> Added
       => (IReadOnlyCollection<T>)this.added;

    public IReadOnlyCollection<T> Removed
       => (IReadOnlyCollection<T>)this.removed;

    public void Add(T entity)
    {
        this.added.Add(entity);
    }

    public void Remove(T entity)
    {
        this.removed.Add(entity);
    }

    public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
    {
        ICollection<T> modifiedEntities = new List<T>();

        PropertyInfo[] primaryKeys = typeof(T)
            .GetProperties()
            .Where(pi => pi.HasAttribute<KeyAttribute>())
            .ToArray();

        foreach (T proxyEntity in this.AllEntities)
        {
            object[] primaryKeyValues = this.GetPrimaryKeyValues(primaryKeys, proxyEntity)
                .ToArray();

            // original entity in the DBSet
            T entity = dbSet.Entities
                .FirstOrDefault(e => this.GetPrimaryKeyValues(primaryKeys, e)
                .SequenceEqual(primaryKeyValues));

            if (entity == null)
            {
                continue;
            }

            bool isModified = this.IsModified(proxyEntity, entity);

            if (isModified)
            {
                modifiedEntities.Add(proxyEntity);
            }
        }

        return modifiedEntities;
    }

    private IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T proxyEntity)
    {
        return primaryKeys.Select(pk => pk.GetValue(proxyEntity));
    }

    private bool IsModified(T proxyEntity, T originalEntities)
    {
        PropertyInfo[] monitoredProperties = typeof(T)
            .GetProperties()
            .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .ToArray();

        PropertyInfo[] modifiedProperties = monitoredProperties
            .Where(pi => !Equals(pi.GetValue(proxyEntity), pi.GetValue(originalEntities)))
            .ToArray();

        return modifiedProperties.Any();
    }

    private ICollection<T> CloneEntities(IEnumerable<T> originalEntities)
    {
        ICollection<T> clonedEntities = new List<T>();

        PropertyInfo[] propertiesToClone = typeof(T)
            .GetProperties()
            .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .ToArray();

        foreach (T originalEntity in originalEntities)
        {
            T entityClone = Activator.CreateInstance<T>();

            foreach (PropertyInfo property in propertiesToClone)
            {
                object originalValue = property.GetValue(originalEntity);

                property.SetValue(entityClone, originalValue);
            }

            clonedEntities.Add(entityClone);
        }

        return clonedEntities;
    }
}