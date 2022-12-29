using System.Text;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class ModelBase
    {

        public ModelBase(string key)
        {
            Key = key;
        }

        protected T GetValue<T>(string key) where T : PropertyValue
        {

            if (!_properties.TryGetValue(key, out var p))
                _properties.Add(key, p = new Property(key));

            return (T)p.PropertyValue;

        }

        protected Property Get(string key)
        {

            if (!_properties.TryGetValue(key, out var p))
                _properties.Add(key, p = new Property(key));

            return p;

        }

        protected bool Exists(string key)
        {
            return _properties.ContainsKey(key);
        }


        public virtual XElement Serialize()
        {
            throw new NotImplementedException();
        }

        protected void Set<T>(string key, T value) where T : PropertyValue
        {

            if (!_properties.TryGetValue(key, out var p))
                _properties.Add(key, p = new Property(key));

            p.Set(value);

        }

        protected string Enquote(string type)
        {

            var t = type.Trim();
            var i = t.Split('.');

            StringBuilder sb = new StringBuilder(type.Length + 4);

            int c = 0;
            foreach (var item in i)
            {

                if (c > 0)
                    sb.Append($".");

                var txt = item.TrimStart('[', ' ').TrimEnd(']', ' ');
                sb.Append($"[{txt}]");

                c++;
            }

            return sb.ToString();
        }

        protected string Dequote(string type)
        {

            var t = type.Trim();
            var i = t.Split('.');

            StringBuilder sb = new StringBuilder(type.Length + 4);

            int c = 0;
            foreach (var item in i)
            {

                if (c > 0)
                    sb.Append($".");

                var txt = item.TrimStart('[', ' ').TrimEnd(']', ' ');
                sb.Append(txt);

                c++;
            }

            return sb.ToString();
        }

        private Dictionary<string, Property> _properties = new Dictionary<string, Property>();
        protected readonly string Key;

    }

}
