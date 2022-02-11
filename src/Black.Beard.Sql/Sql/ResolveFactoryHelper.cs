using System.Data.Common;
using System.Reflection;

namespace Bb.Sql
{

    public static class ResolveFactoryHelper
    {

        /// <summary>
        /// Register all dbfactories specified in the list
        /// </summary>
        /// <param name="factories"></param>
        public static void RegisterFactories(this List<(string, DbProviderFactory)> factories)
        {

            foreach (var factory in factories)
                if (!DbProviderFactories.GetProviderInvariantNames().Any(c => c == factory.Item1))
                    DbProviderFactories.RegisterFactory(factory.Item1, factory.Item2);

        }

        /// <summary>
        /// Parse all loaded assemblies and resolve all <see cref="DbProviderFactory" />
        /// </summary>
        /// <returns></returns>
        public static List<(string, DbProviderFactory)> GetFactoriesFromLoadedAssemblies()
        {
            List<(string, DbProviderFactory)> _items = new List<(string, DbProviderFactory)>();

            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())

            {
                IEnumerable<Type> types;
                if (assembly.IsDynamic)
                    types = assembly.GetTypes();

                else
                    types = assembly.GetExportedTypes();

                foreach (var exposedType in types)
                    if (typeof(DbProviderFactory).IsAssignableFrom(exposedType))
                    {

                        var propertyInstance = exposedType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                        if (propertyInstance != null)
                        {
                            var value = propertyInstance.GetValue(null);
                            if (value != null)
                                _items.Add((exposedType.FullName, (DbProviderFactory)value));
                        }
                        else
                        {
                            var FieldInstance = exposedType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
                            if (FieldInstance != null)
                            {
                                var value = FieldInstance.GetValue(null);
                                if (value != null)
                                    _items.Add((exposedType.FullName, (DbProviderFactory)value));
                            }
                        }
                    }
            }

            return _items;

        }
    }
}
