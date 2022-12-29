namespace Bb.SqlServer.Structures.Dacpacs
{
    public class AnnotationTypePropertyValue : PropertyValue
    {

        private AnnotationTypePropertyValue(string key)
            : base(key)
        {

        }

        public static AnnotationTypePropertyValue SqlInlineConstraintAnnotation = new AnnotationTypePropertyValue("SqlInlineConstraintAnnotation");
        public static AnnotationTypePropertyValue Empty = new AnnotationTypePropertyValue(string.Empty);

    }




}
