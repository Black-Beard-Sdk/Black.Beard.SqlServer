using System.Text.RegularExpressions;

namespace Bb.SqlServer.Structures.Dacpacs
{
    public class Variables : Dictionary<string, string>
    {

        public Variables()
        {

        }

        public Variables(params KeyValuePair<string, string>[] collection)
            : base(collection)
        {

        }

        public List<string> ResolveVariableKeys(string text)
        {

            var list = new List<string>();

            MatchCollection result = _reg.Matches(text);

            foreach (Match match in result)
                list.Add(match.Value);

            return list;

        }

        private Regex _reg = new Regex(pattern, RegexOptions.None);
        private const string pattern = "\\$[^$]*\\$";

    }

}