namespace Bb.SqlServer.Structures
{
    public class PrimaryKeyDescriptor : IndexDescriptor
    {

        public PrimaryKeyDescriptor()
        {

        }

        public PrimaryKeyDescriptor(int capacity) : base(capacity)
        {

        }


        public bool IsDifferent(PrimaryKeyDescriptor target)
        {

            if (base.IsDifferent(target))
                return true;

            return false;

        }

    }



}