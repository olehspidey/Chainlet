using System;

namespace Chainlet
{
    public delegate object DependencyResolver(Type type);

    internal class DependencyDescriptor
    {
        public DependencyDescriptor(Type dependencyType, Type implementationType)
        {
            DependencyType = dependencyType;
            ImplementationType = implementationType;
        }

        public Type DependencyType { get; }

        public Type ImplementationType { get; }
    }
}