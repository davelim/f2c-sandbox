// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
// see: https://github.com/dlemstra/Magick.NET/blob/main/docs/Watermark.md
using ImageMagick;


namespace Test.Magick;

public class Program
{
    public static void Main(string[] args)
    {
        using var image = new ImageMagick.MagickImage(
            System.IO.Path.GetFullPath("./images/monkey.jpg"));
        using var watermark = new MagickImage(
            System.IO.Path.GetFullPath("./images/StatementTestOverlay.png"));
        image.Composite(watermark, Gravity.Center, CompositeOperator.Over);
        image.Write(
            System.IO.Path.GetFullPath("./images/test_monkey.jpg"));
    }
}
