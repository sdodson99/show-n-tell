using System;
using System.Runtime.Serialization;

namespace ShowNTell.Domain.Exceptions
{
    /// <summary>
    /// An exception for an entity not found.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get; set; }

        public EntityNotFoundException()
        {
            EntityType = typeof(Object);
        }

        public EntityNotFoundException(Type entityType)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(Type entityType, string message) : base(message)
        {
            EntityType = entityType;
        }

        public EntityNotFoundException(Type entityType, string message, Exception innerException) : base(message, innerException)
        {
            EntityType = entityType;
        }
    }

    /// <summary>
    /// An exception for an entity not found by an id of the specified type.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity id.</typeparam>
    public class EntityNotFoundException<TKey> : EntityNotFoundException
    {
        public TKey EntityId { get; set; }

        public EntityNotFoundException(TKey entityId) : base()
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, Type entityType) : base(entityType)
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, Type entityType, string message) : base(entityType, message)
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, Type entityType, string message, Exception innerException) : base(entityType, message, innerException)
        {
            EntityId = entityId;
        }
    }
}