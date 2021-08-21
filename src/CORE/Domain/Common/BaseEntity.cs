using System;

namespace CORE.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set;}

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}