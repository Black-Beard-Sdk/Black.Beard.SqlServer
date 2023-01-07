using Bb.SqlServer.Structures.Dacpacs;

namespace Bb.SqlServer.Queries
{
    public class QueryToString : QueryBaseVisitor
    {

        public QueryToString(Writer writer, ScriptContext ctx)
        {
            this._writer = writer;
            this._ctx = ctx;
        }

        public override void VisitMerge(Merge q)
        {

            AppendCloseLine();
            AppendCloseLine("MERGE");
            using (Indent())
            {
                if (q.TopClause != null) // [ TOP ( expression ) [ PERCENT ] ]
                {
                    Append("TOP (", q.TopClause.Limit, ")");
                    if (q.TopClause.Mode == TopModeEnum.Percent)
                        Append(" PERCENT");
                    AppendCloseLine();
                    AppendCloseLine();
                }

                if (q.IntoClause != null) // [ INTO ] <target_table> [ WITH ( <merge_hint> ) ] [ [ AS ] table_alias ]  
                {

                    Append("INTO ", FormatLabel(q.IntoClause.TargetName));

                    if (q.IntoClause.Hints.Any)
                        Append(" WITH (", q.IntoClause.Hints.ToString(), ")");

                    if (!string.IsNullOrEmpty(q.IntoClause.Alias))
                        Append(" AS ", q.IntoClause.Alias);

                    AppendCloseLine();
                    AppendCloseLine();
                }

                if (q.UsingClause != null)   // USING <table_source>[ [AS] table_alias ]
                {

                    Append("USING ", FormatLabel(q.UsingClause.SourceName));

                    if (!string.IsNullOrEmpty(q.UsingClause.Alias))
                        Append(" AS ", q.UsingClause.Alias);

                    AppendCloseLine();
                    AppendCloseLine();

                }

                if (q.OnClause != null)  // ON <merge_search_condition>
                {
                    Append("ON ");
                    q.OnClause.Accept(this);
                    AppendCloseLine();
                    AppendCloseLine();
                }


                if (q.ClauseMatched != null)                    // [ WHEN MATCHED 
                {
                    Append("WHEN MATCHED");
                    q.ClauseMatched.Accept(this);               // AND <clause_search_condition>
                    AppendCloseLine();
                    AppendCloseLine();
                }

                if (q.ClauseNotMatchedByTarget != null)         // [ WHEN NOT MATCHED [ BY TARGET ] 
                {
                    Append("WHEN NOT MATCHED BY TARGET");
                    q.ClauseNotMatchedByTarget.Accept(this);    // AND <clause_search_condition>
                    AppendCloseLine();
                }

                if (q.ClauseNotMatchedBySource != null)         // [ WHEN NOT MATCHED BY SOURCE ]
                {
                    Append("WHEN NOT MATCHED BY SOURCE");
                    q.ClauseNotMatchedBySource.Accept(this);    // AND <clause_search_condition>
                    AppendCloseLine();
                }

                if (q.ClauseNotMatchedByTarget != null)         // [ WHEN NOT MATCHED [ BY TARGET ] 
                {

                    AppendCloseLine("OUTPUT $action");

                    var c1 = new CollectTargetKeys();
                    c1.VisitBinary(q.OnClause as BinarySqlExpression);

                    foreach (var item in c1.Items)
                    {
                        string colName = item.Label;
                        if (item.Child != null)
                            colName = item.Child.Label;
                        AppendCloseLine($"  , COALESCE([DELETED].[{colName}], [INSERTED].[{colName}]) as {colName}");
                    }

                    foreach (var item in q.ClauseNotMatchedByTarget.Sets)
                    {
                        var c = item.Column;
                        string colName = c.Label;
                        if (c.Child != null)
                            colName = c.Child.Label;
                        AppendCloseLine($"  , COALESCE([DELETED].[{colName}], [INSERTED].[{colName}]) as {colName}");
                    }

                    AppendCloseLine();
                }

            }

            AppendCloseLine(";");

        }

        public override void VisitMergeClause(MergeClause q)
        {

            if (q.ClauseSearchCondition != null)
            {
                Append(" AND");
                q.ClauseSearchCondition.Accept(this);
            }

            //      THEN <merge_matched> ] [ ...n ]  
            AppendCloseLine(" THEN ");
            using (Indent())
            {
                switch (q.KindAction)
                {

                    case MergeClauseActionEnumEnum.Insert:
                        AppendCloseLine("INSERT");
                        AppendCloseLine("(");
                        using (Indent())
                        {
                            if (q.Sets != null)
                            {
                                string comma = string.Empty;
                                foreach (var item in q.Sets)
                                {
                                    Append(comma);
                                    Append(FormatLabel(item.Column));
                                    comma = ", ";
                                }

                            }
                        }
                        AppendCloseLine();
                        AppendCloseLine(")");

                        AppendCloseLine("VALUES");
                        AppendCloseLine("(");
                        using (Indent())
                        {
                            if (q.Sets != null)
                            {
                                string comma = string.Empty;
                                foreach (var item in q.Sets)
                                {
                                    Append(comma);
                                    item.Value.Accept(this);
                                    comma = ", ";
                                }
                            }
                        }
                        AppendCloseLine();
                        AppendCloseLine(")");

                        break;

                    case MergeClauseActionEnumEnum.Update:
                        Append("UPDATE SET");
                        using (Indent())
                        {
                            if (q.Sets != null)
                            {
                                string comma = string.Empty;
                                foreach (var item in q.Sets)
                                {

                                    AppendCloseLine(comma);
                                    Append(FormatLabel(item.Column), " = ");
                                    item.Value.Accept(this);
                                    comma = ", ";
                                }
                            }
                        }
                        break;

                    case MergeClauseActionEnumEnum.Delete:
                        AppendCloseLine("DELETE");
                        break;

                    default:
                        break;

                }
            }

        }

        /*     
       [ <output_clause> ]  
       [ OPTION ( <query_hint> [ ,...n ] ) ]
   ;  

    */

        public void Append(params object[] args)
        {
            _writer.Append(args);
        }

        public void AppendCloseLine(params object[] args)
        {
            _writer.AppendEndLine(args);
        }

        private string FormatLabel(SqlLabelReference name)
        {
            return name.Write(this._ctx);
        }

        public IDisposable Indent(bool crlf = false)
        {
            return _writer.Indent(crlf);
        }

        public string AsLabel(params string[] values)
        {
            return _writer.FormatLabel(values);
        }

        public override void VisitBinary(BinarySqlExpression q)
        {

            q.Left.Accept(this);

            switch (q.Kind)
            {
                case BinaryExprEnum.Equal:
                    Append(" = ");
                    break;
                case BinaryExprEnum.NotEqual:
                    Append(" != ");
                    break;
                case BinaryExprEnum.GreatThan:
                    Append(" >");
                    break;
                case BinaryExprEnum.NotGreatThan:
                    Append(" !>");
                    break;
                case BinaryExprEnum.GreatOrEqualThan:
                    Append(" >=");
                    break;
                case BinaryExprEnum.LessThan:
                    Append(" <");
                    break;
                case BinaryExprEnum.NotLessThan:
                    Append(" !<");
                    break;
                case BinaryExprEnum.LessOrEqualThan:
                    Append(" <=");
                    break;
                case BinaryExprEnum.And:
                    Append(" AND ");
                    break;
                case BinaryExprEnum.Or:
                    Append(" OR ");
                    break;
                case BinaryExprEnum.Like:
                    Append(" LIKE ");
                    break;
                case BinaryExprEnum.In:
                    Append(" IN ");
                    break;
                case BinaryExprEnum.Add:
                    Append(" + ");
                    break;
                case BinaryExprEnum.Substract:
                    Append(" - ");
                    break;
                case BinaryExprEnum.Multiply:
                    Append(" * ");
                    break;
                case BinaryExprEnum.Divid:
                    Append(" / ");
                    break;
                case BinaryExprEnum.Modulo:
                    Append(" % ");
                    break;
                case BinaryExprEnum.Between:
                    Append(" BETWEEN ");
                    break;
                default:
                    break;
            }

            q.Right.Accept(this);

        }

        public override void VisitConstant(Constant q)
        {
            Append(q.Key);
        }

        public override void VisitFunction(FunctionSqlExpression q)
        {

            Append(q.Name, "(");
            string comma = string.Empty;
            foreach (var item in q.Arguments)
            {
                _writer.Append(comma);
                item.Accept(this);
                comma = ", ";
            }
            Append(q.Name, ")");

        }

        public override void VisitLabelReference(SqlLabelReference q)
        {
            Append(FormatLabel(q));
        }

        public override void VisitList(SqlExpressionList q)
        {
            string comma = string.Empty;
            foreach (var item in q)
            {
                _writer.Append(comma);
                item.Accept(this);
                comma = ", ";
            }
        }

        public override void VisitUnary(UnarySqlExpression q)
        {

            if (q.Kind == UnaryExprEnum.Not)
                Append("! ");

            q.Child.Accept(this);

            switch (q.Kind)
            {

                case UnaryExprEnum.IsNull:
                    Append(" IS NULL");
                    break;
                case UnaryExprEnum.IsNotNull:
                    Append(" IS NOT NULL");
                    break;
                case UnaryExprEnum.IsDistinctFrom:
                    Append(" IS DISTINCT FROM");
                    break;
                case UnaryExprEnum.IsNotDistinctFrom:
                    Append(" IS NOT DISTINCT FROM");
                    break;

                case UnaryExprEnum.Not:
                default:
                    break;
            }

        }

        public override void VisitKeyword(Keyword q)
        {
            Append(" ", q.Key);
        }


        private class CollectTargetKeys : QueryBaseVisitor
        {

            public CollectTargetKeys()
            {
                this.Items = new List<SqlLabelReference>();

            }

            public List<SqlLabelReference> Items { get; }

            public override void VisitBinary(BinarySqlExpression q)
            {

                q.Left.Accept(this);
            }

            public override void VisitConstant(Constant q)
            {
                throw new NotImplementedException();
            }

            public override void VisitFunction(FunctionSqlExpression q)
            {
                throw new NotImplementedException();
            }

            public override void VisitKeyword(Keyword q)
            {
                throw new NotImplementedException();
            }

            public override void VisitLabelReference(SqlLabelReference q)
            {
                 this.Items.Add(q);
            }

            public override void VisitList(SqlExpressionList q)
            {
                throw new NotImplementedException();
            }

            public override void VisitMerge(Merge q)
            {
                throw new NotImplementedException();
            }

            public override void VisitMergeClause(MergeClause q)
            {
                throw new NotImplementedException();
            }

            public override void VisitUnary(UnarySqlExpression q)
            {
                throw new NotImplementedException();
            }
        }


        private readonly Writer _writer;
        private readonly ScriptContext _ctx;

    }


}
