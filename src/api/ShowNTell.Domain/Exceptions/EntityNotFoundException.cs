using System;

namespace ShowNTell.Domain.Exceptions
{
    public class EntityNotFoundException<TKey> : Exception
    {
        public TKey EntityId { get; set; }
        public Type EntityType { get; set; } = typeof(Object);

        public EntityNotFoundException(TKey entityId)
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, Type entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }

        public EntityNotFoundException(TKey entityId, Type entityType, string message) : base(message)
        {
            EntityId = entityId;
            EntityType = entityType;
        }

        public EntityNotFoundException(TKey entityId, Type entityType, string message, Exception innerException) : base(message, innerException)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }
}