namespace Bb.SqlServer.Structures
{
    public class FileGroupListDescriptor : ListModelDescriptor<FileGroupDescriptor>
    {

        public FileGroupListDescriptor() : base()
        {

        }


        public FileGroupListDescriptor(int capacity) : base(capacity)
        {

        }

        public void AddIfNotExists(string name)
        {

            var item = this.Where(c => c.Name == name).ToList();

            if (item.Count() == 0)
                Add(new FileGroupDescriptor() { Name = name });

        }

        public void For(Action<FileGroupDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }


}