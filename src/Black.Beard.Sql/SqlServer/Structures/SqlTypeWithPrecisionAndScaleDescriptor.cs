namespace Bb.SqlServer.Structures
{
    public class SqlTypeWithPrecisionAndScaleDescriptor : SqlTypeWithPrecisionDescriptor
    {

        public SqlTypeWithPrecisionAndScaleDescriptor(int argument1, int argument2)
            : base(argument1)
        {
            Argument2 = argument2;
        }

        public SqlTypeWithPrecisionAndScaleDescriptor(int argument1, int argument2, SqlDataTypeDescriptor type)
            : base(argument1, type)
        {
            Argument2 = argument2;
        }

        public SqlTypeWithPrecisionAndScaleDescriptor(int argument1, int argument2, string sqlLabel)
            : base(argument1, sqlLabel)
        {
            Argument2 = argument2;
        }

        public bool IsIdentity { get; set; }

        public int Argument2 { get; set; }

    }

}