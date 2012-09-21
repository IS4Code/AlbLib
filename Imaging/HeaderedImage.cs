using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image with basic size information. Can contain multiple frames.
	/// </summary>
	public sealed class HeaderedImage : ImageBase
	{
		/// <summary>
		/// Image width.
		/// </summary>
		public short Width{get;private set;}
		
		/// <summary>
		/// Image height.
		/// </summary>
		public short Height{get;private set;}
		
		/// <summary>
		/// Number or frames.
		/// </summary>
		public byte FramesCount{get;private set;}
		
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
		/// Converts entire image to format-influenced byte array.
		/// </summary>
		/// <returns>
		/// Byte array containing image.
		/// </returns>
		public override byte[] ToRawData()
		{
			byte[] data = new byte[ImageData.Length+6];
			BitConverter.GetBytes(Width).CopyTo(data, 0);
			BitConverter.GetBytes(Height).CopyTo(data, 2);
			data[5] = FramesCount;
			ImageData.CopyTo(data, 6);
			return data;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderedImage(byte[] rawdata)
		{
			if(rawdata.Length==0)return;
			Width = BitConverter.ToInt16(rawdata, 0);
			Height = BitConverter.ToInt16(rawdata, 2);
			FramesCount = rawdata[5];
			ImageData = new byte[rawdata.Length-6];
			Array.Copy(rawdata, 6, ImageData, 0, ImageData.Length);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderedImage(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			Width = reader.ReadInt16();
			Height = reader.ReadInt16();
			reader.ReadByte();
			FramesCount = reader.ReadByte();
			ImageData = reader.ReadBytes(Width*Height);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderedImage(short width, short height, byte[] data)
		{
			Width = width;
			Height = height;
			FramesCount = 1;
			ImageData = data;
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static HeaderedImage FromRawData(byte[] data)
		{
			if(data.Length==0)return null;
			return new HeaderedImage(data);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static HeaderedImage FromStream(Stream stream)
		{
			return new HeaderedImage(stream);
		}
	}
}