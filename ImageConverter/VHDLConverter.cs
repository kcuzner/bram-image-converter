using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace ImageConverter
{
	/// <summary>
	/// Converter into VHDL. Supports only images with < 2K pixels
	/// </summary>
	/// <description>
	/// Converts images into a 2Kx8 std_logic_vector in its own
	/// module which should be synthesized into a BRAM in ROM mode
	/// </description>
    public class VHDLConverter
    {
		/// <summary>
		/// BRAM size is 2Kx8 bits
		/// </summary>
		const int MaxSize = 2048;

		/// <summary>
		/// VHDL Template for a rom
		/// </summary>
		const string TemplateTop = "library ieee;\n" +
		                           "use ieee.std_logic_1164.all;\n" +
		                           "use ieee.numeric_std.all;\n" +
		                           "entity image_rom is\n" +
		                           "\tport(\n" +
		                           "\t\tclk: in std_logic;\n" +
		                           "\t\taddr: in std_logic_vector(10 downto 0);\n" +
		                           "\t\tdata: out std_logic_vector(7 downto 0)\n" +
		                           ");\n" +
		                           "end image_rom;\n" +
		                           "\n" +
		                           "architecture arch of image_rom is\n" +
		                           "\tconstant ADDR_WIDTH: integer := 11;\n" +
		                           "\tconstant DATA_WIDTH: integer := 8;\n" +
		                           "\tsignal addr_reg: std_logic_vector(ADDR_WIDTH - 1 downto 0);\n" +
		                           "\ttype rom_type is array(0 to 2**ADDR_WIDTH - 1)\n" +
		                           "\t\tof std_logic_vector(DATA_WIDTH - 1 downto 0);\n" +
		                         "\tconstant ROM: rom_type := (";
		const string TemplateBottom = ");\n" +
		                              "begin\n" +
		                              "\tprocess(clk)\n" +
		                              "\tbegin\n" +
		                              "\t\tif (clk'event and clk = '1') then\n" +
		                              "\t\t\taddr_reg <= addr;\n" +
		                              "\t\tend if;\n" +
		                              "\tend process;\n" +
		                              "\tdata <= ROM(to_integer(unsigned(addr_reg)));\n" +
		                              "end arch;\n";

		protected Bitmap Bitmap { get; private set; }

		public VHDLConverter(Bitmap b)
        {
			//check that the image is a good size
			if (b.Width * b.Height > MaxSize)
				throw new ArgumentException("Bitmap has >2K pixels and cannot be synthesized into a 16kbit BRAM");

			this.Bitmap = b;
        }

		public void Save(string filename)
		{
			using (StreamWriter wr = new StreamWriter(filename))
			{
				//write the top of the template
				wr.Write(string.Format(TemplateTop));

				//calculate the wasted bytes
				int wasted = MaxSize - (this.Bitmap.Width * this.Bitmap.Height);

				//write the image data in row-major order
				for(int y = 0; y < this.Bitmap.Height; y++)
				{
					for(int x = 0; x < this.Bitmap.Width; x++)
					{
						Color c = this.Bitmap.GetPixel(x, y);
						wr.Write(string.Format("\"{0}{1}{2}\"",
							c.R.ToThreeBitString(), c.G.ToThreeBitString(), c.B.ToTwoBitString()));

						if (wasted != 0 || y != this.Bitmap.Height - 1 || x != this.Bitmap.Width - 1)
						{
							//we still have more things, so we need a comma
							wr.Write(",\n");
						}
					}
				}

				//write any wasted bytes to 0
				for(int i = 0; i < wasted; i++)
				{
					wr.Write("\"00000000\"");
					if (i != wasted - 1)
						wr.Write(",\n");
				}

				//write the bottom of the template
				wr.Write(string.Format(TemplateBottom));
			}
		}
    }
}

