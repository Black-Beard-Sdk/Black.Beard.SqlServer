namespace Bb.SqlServer.Structures.Dacpacs
{
    public class CollationLcidPropertyValue : PropertyValue
    {

        private CollationLcidPropertyValue(string key)
            : base(key)
        {

        }

        public static CollationLcidPropertyValue Collation1033 = new CollationLcidPropertyValue("1033");

    }




}
