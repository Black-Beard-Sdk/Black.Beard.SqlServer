using System.Security.Cryptography;
using System.Xml.Linq;

namespace Bb.SqlServer.Structures.Dacpacs
{
    internal static class ChecksumHelper
    {


        public static HashAlgorithm GetHashAlgorithm()
        {
            var sha256 = new SHA256CryptoServiceProvider();
            return sha256;
            //return  HashAlgorithm.Create("System.Security.Cryptography.SHA256CryptoServiceProvider");
        }

        internal static byte[] CalculateChecksum(Stream stream)
        {

            if (stream == null)
                throw new ArgumentNullException("stream");

            using HashAlgorithm hashGenerator = GetHashAlgorithm();
            return hashGenerator.ComputeHash(stream);

        }

        internal static byte[] CalculateChecksum(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new ArgumentException(nameof(filepath));

            using FileStream stream = new FileStream(filepath, FileMode.Open);
            return CalculateChecksum(stream);

        }

        public static string ByteArrayToString(this byte[] bytes)
        {
            return string.Concat(bytes.Select((b)
                => b.ToString("X2", System.Globalization.CultureInfo.InvariantCulture)));

        }


    }

}
