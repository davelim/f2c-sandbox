// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
// see: https://github.com/dlemstra/Magick.NET/blob/main/docs/Watermark.md
using System;
//using System.Web;

using ImageMagick;

using DotNetDBF;


namespace Test.Magick;

public class Program
{
    public static void Main(string[] args)
    {
        { // ImageMagick
            using var image = new ImageMagick.MagickImage(
                System.IO.Path.GetFullPath("./images/monkey.jpg"));
            using var watermark = new MagickImage(
                System.IO.Path.GetFullPath("./images/StatementTestOverlay.png"));
            image.Composite(watermark, Gravity.Center, CompositeOperator.Over);
            image.Write(
                System.IO.Path.GetFullPath("./images/test_monkey.jpg"));
        }


        {// DBF writer
            // for more test files, see:
            // - /mnt/c/Users/dave.lim/source/repos/OSS/dbf/spec/fixtures
            string test_dbf = "./test/dbase_8b.dbf";
            Console.WriteLine($"db path: {test_dbf}...");
            using var reader = new DBFReader(test_dbf);
            Console.WriteLine(reader.ToString());

            //using var writer = new DBFWriter(test_dbf) {
            //    DataMemoLoc = "./test/dbase_8b.dbt"
            //};
        }


        {// HttpRuntime (.NET Framework) vs IHostEnvironment (ASP.NET Core)
            // .NETFramework
            //Console.WriteLine(System.Web.HttpRunTime.AppDomainAppPath);
            //Console.WriteLine(System.Web.HttpRunTime.AppDomainPathVirtualPath);
            // ASP.NET Core:
            // - IWebHostEnvironment dependency injection
            //   - IWebHostEnvironment.ContentRootPath (inherited from IHostEnvironment)
            // - see: https://stackoverflow.com/questions/43916487/httpruntime-appdomainapppath-equivalent-in-asp-net-core
        }


        {
            void printByteArray(byte[] array) {
                for (int i = 0; i < array.Length; i++) {
                    Console.Write($"{array[i]:X2}");
                    if ((i % 4) == 3) Console.Write(" ");
                }
                Console.WriteLine();
            }
            var unicode = new System.Text.UnicodeEncoding();
            var strm = new System.IO.MemoryStream(unicode.GetBytes(
                "Test stream for MD5 Create() and ComputeHash()."), false);
            var fMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var cMD5 = System.Security.Cryptography.MD5.Create();
            Console.WriteLine("framework...");
            printByteArray(fMD5.ComputeHash(strm));
            Console.WriteLine("core...");
            strm.Seek(0, System.IO.SeekOrigin.Begin);
            printByteArray(cMD5.ComputeHash(strm));
        }
    }
}
