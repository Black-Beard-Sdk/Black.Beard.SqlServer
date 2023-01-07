namespace Bb.SqlServer.Structures
{


    


    public class PrimaryKeyListDescriptor : ListModelDescriptor<PrimaryKeyDescriptor>
    {



        public void For(Action<PrimaryKeyDescriptor> value)
        {
            foreach (var item in this)
                if (value != null)
                    value(item);
        }

    }



}