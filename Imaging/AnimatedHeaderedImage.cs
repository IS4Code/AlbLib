using System;
using System.Drawing;
using System.IO;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image containing multiple images - frames.
	/// </summary>
	public sealed class AnimatedHeaderedImage : ImageBase, IAnimatedPaletteRenderable
	{
		/// <summary>
		/// Count of frames.
		/// </summary>
		public byte FramesCount{get;private set;}
		
		/// <summary>
		/// List of frames.
		/// </summary>
		public HeaderedImage[] Frames{get;private set;}
		
		/// <returns>Width</returns>
		public override int GetWidth()
		{
			return Frames[0].Width;
		}
		/// <returns>Height</returns>
		public override int GetHeight()
		{
			return Frames[0].Height;
		}
		
		/// <summary>
		/// Draws the animated image to bitmap.
		/// </summary>
		/// <param name="index">
		/// Zero-based frame index.
		/// </param>
		/// <param name="palette">
		/// Palette ID.
		/// </param>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public Image Render(byte index, ImagePalette palette)
		{
			return Drawing.DrawBitmap(Frames[index].ImageData, Frames[index].Width, Frames[index].Height, palette);
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
			return Render(0, palette);
		}
		
		/// <summary>
		/// Converts entire image to format-influenced byte array.
		/// </summary>
		/// <returns>
		/// Byte array containing image.
		/// </returns>
		public override byte[] ToRawData()
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public AnimatedHeaderedImage(byte[] rawdata)
		{
			if(rawdata.Length==0)return;
			short width = BitConverter.ToInt16(rawdata, 0);
			short height = BitConverter.ToInt16(rawdata, 2);
			FramesCount = rawdata[5];
			Frames = new HeaderedImage[FramesCount];
			byte[] data = new byte[width*height];
			Array.Copy(rawdata, 6, data, 0, width*height);
			Frames[0] = new HeaderedImage(width, height, data);
			int nextindex = data.Length+6;
			for(int i = 1; i < FramesCount; i++)
			{
				width = BitConverter.ToInt16(rawdata, nextindex);
				height = BitConverter.ToInt16(rawdata, nextindex+2);
				data = new byte[width*height];
				Array.Copy(rawdata, nextindex+6, data, 0, width*height);
				Frames[i] = new HeaderedImage(width, height, data);
				nextindex += data.Length+6;
			}
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public AnimatedHeaderedImage(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			short width = reader.ReadInt16();
			short height = reader.ReadInt16();
			reader.ReadByte();
			FramesCount = reader.ReadByte();
			Frames = new HeaderedImage[FramesCount];
			Frames[0] = new HeaderedImage(width, height, reader.ReadBytes(width*height));
			for(int i = 1; i < FramesCount; i++)
			{
				width = reader.ReadInt16();
				height = reader.ReadInt16();
				reader.ReadByte();
				FramesCount = reader.ReadByte();
				Frames[i] = new HeaderedImage(width, height, reader.ReadBytes(width*height));
			}
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static AnimatedHeaderedImage FromRawData(byte[] data)
		{
			if(data.Length==0)return null;
			return new AnimatedHeaderedImage(data);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static AnimatedHeaderedImage FromStream(Stream stream)
		{
			return new AnimatedHeaderedImage(stream);
		}
	}
}