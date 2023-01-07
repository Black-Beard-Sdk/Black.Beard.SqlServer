namespace Bb.SqlServer.Structures
{

    public class SqlDataTypeDescriptor
    {

        public SqlDataTypeDescriptor() { }

        public SqlDataTypeDescriptor(string SqlLabel, Type type)
        {

            this.SqlLabel = SqlLabel;
            Type = type;

        }

        public string SqlLabel { get; }

        public Type Type { get; }


    }


}