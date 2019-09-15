namespace Smart.Data.Accessor.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Data.Accessor.Attributes;
    using Smart.Data.Accessor.Resolver.Providers;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Scopes;

    public sealed class AccessorMissingHandler : IMissingHandler
    {
        public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
        {
            if (!type.IsInterface || (type.GetCustomAttribute<DataAccessorAttribute>() == null))
            {
                return Enumerable.Empty<IBinding>();
            }

            return new[]
            {
                new Binding(type, new DataAccessorProvider(type), new SingletonScope(components), null, null, null)
            };
        }
    }
}
