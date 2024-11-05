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
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
        { // ImageMagick
            using var image = new ImageMagick.MagickImage(
                System.IO.Path.GetFullPath("./test/monkey.jpg"));
            using var watermark = new MagickImage(
                System.IO.Path.GetFullPath("./test/StatementTestOverlay.png"));
            image.Composite(watermark, Gravity.Center, CompositeOperator.Over);
            image.Write(
                System.IO.Path.GetFullPath("./test/test_monkey.jpg"));
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

        {// .NETFramework WebClient to ASP.NET Core HttpClient
            async System.Threading.Tasks.Task clientTest() {
                string url = "https://fastly.picsum.photos/id/0/5000/3333.jpg?hmac=_j6ghY5fCfSD6tvtcV74zXivkJSPIfR9B8w34XeQmvU";

#if (NET6_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER)
                System.Net.Http.HttpClient client = new ();
                try {
                    using System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);

                    // Get data and type.
                    byte[] data = await response.Content.ReadAsByteArrayAsync();
                    System.Net.Http.Headers.MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;

                    Console.WriteLine($"c. content type: {contentType}"); // implicit .ToString()
                    System.IO.File.WriteAllBytes("./test/c.image.jpg", data);
                } catch (System.Net.Http.HttpRequestException e) {
                    Console.WriteLine($"Message: {e.Message}");
                }
#else
                System.Net.WebClient client = new ();

                // Get data and type.
                byte[] data = client.DownloadData(url);
                System.Net.WebHeaderCollection responseHeaders = client.ResponseHeaders;
                string contentType = responseHeaders["Content-Type"];

                Console.WriteLine($"f. content type: {contentType}");
                System.IO.File.WriteAllBytes("./test/f.image.jpg", data);
#endif
            }

            await clientTest();
        }
    }
}
