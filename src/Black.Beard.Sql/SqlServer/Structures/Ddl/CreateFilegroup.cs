using Bb.SqlServer.Structures;
using Bb.SqlServer.Structures.Dacpacs;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Ddl
{


    public class CreateFilegroup : DdlBase
    {

        public CreateFilegroup(Writer writer, ScriptContext ctx, string path) : base(writer, ctx)
        {
            this._path = path;
        }


        public void Parse(DatabaseStructure structure)
        {

            CommentLine("Create filegroups");

            Parse(structure.DatabaseName, structure.FileGroups.Where(c => c.Name != "PRIMARY" && c.Name != "SECONDARY").ToList());

        }


        public void Parse(string databaseName, List<FileGroupDescriptor> fileGroups)
        {
            if (fileGroups.Any())
            {
                AppendEndLine("ALTER DATABASE ", AsLabel(this._ctx.ReplaceVariables(databaseName)));
                using (Indent())
                {

                    AppendEndLine("ADD FILE");

                    foreach (var group in fileGroups)
                        ParseFile(group);

                    Go();

                }
            }
        }

        private void ParseFile(FileGroupDescriptor group)
        {

            string path = GetPath();
            var name = this._ctx.ReplaceVariables(group.Name);

            using (IndentWithParentheses(true))
            {
                AppendEndLine($"NAME = N'{group.Name}',");
                AppendEndLine($"FILENAME = N'{Path.Combine(path, name)}.mdf',");
                AppendEndLine($"SIZE = 8192KB,");
                AppendEndLine($"FILEGROWTH = 65536KB");
            }

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


        private readonly string _path;

    }


}