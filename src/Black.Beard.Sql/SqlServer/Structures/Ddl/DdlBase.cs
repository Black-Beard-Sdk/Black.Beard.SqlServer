using Bb.SqlServer.Structures.Dacpacs;
using System;

namespace Bb.SqlServer.Structures.Ddl
{
    public class DdlBase
    {

        public DdlBase(Writer writer, ScriptContext? ctx = null)
        {
            _writer = writer;
            _ctx = ctx ?? new ScriptContext();
        }

        public void Go()
        {
            //if (this.WriteGo)
            _writer.AppendGo();
        }

        public void Use(string databaseName)
        {
            //if (this.WriteGo)
            _writer.AppendUse(databaseName);
        }

        public IDisposable Indent(bool crlf = false)
        {

            return _writer.Indent(crlf);
        }

        protected void CommentLine(params string[] comments)
        {
            Append(" -- ");
            AppendEndLine(comments);
        }


        public void AppendEndLine(params object[] values)
        {           
           _writer.AppendEndLine(values);
        }
           
        public void Append(params object[] values)
        {
            _writer.Append(values);
        }

        public string AsLabel(params string[] values)
        {
            return _writer.FormatLabel(values);
        }

        public IDisposable IndentWithParentheses(bool crlf = false)
        {
            return _writer.IndentWithParentheses(crlf);
        }

        protected string Evaluate(bool test)
        {
            return test ? ON : OFF;
        }

        protected string Evaluate(bool test, string TrueValue, string FalseValue)
        {
            return test ? TrueValue : FalseValue;
        }

        protected string Evaluate(bool test, string TrueValue)
        {
            return test ? TrueValue : string.Empty;
        }

        //public bool WriteGo { get; set; } = true;

        protected readonly Writer _writer;
        protected readonly ScriptContext _ctx;

        protected const string ON = "ON";
        protected const string OFF = "OFF";
        protected const string ASC = "ASC";
        protected const string DESC = "DESC";
        protected const string CLUSTERED = "CLUSTERED";
        protected const string NONCLUSTERED = "NONCLUSTERED";
        protected const string UNIQUE = "UNIQUE";


    }
}