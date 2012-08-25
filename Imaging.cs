using System;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using AlbLib.IFF;
using AlbLib.XLD;

namespace AlbLib
{
	namespace Imaging
	{
		/// <summary>
		/// Main class which works with palettes and draws images.
		/// </summary>
		public static class Drawing
		{
			private static readonly Color[][] Palettes = new Color[short.MaxValue][];
			private static Color[] GlobalPalette = null;
			
			/// <summary>
			/// Default transparent color index. Default is -1.
			/// </summary>
			public static int TransparentIndex = -1;
			
			/// <summary>
			/// Loads all palettes.
			/// </summary>
			public static void LoadPalettes()
			{
				foreach(string path in Paths.PaletteN.EnumerateList())
				{
					using(FileStream stream = new FileStream(path, FileMode.Open))
					{
						foreach(byte[] palette in XLDFile.EnumerateSubfiles(stream))
						{
							ReadPalette(palette);
						}
					}
				}
				using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
				{
					ReadGlobalPalette((int)stream.Length, new BinaryReader(stream, Encoding.ASCII));
				}
			}
			
			private static void LoadGlobalPalette()
			{
				using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
				{
					ReadGlobalPalette((int)stream.Length, new BinaryReader(stream, Encoding.ASCII));
				}
			}
			
			private static void LoadPalette(int index)
			{
				int subindex = index%100;
				int fileindex = index/100;
				using(FileStream stream = new FileStream(String.Format(Paths.PaletteN, fileindex), FileMode.Open))
				{
					int length = XLDFile.ReadToIndex(stream, subindex);
					ReadPalette(stream, length);
				}
			}
			
			/// <summary>
			/// Parses palette from stream.
			/// </summary>
			/// <param name="input">
			/// Input stream containing palette data.
			/// </param>
			/// <param name="length">
			/// Length of palette bytes. Usually triple of colors count.
			/// </param>
			/// <returns>
			/// Palette as color array.
			/// </returns>
			public static Color[] ReadPalette(Stream input, int length)
			{
				if(length%3!=0)
				{
					throw new Exception("Palette has not appropriate length.");
				}
				BinaryReader reader = new BinaryReader(input);
				Color[] cols = new Color[length/3];
				for(int i = 0; i < length/3; i++)
				{
					byte R = reader.ReadByte();
					byte G = reader.ReadByte();
					byte B = reader.ReadByte();
					cols[i] = Color.FromArgb(R, G, B);
				}
				return cols;
			}
			
			/// <summary>
			/// Parses palette from byte array.
			/// </summary>
			/// <param name="palette">
			/// Palette data as bytes. Usually multiple of three.
			/// </param>
			/// <returns>
			/// Palette as color array.
			/// </returns>
			public static Color[] ReadPalette(byte[] palette)
			{
				if(palette.Length%3!=0)
				{
					throw new Exception("Palette has not appropriate length.");
				}
				Color[] cols = new Color[palette.Length/3];
				for(int i = 0; i < palette.Length/3; i++)
				{
					byte R = palette[i*3];
					byte G = palette[i*3+1];
					byte B = palette[i*3+2];
					cols[i] = Color.FromArgb(R, G, B);
				}
				return cols;
			}
			
			private static void ReadGlobalPalette(int length, BinaryReader reader)
			{
				if(length != 192)
				{
					throw new Exception("Global palette has not appropriate length.");
				}
				GlobalPalette = new Color[64];
				for(int i = 0; i < length/3; i++)
				{
					byte R = reader.ReadByte();
					byte G = reader.ReadByte();
					byte B = reader.ReadByte();
					GlobalPalette[i] = Color.FromArgb(R, G, B);
				}
			}
			
			/// <summary>
			/// Returns the global palette which is used in combination with local palette.
			/// </summary>
			/// <returns>
			/// The global palette.
			/// </returns>
			public static Color[] GetGlobalPalette()
			{
				if(GlobalPalette == null)
				{
					LoadGlobalPalette();
				}
				return GlobalPalette;
			}
			
			/// <summary>
			/// Gets local palette using specified <paramref name="index"/>.
			/// </summary>
			/// <param name="index">
			/// Zero-based index.
			/// </param>
			/// <returns>
			/// The local palette.
			/// </returns>
			public static Color[] GetPalette(int index)
			{
				if(Palettes[index] == null)
				{
					LoadPalette(index);
				}
				return Palettes[index];
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
			/// Used palette index.
			/// </param>
			/// <returns>
			/// Drawn bitmap.
			/// </returns>
			public static Bitmap DrawBitmap(byte[] data, int width, int height, int palette)
			{
				return DrawBitmap(data, width, height, GetPalette(palette), GetGlobalPalette());
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
			/// <param name="palette1">
			/// Local palette.
			/// </param>
			/// <param name="palette2">
			/// Global palette.
			/// </param>
			/// <returns>
			/// Drawn bitmap.
			/// </returns>
			public static Bitmap DrawBitmap(byte[] data, int width, int height, Color[] palette1, Color[] palette2)
			{
				Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
				ColorPalette pal = bmp.Palette;
				palette1.CopyTo(pal.Entries, 0);
				palette2.CopyTo(pal.Entries, 192);
				if(TransparentIndex >= 0)pal.Entries[TransparentIndex] = Color.Transparent;
				bmp.Palette = pal;
				BitmapData bmpdata = bmp.LockBits(new Rectangle(0,0,width,height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
				if(width%4 == 0)
				{
					Marshal.Copy(data, 0, bmpdata.Scan0, data.Length);
				}else{
					for(int y = 0; y < height; y++)
					{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
					}
				}
				bmp.UnlockBits(bmpdata);
				return bmp;
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
			/// Only one color palette.
			/// </param>
			/// <returns></returns>
			public static Bitmap DrawBitmap(byte[] data, int width, int height, Color[] palette)
			{
				Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
				ColorPalette pal = bmp.Palette;
				palette.CopyTo(pal.Entries, 0);
				if(TransparentIndex >= 0)pal.Entries[TransparentIndex] = Color.Transparent;
				bmp.Palette = pal;
				BitmapData bmpdata = bmp.LockBits(new Rectangle(0,0,width,height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
				if(width%4 == 0)
				{
					Marshal.Copy(data, 0, bmpdata.Scan0, data.Length);
				}else{
					for(int y = 0; y < height; y++)
					{
						Marshal.Copy(data, width*y, bmpdata.Scan0+bmpdata.Stride*y, width);
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
			public static Bitmap DrawBitmap(byte[] data, int width, short palette)
			{
				int height = (data.Length+width-1)/width;
				return DrawBitmap(data, width, height, palette);
			}
		}
		
		/// <summary>
		/// Base image class.
		/// </summary>
		public abstract class ImageBase
		{
			/// <summary>
			/// Byte array containing actual pixels of image.
			/// </summary>
			public byte[] ImageData{get;protected set;}
			
			/// <summary>
			/// Converts entire image to format-influenced byte array.
			/// </summary>
			/// <returns>
			/// Byte array containing image.
			/// </returns>
			public abstract byte[] ToRawData();
			
			/// <summary>
			/// Draws the image to bitmap.
			/// </summary>
			/// <param name="palette">
			/// Palette ID.
			/// </param>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public abstract Bitmap DrawToBitmap(short palette);
		}
		
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
			public override Bitmap DrawToBitmap(short palette)
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
				ImageData = new BinaryReader(stream, Encoding.ASCII).ReadBytes(length);
			}
			
			/// <summary>
			/// Initializes new instance.
			/// </summary>
			public RawImage(short width, byte[] data)
			{
				Width = width;
				Height = (data.Length+width-1)/width;
				ImageData = data;
			}
			
			/// <summary>
			/// Initializes new instance.
			/// </summary>
			public RawImage(short width, short height, byte[] data)
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
		}
		
		/// <summary>
		/// Image in ILBM format, containing many informations. Currently read-only.
		/// </summary>
		public sealed class ILBMImage : ImageBase
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
			public Color[] Palette{get;private set;}
			
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
							Palette = new Color[chunk.Length/3];
							for(int i = 0; i < Palette.Length; i++)
							{
								byte R = reader.ReadByte();
								byte G = reader.ReadByte();
								byte B = reader.ReadByte();
								Palette[i] = Color.FromArgb(R, G, B);
							}
							break;
						case "GRAB":
							HotspotX = reader.ReadInt16();
							HotspotY = reader.ReadInt16();
							break;
						case "TINY":
							short width = reader.ReadInt16();
							short height = reader.ReadInt16();
							byte[] tiny = reader.ReadBytes(chunk.Length-4);
							if(Compression == 1)
							{
								tiny = IFFReader.Decompress(tiny, width*height);
							}
							Tiny = new TinyImage(width, height, tiny);
							break;
						case "BODY":
							byte[] data = reader.ReadBytes(chunk.Length);
							if(Compression == 1)
							{
								data = IFFReader.Decompress(data, Width*Height);
							}
							ImageData = data;
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
			/// Draws the image to bitmap using specified <paramref name="palette"/>.
			/// </summary>
			/// <param name="palette">
			/// Palette ID.
			/// </param>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public override Bitmap DrawToBitmap(short palette)
			{
				if(palette >= 0)
					return Drawing.DrawBitmap(ImageData, Width, Height, palette);
				else return DrawToBitmap();
			}
			
			/// <summary>
			/// Draws the image to bitmap using its own palette.
			/// </summary>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public Bitmap DrawToBitmap()
			{
				return Drawing.DrawBitmap(ImageData, Width, Height, Palette);
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
			public Bitmap DrawTiny(short palette)
			{
				return Tiny.DrawToBitmap(palette);
			}
			
			/// <summary>
			/// Draws the thumbnail to bitmap.
			/// </summary>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public Bitmap DrawTiny()
			{
				return Drawing.DrawBitmap(Tiny.ImageData, Tiny.Width, Tiny.Height, Palette);
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
		}
		
		/// <summary>
		/// Thumbnail variation of ILBM image.
		/// </summary>
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
			
			/// <summary>
			/// Draws the image to bitmap.
			/// </summary>
			/// <param name="palette">
			/// Palette ID.
			/// </param>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public override Bitmap DrawToBitmap(short palette)
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
			
			/// <summary>
			/// Draws the image to bitmap.
			/// </summary>
			/// <param name="palette">
			/// Palette ID.
			/// </param>
			/// <returns>
			/// Drawn image.
			/// </returns>
			public override Bitmap DrawToBitmap(short palette)
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
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
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
		
		/// <summary>
		/// Image containing multiple images - frames.
		/// </summary>
		public sealed class AnimatedHeaderedImage : ImageBase
		{
			/// <summary>
			/// Count of frames.
			/// </summary>
			public byte FramesCount{get;private set;}
			
			/// <summary>
			/// List of frames.
			/// </summary>
			public HeaderedImage[] Frames{get;private set;}
			
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
			public Bitmap DrawToBitmap(byte index, short palette)
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
			public override Bitmap DrawToBitmap(short palette)
			{
				return DrawToBitmap(0, palette);
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
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
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
}