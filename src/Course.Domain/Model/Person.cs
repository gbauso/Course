using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Model
{
    public abstract class Person : IDomain
    {
        protected Person()
        {

        }

        protected Person(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        public abstract void Validate();
    }
}
