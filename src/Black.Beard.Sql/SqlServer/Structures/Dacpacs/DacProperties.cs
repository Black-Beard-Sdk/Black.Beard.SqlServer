namespace Bb.SqlServer.Structures.Dacpacs
{
    public class DacProperties : DacListOfModel<DacProperty>
    {

        public DacProperties()
            : base(string.Empty)
        {

        }


        public DacProperty Resolve(string name)
        {

            var item = GetOrCreate<DacProperty>(c => c.Name == name);
            if (string.IsNullOrEmpty(item?.Name))
                item.Name = name;

            return item;

        }

        protected override T1 Create<T1>()
        {
            return (T1)new DacProperty();
        }

    }


}
