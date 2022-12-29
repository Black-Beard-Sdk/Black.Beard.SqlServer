using System.Text.RegularExpressions;

namespace Bb.SqlServerStructures
{

    public class ScriptConvert
    {

        public static ScriptItemList Get(string input)
        {

            ScriptItemList result = new ScriptItemList();

            var current = new ScriptItems();
            result.Add(current);

            string pattern = @"GO\r\n";

            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            int position = 0;

            foreach (Match m in Regex.Matches(input, pattern, options))
            {
                var script = new ScriptItem(position, input.Substring(position, m.Index - position));

                if (current.CanBeAdded(script))
                    current.Add(script);

                else
                {
                    current = new ScriptItems();
                    current.Add(script);
                    result.Add(current);
                }


                position = m.Index + m.Length;
            }

            return result;
        }

    }



}