using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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
			return DrawBitmap(data, width, height, ImagePalette.GetFullPalette(palette));
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
		/// <param name="options">
		/// More rendering options.
		/// </param>
		/// <returns>
		/// Drawn bitmap.
		/// </returns>
		public static Bitmap DrawBitmap(byte[] data, int width, int height, RenderOptions options)
		{
			ImagePalette palette = options.Palette;
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
					if(width*y < data.Length)
					{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, Math.Min(width, data.Length-width*y));
					}else{
						break;
					}
					/*if(y == height-1)
					{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, Math.Min(width, data.Length-width*y));
					}else{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
					}*/
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
		/// Converts bitmap to byte array using internal palette.
		/// </summary>
		public static byte[] LoadBitmap(Bitmap bmp)
		{
			byte[] result = new byte[bmp.Width*bmp.Height];
			BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			for(int y = 0; y < bmp.Height; y++)
			for(int x = 0; x < bmp.Width; x++)
			{
				byte b = Marshal.ReadByte(data.Scan0, y*data.Stride+x);
				result[y*bmp.Width+x] = b;
			}
			return result;
		}
		
		/// <summary>
		/// Converts bitmap to byte array using external palette.
		/// </summary>
		public static byte[] LoadBitmap(Bitmap bmp, ImagePalette palette)
		{
			byte[] result = new byte[bmp.Width*bmp.Height];
			var rect = new Rectangle(Point.Empty, bmp.Size);
			if(bmp.Palette.Entries.SequenceEqual(palette, ColorComparer.Instance))
			{
				BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
				for(int y = 0; y < bmp.Height; y++)
				{
					Marshal.Copy(data.Scan0+data.Stride*y, result, y*bmp.Width, bmp.Width);
				}
				bmp.UnlockBits(data);
			}else{
				BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
				for(int y = 0; y < bmp.Height; y++)
				for(int x = 0; x < bmp.Width; x++)
				{
					byte b = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3);
					byte g = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3+1);
					byte r = Marshal.ReadByte(data.Scan0, y*data.Stride+x*3+2);
					result[y*bmp.Width+x] = (byte)palette.GetNearestColorIndex(Color.FromArgb(r,g,b));
				}
				bmp.UnlockBits(data);
			}
			return result;
		}
		
		private class ColorComparer : IEqualityComparer<Color>
		{
			public static readonly ColorComparer Instance = new ColorComparer();
			
			private ColorComparer()
			{
				
			}
			
			public bool Equals(Color x, Color y)
			{
				return x.ToArgb() == y.ToArgb();
			}
			
			public int GetHashCode(Color obj)
			{
				return obj.ToArgb().GetHashCode();
			}
		}
	}
}