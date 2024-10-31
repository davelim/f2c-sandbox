// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
// see: https://github.com/dlemstra/Magick.NET/blob/main/docs/Watermark.md
using ImageMagick;

using DotNetDBF;


namespace Test.Magick;

public class Program
{
    public static void Main(string[] args)
    {
        // ImageMagick
        using var image = new ImageMagick.MagickImage(
            System.IO.Path.GetFullPath("./images/monkey.jpg"));
        using var watermark = new MagickImage(
            System.IO.Path.GetFullPath("./images/StatementTestOverlay.png"));
        image.Composite(watermark, Gravity.Center, CompositeOperator.Over);
        image.Write(
            System.IO.Path.GetFullPath("./images/test_monkey.jpg"));


        // DBF writer
        string test_dbf = "./test/dbase_8b.dbf";
        using var reader = new DBFReader(test_dbf);
        System.Console.WriteLine(reader.ToString());

        //using var writer = new DBFWriter(test_dbf) {
        //    DataMemoLoc = "./test/dbase_8b.dbt"
        //};
    }
}
