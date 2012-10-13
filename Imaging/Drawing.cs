using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Main class which works with palettes and draws images.
	/// </summary>
	public static class Drawing
	{
		/// <summary>
		/// Draws bitmap.
		/// </summary>
		/// <param name="data">
		/// Image pixel data.
		/// </param>
		/// <param name="width">
		/// Output image width.
		/// </param>
		/// <param name="height">
		/// Output image height.
		/// </param>
		/// <param name="palette">
		/// Used palette index.
		/// </param>
		/// <returns>
		/// Drawn bitmap.
		/// </returns>
		public static Bitmap DrawBitmap(byte[] data, int width, int height, byte palette)
		{
			return DrawBitmap(data, width, height, ImagePalette.GetFullPalette(palette), null);
		}
		
		/// <summary>
		/// Draws bitmap.
		/// </summary>
		/// <param name="data">
		/// Image pixel data.
		/// </param>
		/// <param name="width">
		/// Output image width.
		/// </param>
		/// <param name="height">
		/// Output image height.
		/// </param>
		/// <param name="palette">
		/// Color palette.
		/// </param>
		/// <returns>
		/// Drawn bitmap.
		/// </returns>
		public static Bitmap DrawBitmap(byte[] data, int width, int height, ImagePalette palette, RenderOptions options)
		{
			options = options??RenderOptions.Empty;
			Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
			ColorPalette pal = bmp.Palette;
			palette.CopyTo(pal.Entries, 0);
			if(options.TransparentIndex >= 0)pal.Entries[options.TransparentIndex] = Color.Transparent;
			bmp.Palette = pal;
			BitmapData bmpdata = bmp.LockBits(new Rectangle(0,0,width,height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
			if(data.Length == 0){}
			else if(width%4 == 0)
			{
				Marshal.Copy(data, 0, bmpdata.Scan0, Math.Min(bmpdata.Stride*bmpdata.Height, data.Length));
			}else{
				for(int y = 0; y < height; y++)
				{
					if(y == height-1)
					{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, data.Length-width*y);
					}else{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
					}
				}
			}
			bmp.UnlockBits(bmpdata);
			return bmp;
		}
		
		/// <summary>
		/// Computes height and draws bitmap.
		/// </summary>
		/// <param name="data">
		/// Image pixel data.
		/// </param>
		/// <param name="width">
		/// Output image width.
		/// </param>
		/// <param name="palette">
		/// Used palette index.
		/// </param>
		/// <returns>
		/// Drawn bitmap.
		/// </returns>
		public static Bitmap DrawBitmap(byte[] data, int width, byte palette)
		{
			int height = (data.Length+width-1)/width;
			return DrawBitmap(data, width, height, palette);
		}
		
		/// <summary>
		/// Converts bitmap to byte array using external palette.
		/// </summary>
		public static byte[] LoadBitmap(Bitmap bmp, ImagePalette palette)
		{
			byte[] result = new byte[bmp.Width*bmp.Height];
			BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			for(int y = 0; y < bmp.Height; y++)
			for(int x = 0; x < bmp.Width; x++)
			{
				byte r = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3);
				byte g = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3+1);
				byte b = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3+2);
				result[y*bmp.Width+x] = (byte)palette.GetNearestColorIndex(Color.FromArgb(r,g,b));
			}
			return result;
		}
	}
}