using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image with basic size information. Can contain multiple frames.
	/// </summary>
	[Serializable]
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
		public int FramesCount{get{return Frames.Length;}}
		
		public HeaderedImage[] Frames{get;private set;}
		
		/*public override byte[] ImageData{
			get{
				return Frames[0].ImageData;
			}
			protected set{
				Frames[0].ImageData = value;
			}
		}*/
		
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
			byte[] data = new byte[ImageData.Length+6];
			BitConverter.GetBytes(Width).CopyTo(data, 0);
			BitConverter.GetBytes(Height).CopyTo(data, 2);
			data[5] = (byte)FramesCount;
			for(int i = 0; i < FramesCount; i++)
			{
				Frames[i].ImageData.CopyTo(data, 6+i*Width*Height);
			}
			return data;
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		/*public HeaderedImage(byte[] rawdata)
		{
			if(rawdata.Length == 0)
			{
				Frames = new byte[0][];
				return;
			}
			Width = BitConverter.ToInt16(rawdata, 0);
			Height = BitConverter.ToInt16(rawdata, 2);
			byte frames = rawdata[5];
			int bytes = Width*Height;
			Frames = new byte[frames][];
			for(int i = 0; i < frames && 6+bytes*i < rawdata.Length; i++)
			{
				Frames[i] = new byte[bytes];
				Array.Copy(rawdata, 6+bytes*i, Frames[i], 0, Math.Min(bytes, rawdata.Length-(6+bytes*i)));
			}
		}*/
		public HeaderedImage(byte[] rawdata, bool constsize) : this(new MemoryStream(rawdata), constsize)
		{
			
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderedImage(Stream stream, bool constsize)
		{
			BinaryReader reader = new BinaryReader(stream);
			Width = reader.ReadInt16();
			Height = reader.ReadInt16();
			reader.ReadByte();
			byte frames = reader.ReadByte();
			Frames = new HeaderedImage[frames];
			Frames[0] = new HeaderedImage(Width, Height, reader.ReadBytes(Width*Height));
			for(int i = 1; i < frames; i++)
			{
				short width, height;
				if(!constsize)
				{
					width = reader.ReadInt16();
					height = reader.ReadInt16();
					reader.ReadByte();
					reader.ReadByte();
				}else{
					width = Width;
					height = Height;
				}
				Frames[i] = new HeaderedImage(width, height, reader.ReadBytes(width*height));
			}
		}
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public HeaderedImage(short width, short height, byte[] data)
		{
			Width = width;
			Height = height;
			Frames = new HeaderedImage[]{this};
			ImageData = data;
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static HeaderedImage FromRawData(byte[] data, bool constsize)
		{
			if(data.Length==0)return null;
			return new HeaderedImage(data, constsize);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static HeaderedImage FromStream(Stream stream, bool constsize)
		{
			return new HeaderedImage(stream, constsize);
		}
		
		public override Image Render(RenderOptions options)
		{
			if(FramesCount == 0) return null;
			var frame = Frames[options.Frame%FramesCount];
			return Drawing.DrawBitmap(frame.ImageData, frame.Width, frame.Height, options);
		}
		
		public RawImage this[int index]
		{
			get{
				var frame = Frames[index%FramesCount];
				return new RawImage(frame.ImageData, frame.Width, frame.Height);
			}
		}
	}
}