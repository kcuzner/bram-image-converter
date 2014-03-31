using System;
using System.Drawing;

namespace ImageConverter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			using(ImageCollection collection = new ImageCollection())
			{
				foreach(var fn in args)
				{
					collection.Add(new Bitmap(fn));
				}

				using(Image i = collection.Render())
				{

				}
			}
        }
    }
}
