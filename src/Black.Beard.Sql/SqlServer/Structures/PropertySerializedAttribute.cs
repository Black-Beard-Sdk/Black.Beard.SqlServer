using System.Reflection;

namespace Bb.SqlServer.Structures
{

    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]

    public class PropertySerializedAttribute : Attribute
    {

        public PropertySerializedAttribute(string text)
        {
            this.Text = text;
        }


        public string Text { get; }


        public string Serialize(object instance, PropertyInfo property)
        {

            var value = property.GetValue(instance);

            if (property.PropertyType.IsEnum)
            {

                var type = property.PropertyType;
                var n = Enum.GetName(type, value);
                var field = type.GetField(n, BindingFlags.Public | BindingFlags.Static);
                var attribute = field?.GetCustomAttribute(typeof(PropertySerializedAttribute), true) as PropertySerializedAttribute;
                if (attribute != null)
                {
                    return string.Format(Text, attribute.Text);
                }

                return string.Format(Text, value?.ToString());

            }

            return string.Format(Text, value);

        }

    }


}