namespace Bb.Extended
{
    public class HostModel : ListNamedModelDescriptor<ConnectionModel>
    {


        public string? Instance { get; set; }

        
        public TypeHostEnum HostKind { get; set; }


        public HostModel AddCatalog(string databaseName)
        {

            var cnx = new ConnectionModel()
            {
                Name = databaseName,
                UserId = null,
                Password = null,
            };

            this.Add(cnx);

            return this;

        }

        public ConnectionModel? GetDatabase(string databaseName)
        {
            return this.FirstOrDefault(c => c.Name == databaseName);
        }

    }


}
