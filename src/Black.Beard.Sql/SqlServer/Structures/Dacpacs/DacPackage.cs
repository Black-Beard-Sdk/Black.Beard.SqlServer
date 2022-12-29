using System.IO.Compression;
using System.Reflection;
using Bb.Dacpacs;

namespace Bb.SqlServer.Structures.Dacpacs
{

    public class DacPackage
    {

        public DacPackage()
        {
            Schema = new DacDataSchemaModel();
            _start = DateTime.Now;
        }

        public string Name { get; set; }

        public DacDataSchemaModel Schema { get; }

        public void Write(string filename, bool DeleteIfExists = false)
        {

            var file = new FileInfo(filename);

            if (DeleteIfExists)
            {
                if (file.Exists)
                    file.Delete();
            }
            else
                throw new IOException($" File {filename} allready exists. please delete before.");

            if (string.IsNullOrEmpty(Name))
                Name = Path.GetFileNameWithoutExtension(file.Name);

            try
            {

                GenerateTempDirectory(filename);
                Write();

                ZipFile.CreateFromDirectory(_dir.FullName, filename);

            }
            finally
            {
                Clean();
            }

        }

        //public void GenerateDotnetPublish(string filename, string server, string dbName, string userId, string password, string createNewDatabase)
        //{

        //    var file = new FileInfo(filename);

        //    string dacpacPath = file.Directory.FullName;
        //    string dacpacPattern = file.Directory.Name;

        //    Bb.DotnetCommand.DacpacPublishWithCreateDatabase(dacpacPath, dacpacPattern, server, dbName, userId, password, createNewDatabase);

        //}

        private void Write()
        {

            File.WriteAllText(_file_model, Schema.GetToString());
            File.WriteAllText(_file_Content_Types, ContentTypes.GetToString());
            File.WriteAllText(_file_DacMetadata, DacTypes.GetToString(Name, "1.0.0.0"));

            string checksum = ChecksumHelper.CalculateChecksum(_file_model).ByteArrayToString();

            File.WriteAllText(_file_origin, DacOrigin.GetToString(
                    new Guid("adc6ba86-fcc3-45dd-aa9c-c8f196efa9cc")
                    , _start
                    , DateTime.Now
                    , checksum
                ));

        }

        private void Clean()
        {

            if (File.Exists(_file_Content_Types))
                File.Delete(_file_Content_Types);

            if (File.Exists(_file_DacMetadata))
                File.Delete(_file_DacMetadata);

            if (File.Exists(_file_model))
                File.Delete(_file_model);

            if (File.Exists(_file_origin))
                File.Delete(_file_origin);

            _dir.Delete();

        }

        private void GenerateTempDirectory(string outputfile)
        {

            var f = new FileInfo(outputfile);

            if (f.Directory.Exists)
                f.Directory.Create();

            else if (f.Exists)
                f.Delete();

            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", ""));

            _dir = new DirectoryInfo(path);

            if (_dir.Exists)
                _dir.Delete();

            _dir.Create();

            _file_Content_Types = Path.Combine(_dir.FullName, "[Content_Types].xml");
            _file_DacMetadata = Path.Combine(_dir.FullName, "DacMetadata.xml");
            _file_model = Path.Combine(_dir.FullName, "model.xml");
            _file_origin = Path.Combine(_dir.FullName, "Origin.xml");

        }

        private readonly DateTime _start;
        private string _file_Content_Types;
        private string _file_DacMetadata;
        private string _file_model;
        private string _file_origin;
        private DirectoryInfo _dir;

    }

}
