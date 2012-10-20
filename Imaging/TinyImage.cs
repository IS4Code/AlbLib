using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Thumbnail variation of ILBM image.
	/// </summary>
	[Serializable]
	public sealed class TinyImage : ImageBase
	{
		/// <summary>
		/// Width of image.
		/// </summary>
		public short Width{get;private set;}
		
		/// <summary>
		/// Height of image.
		/// </summary>
		public short Height{get;private set;}
		
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
			byte[] data = new byte[ImageData.Length+4];
			BitConverter.GetBytes(Width).CopyTo(data, 0);
			BitConverter.GetBytes(Height).CopyTo(data, 2);
			ImageData.CopyTo(data, 4);
			return data;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public TinyImage(byte[] rawdata)
		{
			if(rawdata.Length==0)return;
			Width = BitConverter.ToInt16(rawdata, 0);
			Height = BitConverter.ToInt16(rawdata, 2);
			ImageData = new byte[rawdata.Length-4];
			Array.Copy(rawdata, 4, ImageData, 0, ImageData.Length);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public TinyImage(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			Width = reader.ReadInt16();
			Height = reader.ReadInt16();
			ImageData = reader.ReadBytes(Width*Height);
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public TinyImage(short width, short height, byte[] data)
		{
			Width = width;
			Height = height;
			ImageData = data;
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static TinyImage FromRawData(byte[] data)
		{
			if(data.Length==0)return null;
			return new TinyImage(data);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static TinyImage FromStream(Stream stream)
		{
			return new TinyImage(stream);
		}
	}
}