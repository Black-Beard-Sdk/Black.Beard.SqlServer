using Bb.SqlServer.Queries;
using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using SharpCompress.Writers;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.SqlServer.Structures.Ddl
{

    public class CreateDatabase : DdlBase
    {


        public CreateDatabase(Writer writer, ScriptContext ctx, string path)
            : base(writer, ctx)
        {
            _path = path;
        }


        public void Parse(DatabaseStructure structure)
        {


            string path = GetPath();

            var name = this._ctx.ReplaceVariables(structure.DatabaseName);

            CommentLine("Create database ", name);

            AppendEndLine();
            Use("master");
            Go();

            AppendEndLine("DECLARE @noExists BIT;");
            AppendEndLine("SET @noExists = 0;");
            AppendEndLine(TextQueries.TestDatabaseNoExists(structure.DatabaseName));
            using (Indent())
                AppendEndLine("SET @noExists = 1;");
            AppendEndLine();

            AppendEndLine("IF (@noExists = 1)");
            AppendEndLine("BEGIN");
            using (Indent())
            {

                AppendEndLine("CREATE DATABASE ", AsLabel(name));
                using (Indent())
                {

                    Append("CONTAINMENT = ");
                    if (structure.ContainmentType == ContainmentEnum.NONE)
                        AppendEndLine("NONE");
                    else
                        AppendEndLine("PARTIAL");

                    AppendEndLine("ON PRIMARY");
                    using (IndentWithParentheses(true))
                    {
                        AppendEndLine($"NAME = N'{name}',");
                        AppendEndLine($"FILENAME = N'{Path.Combine(path, name)}.mdf',");
                        AppendEndLine($"SIZE = 8192KB,");
                        AppendEndLine($"FILEGROWTH = 65536KB");
                    }

                    AppendEndLine("LOG ON");
                    using (IndentWithParentheses(true))
                    {
                        AppendEndLine($"NAME = N'{name}_log',");
                        AppendEndLine($"FILENAME = N'{Path.Combine(path, name)}_log.ldf',");
                        AppendEndLine($"SIZE = 8192KB,");
                        AppendEndLine($"FILEGROWTH = 65536KB");
                    }

                    AppendEndLine();

                }


                WriteSetProperty(name, c => c.CompatibilityLevel, structure);
                WriteSetProperty(name, c => c.AnsiNullDefault, structure);
                WriteSetProperty(name, c => c.AnsiNulls, structure);
                WriteSetProperty(name, c => c.AnsiPadding, structure);
                WriteSetProperty(name, c => c.AnsiWarnings, structure);
                WriteSetProperty(name, c => c.Arithabort, structure);
                WriteSetProperty(name, c => c.AutoClose, structure);
                WriteSetProperty(name, c => c.AutoShrink, structure);
                WriteSetProperty(name, c => c.AutoUpdateStatistics, structure);
                WriteSetProperty(name, c => c.CursorCloseOnCommit, structure);
                WriteSetProperty(name, c => c.CursorDefault, structure);
                WriteSetProperty(name, c => c.ConcatNullYieldsNull, structure);
                WriteSetProperty(name, c => c.NumericRoundAbort, structure);
                WriteSetProperty(name, c => c.QuotedIdentifier, structure);
                WriteSetProperty(name, c => c.RecursiveTriggers, structure);
                WriteSetProperty(name, c => c.AutoUpdateStatisticsAsync, structure);
                WriteSetProperty(name, c => c.DateCorrelationOptimization, structure);
                WriteSetProperty(name, c => c.Parametrization, structure);
                WriteSetProperty(name, c => c.ReadCommitedSnapShot, structure);
                WriteSetProperty(name, c => c.DatabaseReadOnly, structure);
                WriteSetProperty(name, c => c.Recovery, structure);
                WriteSetProperty(name, c => c.RestrictAccess, structure);
                WriteSetProperty(name, c => c.PageVerify, structure);
                WriteSetProperty(name, c => c.TargetRecoveryTimeInSecond, structure);
                WriteSetProperty(name, c => c.Broker, structure);
                // WriteSetProperty(name, c => c.AutoCreateStatistics, structure);

            }

            AppendEndLine("END");

            Go();

            Use(name);

            WriteSetProperty(c => c.CardinalyEstimation, structure);
            WriteSetProperty(c => c.CardinalyEstimationForSecondary, structure);

            WriteSetProperty(c => c.MaxDop, structure);
            //WriteSetProperty(c => c.MaxDopForSecondary, structure);

            WriteSetProperty(c => c.ParameterSniffing, structure);
            WriteSetProperty(c => c.ParameterSniffingForSecondary, structure);

            WriteSetProperty(c => c.QueryOptimizerHotfixes, structure);
            WriteSetProperty(c => c.QueryOptimizerHotfixesForSecondary, structure);

            AppendEndLine();

            Use(name);

        }




        private string GetPath()
        {

            if (!string.IsNullOrEmpty(_path))
                return _path;

            var p = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Microsoft SQL Server"));
            if (p.Exists)
            {

                var next = p.GetDirectories("MSSQL*.MSSQLSERVER", SearchOption.TopDirectoryOnly).OrderBy(c => c.Name).LastOrDefault();
                if (next != null)
                {
                    p = new DirectoryInfo(Path.Combine(next.FullName, "MSSQL", "DATA"));
                    if (p.Exists)
                        return p.FullName;
                }

            }

            p = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Microsoft SQL Server"));
            if (p.Exists)
            {

                var next = p.GetDirectories("MSSQL*.MSSQLSERVER", SearchOption.TopDirectoryOnly).OrderBy(c => c.Name).LastOrDefault();
                if (next != null)
                {
                    p = new DirectoryInfo(Path.Combine(next.FullName, "MSSQL", "DATA"));
                    if (p.Exists)
                        return p.FullName;
                }

            }

            return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        }


        private void WriteSetProperty<T>(string databseName, Expression<Func<DatabaseStructure, T>> e, object instance)
        {

            Append("ALTER DATABASE ", AsLabel(databseName), " ");
            var property = e.GetProperty();
            var attribute = property.GetCustomAttribute(typeof(PropertySerializedAttribute), true) as PropertySerializedAttribute;

            if (attribute != null)
                AppendEndLine(attribute.Serialize(instance, property));

            else
            {


            }
            // Go();

        }


        private void WriteSetProperty<T>(Expression<Func<DatabaseStructure, T>> e, object instance)
        {

            Append("ALTER DATABASE ");
            var property = e.GetProperty();
            var attribute = property.GetCustomAttribute(typeof(PropertySerializedAttribute), true) as PropertySerializedAttribute;

            if (attribute != null)
                AppendEndLine(attribute.Serialize(instance, property));

            else
            {


            }

            Go();

        }


        private readonly string _path;


    }
}