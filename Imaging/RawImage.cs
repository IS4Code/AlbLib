using System.Drawing;
using System.IO;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image that consists only of pixels with no other data.
	/// </summary>
	public sealed class RawImage : ImageBase
	{
		/// <summary>
		/// Image width is not stored within image data.
		/// </summary>
		public int Width{get; set;}
		
		/// <summary>
		/// Image height is not stored within image data.
		/// </summary>
		public int Height{get; set;}
		
		/// <returns>Width</returns>
		public override int GetWidth()
		{
			return Width;
		}
		/// <returns>Height</returns>
		public override int GetHeight()
		{
			return Height;
		}
		
		/// <summary>
		/// Converts entire image to format-influenced byte array.
		/// </summary>
		/// <returns>
		/// Byte array containing image.
		/// </returns>
		public override byte[] ToRawData()
		{
			return ImageData;
		}
		
		/// <summary>
		/// Draws the image to bitmap.
		/// </summary>
		/// <param name="palette">
		/// Palette ID.
		/// </param>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public override Image Render(ImagePalette palette)
		{
			return Drawing.DrawBitmap(ImageData, Width, Height, palette);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(byte[] rawdata)
		{
			if(rawdata.Length==0)return;
			ImageData = rawdata;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(Stream stream, int length)
		{
			ImageData = new BinaryReader(stream).ReadBytes(length);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(Stream stream, int width, int height)
		{
			ImageData = new BinaryReader(stream).ReadBytes(width*height);
			Width = width;
			Height = height;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(byte[] data, int width)
		{
			Width = width;
			Height = (data.Length+width-1)/width;
			ImageData = data;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(byte[] data, int width, int height)
		{
			Width = width;
			Height = height;
			ImageData = data;
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static RawImage FromRawData(byte[] data)
		{
			if(data.Length==0)return null;
			return new RawImage(data);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static RawImage FromStream(Stream stream, int length)
		{
			return new RawImage(stream, length);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static RawImage FromBitmap(Bitmap bmp, ImagePalette palette)
		{
			return new RawImage(Drawing.LoadBitmap(bmp, palette), bmp.Width, bmp.Height);
		}
	}
}