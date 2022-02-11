namespace Bb.Sql
{

    public class ConnectionStringSettings : List<ConnectionStringSetting>
    {


        public ConnectionStringSetting? this[string key] => GetConnectionString(key);


        public ConnectionStringSetting? GetConnectionString(string name)
        {
            return this.FirstOrDefault(x => x.Name == name);
        }


    }
}
