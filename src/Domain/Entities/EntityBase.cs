﻿namespace Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Key = Guid.NewGuid();
        }

        protected EntityBase(Guid key) => Key = key;

        public Guid Key { get; protected set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        protected void WasUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}
