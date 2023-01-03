using System.Text;

namespace Bb.SqlServer
{

    public class Writer
    {

        public Writer(StringBuilder sb)
        {
            _sb = sb;
            _index = 0;
        }

        public Writer Append(params object[] values)
        {
            foreach (var value in values)
                _sb.Append(value);
            return this;
        }

     

        public Writer AppendGo()
        {
            CleanIndent();
            _sb.AppendLine("GO");
            _sb.AppendLine();
            return this;
        }

        public Writer AppendUse(string databaseName)
        {
            CleanIndent();

            AppendEndLine("USE ", FormatLabel(databaseName));
            _sb.AppendLine("GO");
            _sb.AppendLine();
            return this;
        }                              

        public string FormatLabel(params string[] values)
        {
            return ToLabel(values);
        }

        public static string ToLabel(params string[] values)
        {

            var sb = new StringBuilder();
            bool dot = false;
            foreach (var item in values)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (dot)
                        sb.Append('.');
                    sb.Append($"[{item}]");
                    dot = true;
                }
            }

            return sb.ToString();
        }

        public void CleanIndent()
        {
            while (_index > 0)
                DelIndent();
        }

        public void DelIndent()
        {
            _index--;
            if (_index < 0)
                _index = 0;
            else
            {
                var last = _sb[_sb.Length - 1];
                if (last == '\t')
                    _sb.Remove(_sb.Length - 1, 1);
            }
        }

        public void AddIndent()
        {

            if (_index < 0)
                _index = 0;

            _index++;

            _sb.Append('\t');

        }

        internal void AppendEndLine(params object[] values)
        {
            foreach (var value in values)
                _sb.Append(value);
            AppendEndLine();
        }

        internal void AppendEndLine()
        {

            _sb.AppendLine();
            for (int i = 0; i < _index; i++)
                _sb.Append('\t');

        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        public IDisposable Indent(bool crlf = false)
        {
            var result = new _disposable(this);
            if (crlf)
                result.After.Add(c => c.AppendEndLine());
            return result;
        }

        public IDisposable IndentWithParentheses(bool crlf = false)
        {
            var result = new _disposable(this, "(", ")");
            if (crlf)
                result.After.Add(c => c.AppendEndLine());
            return result;
        }

        private class _disposable : IDisposable
        {

            public _disposable(Writer writer, string start = null, string end = null)
            {

                this.After = new List<Action<Writer>>();

                this._writer = writer;

                if (!string.IsNullOrEmpty(start))
                    _writer.Append(start);

                this._writer.AddIndent();
                this._end = end;
            }

            protected virtual void Dispose(bool disposing)
            {

                if (!disposedValue)
                {
                    if (disposing)
                    {

                        this._writer.DelIndent();

                        if (!string.IsNullOrEmpty(_end))
                            _writer.Append(_end);

                        foreach (var item in After)
                            item(this._writer);

                    }
                    disposedValue = true;
                }
            }

            public List<Action<Writer>> After { get; set; }

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            private bool disposedValue;
            private Writer _writer;
            private string _end;
        }


        private readonly StringBuilder _sb;
        private int _index;

    }


}