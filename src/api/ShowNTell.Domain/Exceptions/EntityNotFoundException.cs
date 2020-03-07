using System;

namespace ShowNTell.Domain.Exceptions
{
    public class EntityNotFoundException<TKey> : Exception
    {
        public TKey EntityId { get; set; }

        public EntityNotFoundException(TKey entityId)
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, string message) : base(message)
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(TKey entityId, string message, Exception innerException) : base(message, innerException)
        {
            EntityId = entityId;
        }
    }
}