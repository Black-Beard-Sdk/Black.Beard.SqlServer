using Bb.SqlServer.Structures;
using Bb.SqlServerStructures;

namespace Bb.Extended
{

    public class EnvironmentModel : NamedModelDescriptor
    {

        public SqlServer.Structures.DatabaseStructure Database { get; set; }

        public ConnectionModel? Connection { get; private set; }

        public EnvironmentModel ResolveDatabase()
        {

            if (Connection == null)
                throw new NullReferenceException(nameof(Connection));

            var cnx = Connection.GetConnectionStringSetting();
            Database = DatabaseStructure.ResolveFromDatabase(cnx);

            return this;

        }

        public EnvironmentModel SetConnection(string server, string databaseName)
        {

            var models = this.Root();
            if (models == null)
                throw new KeyNotFoundException("models");

            var host = models.GetHost(server);
            if (host == null)
                throw new KeyNotFoundException(server);

            var database = host.GetDatabase(databaseName);

            this.Connection = database;

            return this;
        }

    }


}
