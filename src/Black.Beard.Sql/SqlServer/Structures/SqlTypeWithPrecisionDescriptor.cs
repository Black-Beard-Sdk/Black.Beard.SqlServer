namespace Bb.SqlServer.Structures
{
    public class SqlTypeWithPrecisionDescriptor : SqlTypeDescriptor
    {

        public SqlTypeWithPrecisionDescriptor(int argument1)
            : base()
        {
            Argument1 = argument1;
        }

        public SqlTypeWithPrecisionDescriptor(int argument1, string sqlLabel)
            : base(sqlLabel)
        {
            Argument1 = argument1;
        }

        public SqlTypeWithPrecisionDescriptor(int argument1, SqlDataTypeDescriptor type)
            : base(type)
        {
            Argument1 = argument1;
        }

        public int Argument1 { get; set; }
    }

}