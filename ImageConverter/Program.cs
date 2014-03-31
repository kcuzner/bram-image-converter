using System;
using System.Drawing;
using System.Linq;

namespace ImageConverter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			if (args.Length < 1)
			{
				Console.WriteLine("ERROR: No output filename specified");
				return;
			}

			if (args.Length < 2)
			{
				Console.WriteLine("ERROR: At least one input file must be specified");
				return;
			}

			using(ImageCollection collection = new ImageCollection())
			{
				string outputFilename = args[0];

				foreach(var fn in args.Skip(1))
				{
					collection.Add(new Bitmap(fn));
				}

				using(Bitmap b = collection.Render())
				{
					VHDLConverter c = new VHDLConverter(b);

					c.Save(outputFilename);
				}

				Console.WriteLine("Output written to {0}", outputFilename);
			}
        }
    }
}
