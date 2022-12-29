namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacRelationships : DacListOfModel<DacRelationship>
    {

        public DacRelationships()
            : base(string.Empty)
        {

        }

        public DacRelationship Resolve(RelationshipNamePropertyValue name)
        {

            var item = GetOrCreate<DacRelationship>(c => c.Name.Value == name.Value);
            if (item != null && item?.Name == null)
                item.Name = name;

            return item;

        }

        protected override T1 Create<T1>()
        {
            return (T1)new DacRelationship();
        }

    }



}
