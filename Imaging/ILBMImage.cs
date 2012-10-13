using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AlbLib.IFF;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Image in ILBM format, containing many informations. Currently read-only.
	/// </summary>
	public sealed class ILBMImage : ImageBase, IRenderable
	{
		/// <summary>
		/// Width of image.
		/// </summary>
		public short Width{get;private set;}
		
		/// <summary>
		/// Height of image.
		/// </summary>
		public short Height{get;private set;}
		
		/// <summary>
		/// Parent-relative position X.
		/// </summary>
		public short PosX{get;private set;}
		
		/// <summary>
		/// Parent-relative position Y.
		/// </summary>
		public short PosY{get;private set;}
		
		/// <summary>
		/// Number of image planes.
		/// </summary>
		public byte NumPlanes{get;private set;}
		
		/// <summary>
		/// Image mask type.
		/// </summary>
		public byte Mask{get;private set;}
		
		/// <summary>
		/// Image compression type.
		/// </summary>
		public byte Compression{get;private set;}
		
		/// <summary>
		/// Parent-relative padding.
		/// </summary>
		public byte Padding{get;private set;}
		
		/// <summary>
		/// Transparent color index.
		/// </summary>
		public short Transparent{get;private set;}
		
		/// <summary>
		/// Aspect ratio when viewing.
		/// </summary>
		public short AspectRatio{get;private set;}
		
		/// <summary>
		/// Page width when viewing.
		/// </summary>
		public short PageWidth{get;private set;}
		
		/// <summary>
		/// Page height when viewing.
		/// </summary>
		public short PageHeight{get;private set;}
		
		/// <summary>
		/// Image palette.
		/// </summary>
		public ImagePalette Palette{get;private set;}
		
		/// <summary>
		/// Hotspot position X. Mostly on cursors.
		/// </summary>
		public short HotspotX{get;private set;}
		
		/// <summary>
		/// Hotspot position Y. Mostly on cursors.
		/// </summary>
		public short HotspotY{get;private set;}
		
		/// <summary>
		/// Thumbnail.
		/// </summary>
		public TinyImage Tiny{get;private set;}
		
		/// <summary>
		/// Color ranges.
		/// </summary>
		public IList<ColorRange> ColorRanges{get;private set;}
		
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
		
		/// <param name="rawdata">
		/// Byte array containing ILBM image data.
		/// </param>
		public ILBMImage(byte[] rawdata) : this(new MemoryStream(rawdata))
		{
			if(rawdata.Length==0)return;
			/*using(MemoryStream stream = new MemoryStream(rawdata))
			{
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				ReadNext(reader);
			}*/
		}
		
		/// <param name="stream">
		/// Stream containing ILBM image data.
		/// </param>
		public ILBMImage(Stream stream)
		{
			//BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			//ReadNext(reader);
			
			IFFReader reader = new IFFReader(stream);
			var file = reader.ReadFileHeader();
			if(file.FormatID != "PBM ")
			{
				throw new NotSupportedException("This is not supported IBLM file.");
			}
			
			foreach(IFFChunk chunk in reader.ReadAll())
			{
				switch(chunk.TypeID)
				{
					case "BMHD":
						Width = reader.ReadInt16();
						Height = reader.ReadInt16();
						PosX = reader.ReadInt16();
						PosY = reader.ReadInt16();
						NumPlanes = reader.ReadByte();
						Mask = reader.ReadByte();
						Compression = reader.ReadByte();
						Padding = reader.ReadByte();
						Transparent = reader.ReadInt16();
						AspectRatio = reader.ReadInt16();
						PageWidth = reader.ReadInt16();
						PageHeight = reader.ReadInt16();
						break;
					case "CMAP":
						Color[] pal = new Color[chunk.Length/3];
						for(int i = 0; i < pal.Length; i++)
						{
							byte R = reader.ReadByte();
							byte G = reader.ReadByte();
							byte B = reader.ReadByte();
							pal[i] = Color.FromArgb(R, G, B);
						}
						Palette = ImagePalette.Create(pal);
						break;
					case "GRAB":
						HotspotX = reader.ReadInt16();
						HotspotY = reader.ReadInt16();
						break;
					case "CRNG":
						if(ColorRanges == null)ColorRanges = new List<ColorRange>();
						ColorRanges.Add(new ColorRange(reader));
						break;
					case "TINY":
						short width = reader.ReadInt16();
						short height = reader.ReadInt16();
						byte[] tiny;
						if(Compression == 1)
						{
							tiny = reader.ReadUnpack(chunk.Length-4);
						}else{
							tiny = reader.ReadBytes(chunk.Length-4);
						}
						Tiny = new TinyImage(width, height, tiny);
						break;
					case "BODY":
						if(Compression == 1)
						{
							ImageData = reader.ReadUnpack(chunk.Length);
						}else{
							ImageData = reader.ReadBytes(chunk.Length);
						}
						break;
				}
			}
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
		/// Draws the image to bitmap using its own palette.
		/// </summary>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public Image Render()
		{
			return Drawing.DrawBitmap(ImageData, Width, Height, ImagePalette.Create(Palette), null);
		}
		
		public Image Render(RenderOptions options)
		{
			return Drawing.DrawBitmap(ImageData, Width, Height, ImagePalette.Create(Palette), options);
		}
		
		/// <summary>
		/// Draws the thumbnail to bitmap.
		/// </summary>
		/// <param name="palette">
		/// Palette ID.
		/// </param>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public Image RenderTiny(ImagePalette palette)
		{
			return Tiny.Render(palette);
		}
		
		public Image RenderTiny(ImagePalette palette, RenderOptions options)
		{
			return Tiny.Render(palette, options);
		}
		
		/// <summary>
		/// Draws the thumbnail to bitmap.
		/// </summary>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public Image RenderTiny()
		{
			return Drawing.DrawBitmap(Tiny.ImageData, Tiny.Width, Tiny.Height, ImagePalette.Create(Palette), null);
		}
		
		public Image RenderTiny(RenderOptions options)
		{
			return Drawing.DrawBitmap(Tiny.ImageData, Tiny.Width, Tiny.Height, ImagePalette.Create(Palette), options);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static ILBMImage FromRawData(byte[] data)
		{
			if(data.Length==0)return null;
			return new ILBMImage(data);
		}
		
		/// <summary>
		/// Creates new instance.
		/// </summary>
		public static ILBMImage FromStream(Stream stream)
		{
			return new ILBMImage(stream);
		}
		
		/// <summary>
		/// Maybe useful for palette swapping?
		/// </summary>
		public struct ColorRange
		{
			private readonly short pad1;
			
			/// <summary>
			/// Animation rate.
			/// </summary>
			public short Rate{get;set;}
			
			/// <summary>
			/// Range flags.
			/// </summary>
			public ColorRangeFlags Flags{get;set;}
			
			/// <summary>
			/// Low color index.
			/// </summary>
			public byte Low{get;set;}
			
			/// <summary>
			/// High color index.
			/// </summary>
			public byte High{get;set;}
			
			/// <summary>
			/// Reads range from IFF stream.
			/// </summary>
			public ColorRange(IFFReader reader) : this()
			{
				pad1 = reader.ReadInt16();
				Rate = reader.ReadInt16();
				Flags = (ColorRangeFlags)reader.ReadInt16();
				Low = reader.ReadByte();
				High = reader.ReadByte();
			}
			
			/// <summary>
			/// Reads range from normal stream.
			/// </summary>
			public ColorRange(Stream stream) : this()
			{
				BinaryReader reader = new BinaryReader(stream);
				pad1 = reader.ReadInt16();
				Rate = reader.ReadInt16();
				Flags = (ColorRangeFlags)reader.ReadInt16();
				Low = reader.ReadByte();
				High = reader.ReadByte();
			}
		}
		
		/// <summary>
		/// Range flags.
		/// </summary>
		public enum ColorRangeFlags : short
		{
			/// <summary>
			/// None.
			/// </summary>
			None = 0,
			
			/// <summary>
			/// Animation is active.
			/// </summary>
			Active = 1,
			
			/// <summary>
			/// Animation is reversed.
			/// </summary>
			Reversed = 2
		}
	}
}