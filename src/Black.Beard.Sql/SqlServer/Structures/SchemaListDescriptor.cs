namespace Bb.SqlServer.Structures
{


    public class SchemaListDescriptor : ListModelDescriptor<SchemaDescriptor>
    {

        public SchemaListDescriptor() : base()
        {

        }


        public SchemaListDescriptor(int capacity) : base(capacity)
        {

        }



        public void AddIfNotExists(string schema, string parent)
        {

            var item = this.Where(c => c.Name == schema).ToList();

            if (item.Count() == 0)
            {

                Add(new SchemaDescriptor() { Name = schema, Parent = parent });

            }
            else
            {

            }

        }


        public void For(Action<SchemaDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }


    }


}