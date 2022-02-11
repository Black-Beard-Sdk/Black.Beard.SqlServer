# Black.Beard.SqlServer

[![Build status](https://ci.appveyor.com/api/projects/status/iikn91nm5bpfkjwo?svg=true)](https://ci.appveyor.com/project/gaelgael5/black-beard-sqlserver)


## Configuration
Manage connectionString configuration 


# Register all DbProviderFactory in the assemblies

```CSHARP

    // Parse all loaded assemblies and resolve all DbProviderFactory
    var list = ResolveFactoryHelper.GetFactoriesFromLoadedAssemblies();

    // Register the factories
    list.RegisterFactories();

```


```CSHARP

# Create and resolve a new ConnectionString.

```CSHARP

    var items = new ConnectionSettings()
    {
        ConnectionStringSettings = new ConnectionStringSettings()
         {
             new ConnectionStringSetting()
             {
                 Name = "Name1",
                 ConnectionString = "Data Source=.",
                 ProviderName = ""
             }
         }
    };

    ConnectionStringSetting? i = items["Name1"];

```
