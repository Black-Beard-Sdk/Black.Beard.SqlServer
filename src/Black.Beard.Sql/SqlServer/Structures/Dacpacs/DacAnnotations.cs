namespace Bb.SqlServer.Structures.Dacpacs
{
    public class DacAnnotations : DacListOfModel<DacAnnotation>
    {

        public DacAnnotations()
            : base(string.Empty)
        {

        }

        protected override T1 Create<T1>()
        {
            return (T1)new DacAnnotation();
        }

    }


}
