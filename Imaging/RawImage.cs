using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image that consists only of pixels with no other data.
	/// </summary>
	[Serializable]
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
		
		/*public override byte[] ImageData{
			get{
				return Frames.Aggregate(Enumerable.Empty<byte>(), (a,e)=>a.Concat(e)).ToArray();
			}
			protected set{
				Frames = new[][]{value};
			}
		}*/
		
		public byte[][] GetFrames()
		{
			byte[][] arr = new byte[ImageData.Length/(Width*Height)][];
			for(int i = 0; i < arr.Length; i++)
			{
				byte[] elem = new byte[Width*Height];
				Array.Copy(ImageData, i*Width*Height, elem, 0, elem.Length);
				arr[i] = elem;
			}
			return arr;
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
			ImageData = new byte[length];
			stream.Read(ImageData, 0, length);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(Stream stream, int width, int height) : this(stream, width, height, width*height)
		{
			
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public RawImage(Stream stream, int width, int height, int length)
		{
			ImageData = new byte[length];
			
			stream.Read(ImageData, 0, length);
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
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static RawImage FromBitmap(Bitmap bmp)
		{
			return new RawImage(Drawing.LoadBitmap(bmp), bmp.Width, bmp.Height);
		}
	}
}