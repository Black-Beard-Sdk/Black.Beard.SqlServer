﻿namespace Bb.SqlServer.Structures.Dacpacs
{
    public class ScriptContext
    {

        public ScriptContext(Variables variables)
        {
            this._variables = variables;
        }


        public DatabaseStructure? TargetState { get; set; }

        
        public bool CreateDatabase { get; set; }


        public string ReplaceVariables(string initialValue)
        {

            var var1 = initialValue;

            var list = _variables.ResolveVariableKeys(var1);
            int count = 0;

            while (list.Count > 0 && count < 15)
            {

                foreach (var item in list)
                {

                    var v1 = item.Trim('$');

                    if (this._variables.TryGetValue(v1, out var value))
                        var1 = var1.Replace(item, value);

                    else
                        var1 = var1.Replace(v1, item);

                }

                list = _variables.ResolveVariableKeys(var1);
                count++;

            }

            return var1;

        }

        private readonly Variables _variables;

    }



}