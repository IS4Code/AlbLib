using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			private static readonly ImagePalette[] Palettes = new ImagePalette[short.MaxValue];
			private static ImagePalette GlobalPalette = null;
			
			/// <summary>
			/// Default transparent color index. Default is -1.
			/// </summary>
			public static int TransparentIndex = -1;
			
			/// <summary>
			/// Loads all palettes.
			/// </summary>
			public static void LoadPalettes()
			{
				int id = 0;
				foreach(string path in Paths.PaletteN.EnumerateList())
				{
					using(FileStream stream = new FileStream(path, FileMode.Open))
					{
						foreach(byte[] palette in XLDFile.EnumerateSubfiles(stream))
						{
							Palettes[id++] = ReadPalette(palette);
						}
					}
				}
				using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
				{
					GlobalPalette = ReadGlobalPalette((int)stream.Length, stream);
				}
			}
			
			private static void LoadGlobalPalette()
			{
				using(FileStream stream = new FileStream(Paths.GlobalPalette, FileMode.Open))
				{
					GlobalPalette = ReadGlobalPalette((int)stream.Length, stream);
				}
			}
			
			private static void LoadPalette(int index)
			{
				int subindex = index%100;
				int fileindex = index/100;
				using(FileStream stream = new FileStream(String.Format(Paths.PaletteN, fileindex), FileMode.Open))
				{
					int length = XLDFile.ReadToIndex(stream, subindex);
					Palettes[index] = ReadPalette(stream, length);
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
			public static ImagePalette ReadPalette(Stream input, int length)
			{
				if(length%3!=0)
				{
					throw new Exception("Palette has not appropriate length.");
				}
				return ImagePalette.Load(input, length/3, PaletteFormat.Binary);
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
			public static ImagePalette ReadPalette(byte[] palette)
			{
				if(palette.Length%3!=0)
				{
					throw new Exception("Palette has not appropriate length.");
				}
				return ImagePalette.Load(new MemoryStream(palette), palette.Length/3, PaletteFormat.Binary);
			}
			
			private static ImagePalette ReadGlobalPalette(int length, Stream stream)
			{
				if(length != 192)
				{
					throw new Exception("Global palette has not appropriate length.");
				}
				return ImagePalette.Load(stream, length/3, PaletteFormat.Binary);
			}
			
			/// <summary>
			/// Returns the global palette which is used in combination with local palette.
			/// </summary>
			/// <returns>
			/// The global palette.
			/// </returns>
			public static ImagePalette GetGlobalPalette()
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
			public static ImagePalette GetPalette(int index)
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
				return DrawBitmap(data, width, height, GetPalette(palette)+GetGlobalPalette());
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
			public static Bitmap DrawBitmap(byte[] data, int width, int height, ImagePalette palette)
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
				ImageData = new BinaryReader(stream).ReadBytes(length);
			}
			
			/// <summary>
			/// Initializes new instance.
			/// </summary>
			public RawImage(Stream stream, short width, short height)
			{
				ImageData = new BinaryReader(stream).ReadBytes(width*height);
				Width = width;
				Height = height;
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
				return Drawing.DrawBitmap(ImageData, Width, Height, ImagePalette.Create(Palette));
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
				return Drawing.DrawBitmap(Tiny.ImageData, Tiny.Width, Tiny.Height, ImagePalette.Create(Palette));
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
		
		/// <summary>
		/// Color palette used when drawing images.
		/// </summary>
		public abstract class ImagePalette : IList<Color>
		{
			/// <summary>
			/// Returns Color at index in palette.
			/// </summary>
			public abstract Color this[int index]
			{
				get;
			}
			
			Color IList<Color>.this[int index]
			{
				get{
					return this[index];
				}
				set{
					throw new NotSupportedException();
				}
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			/// <summary>
			/// Enumerates through all colors in a palette.
			/// </summary>
			/// <returns>Enumerator object.</returns>
			public virtual IEnumerator<Color> GetEnumerator()
			{
				for(int i = 0; i < this.Length; i++)
				{
					yield return this[i];
				}
			}
			
			/// <summary>
			/// Palettes are always read-only.
			/// </summary>
			public bool IsReadOnly{
				get{
					return true;
				}
			}
			
			int ICollection<Color>.Count{
				get{
					return this.Length;
				}
			}
			
			/// <summary>
			/// Gets count of all colors.
			/// </summary>
			public abstract int Length{
				get;
			}
			
			bool ICollection<Color>.Remove(Color c)
			{
				throw new NotSupportedException();
			}
			
			/// <summary>
			/// Copies colors to another array.
			/// </summary>
			/// <param name="array">Output array.</param>
			/// <param name="index">Start index.</param>
			public virtual void CopyTo(Color[] array, int index)
			{
				for(int i = 0; i < this.Length; i++)
				{
					array[index+i] = this[i];
				}
			}
			
			/// <summary>
			/// Checks if palette contains given color.
			/// </summary>
			/// <param name="item">Color to check.</param>
			/// <returns>True if <paramref name="item"/> is in palette, otherwise false.</returns>
			public virtual bool Contains(Color item)
			{
				for(int i = 0; i < this.Length; i++)
				{
					if(this[i] == item)return true;
				}
				return false;
			}
			
			void ICollection<Color>.Clear()
			{
				throw new NotSupportedException();
			}
			
			void ICollection<Color>.Add(Color item)
			{
				throw new NotSupportedException();
			}
			
			void IList<Color>.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}
			
			void IList<Color>.Insert(int index, Color item)
			{
				throw new NotSupportedException();
			}
			
			/// <summary>
			/// Returns index of color in a palette.
			/// </summary>
			/// <param name="item">Color to find.</param>
			/// <returns>Zero-based index of <paramref name="item"/>.</returns>
			public virtual int IndexOf(Color item)
			{
				for(int i = 0; i < this.Length; i++)
				{
					if(this[i] == item)return i;
				}
				return -1;
			}
			
			/// <summary>
			/// Returns index of the nearest color to <paramref name="original"/>.
			/// </summary>
			/// <param name="original">Original color.</param>
			/// <returns>Nearest color index.</returns>
			public virtual int GetNearestColorIndex(Color original)
			{
				double input_red = original.R;
				double input_green = original.G;
				double input_blue = original.B;
				double distance = 500.0;
				int nearest_color_index = -1;
				for(int i = 0; i < this.Length; i++)
				{
					Color c = this[i];
					double test_red = Math.Pow(c.R - input_red, 2.0);
					double test_green = Math.Pow(c.G - input_green, 2.0);
					double test_blue = Math.Pow(c.B - input_blue, 2.0);
					double temp = Math.Sqrt(test_blue + test_green + test_red);
					if(temp == 0.0)
					{
						return i;
					}
					else if (temp < distance)
					{
						distance = temp;
						nearest_color_index = i;
					}
				}
				return nearest_color_index;
			}
			
			/// <summary>
			/// Converts palette to color array.
			/// </summary>
			/// <returns>Array containing palette colors.</returns>
			public Color[] ToArray()
			{
				Color[] arr = new Color[this.Length];
				this.CopyTo(arr, 0);
				return arr;
			}
			
			/// <summary>
			/// Loads palette from a <paramref name="file"/>.
			/// </summary>
			/// <param name="file">Path to a file.</param>
			/// <param name="numcolors">Number of colors in a palette.</param>
			/// <param name="format">Palette format.</param>
			/// <returns>Loaded palette.</returns>
			public static ImagePalette Load(string file, int numcolors, PaletteFormat format)
			{
				using(FileStream stream = new FileStream(file, FileMode.Open))
				{
					return Load(stream, numcolors, format);
				}
			}
			
			/// <summary>
			/// Loads palette from stream.
			/// </summary>
			/// <param name="sourceStream">Stream containg color data.</param>
			/// <param name="numcolors">Number of colors in a palette.</param>
			/// <param name="format">Palette format.</param>
			/// <returns>Loaded palette.</returns>
			public static ImagePalette Load(Stream sourceStream, int numcolors, PaletteFormat format)
			{
				Color[] colors = new Color[numcolors];
				switch(format)
				{
					case PaletteFormat.Binary:
						BinaryReader binReader = new BinaryReader(sourceStream);
						for(int i = 0; i < numcolors; i++)
						{
							colors[i] = Color.FromArgb(binReader.ReadByte(),binReader.ReadByte(),binReader.ReadByte());
						}
						break;
					case PaletteFormat.Text: case PaletteFormat.TextDOS:
						StreamReader strReader = new StreamReader(sourceStream);
						for(int i = 0; i < numcolors; i++)
						{
							string line = strReader.ReadLine();
							string[] split = line.Split(' ');
							if(format == PaletteFormat.Text)
								colors[i] = Color.FromArgb(Byte.Parse(split[0]), Byte.Parse(split[1]), Byte.Parse(split[2]));
							else
								colors[i] = Color.FromArgb(Convert.ToByte(Byte.Parse(split[0])*Constants.ColorConversion), Convert.ToByte(Byte.Parse(split[1])*Constants.ColorConversion), Convert.ToByte(Byte.Parse(split[2])*Constants.ColorConversion));
						}
						break;
					default:
						throw new NotImplementedException();
				}
				return new ListPalette(colors);
			}
			
			/// <summary>
			/// Loads palette in JASC format from stream.
			/// </summary>
			/// <param name="sourceStream">Stream containg color data.</param>
			/// <returns>Loaded palette.</returns>
			public static ImagePalette LoadJASC(Stream sourceStream)
			{
				StreamReader reader = new StreamReader(sourceStream);
				if(reader.ReadLine() != "JASC-PAL")throw new Exception("Not a JASC palette.");
				if(reader.ReadLine() != "0100")throw new Exception("Unknown version.");
				int colors = Int32.Parse(reader.ReadLine());
				return Load(sourceStream, colors, PaletteFormat.Text);
			}
			
			/// <summary>
			/// Creates new palette using color list.
			/// </summary>
			/// <param name="args">Colors.</param>
			/// <returns>Newly created palette.</returns>
			public static ImagePalette Create(params Color[] args)
			{
				return new ListPalette(args);
			}
			
			/// <summary>
			/// Creates new palette using color list.
			/// </summary>
			/// <param name="list">List of colors.</param>
			/// <returns>Newly created palette.</returns>
			public static ImagePalette Create(IList<Color> list)
			{
				return new ListPalette(list);
			}
			
			/// <summary>
			/// Joins two palettes together.
			/// </summary>
			/// <param name="a">Left palette.</param>
			/// <param name="b">Right palette.</param>
			/// <returns>Joined palette.</returns>
			public static ImagePalette Join(ImagePalette a, ImagePalette b)
			{
				return new JoinPalette(a,b);
			}
			
			/// <summary>
			/// Joins two palettes together.
			/// </summary>
			/// <param name="a">Left palette.</param>
			/// <param name="b">Right palette.</param>
			/// <returns>Joined palette.</returns>
			public static ImagePalette operator +(ImagePalette a, ImagePalette b)
			{
				return new JoinPalette(a,b);
			}
			
			private class JoinPalette : ImagePalette
			{
				private readonly ImagePalette left;
				private readonly ImagePalette right;
				
				public JoinPalette(ImagePalette a, ImagePalette b)
				{
					left = a;
					right = b;
				}
				
				public override int Length{
					get{
						return left.Length+right.Length;
					}
				}
				
				public override Color this[int index]{
					get{
						if(index<left.Length)
						{
							return left[index];
						}else{
							return right[index-left.Length];
						}
					}
				}
				
				public override IEnumerator<Color> GetEnumerator()
				{
					foreach(Color c in left)
						yield return c;
					foreach(Color c in right)
						yield return c;
				}
			}
			
			private class ListPalette : ImagePalette
			{
				private readonly IList<Color> list;
				public ListPalette(IList<Color> list)
				{
					this.list = list;
				}
				
				public override int Length{
					get{
						return list.Count;
					}
				}
				
				public override Color this[int index]{
					get{
						return list[index];
					}
				}
				
				public override void CopyTo(Color[] array, int index)
				{
					list.CopyTo(array, index);
				}
				
				public override IEnumerator<Color> GetEnumerator()
				{
					return list.GetEnumerator();
				}
			}
		}
		
		/// <summary>
		/// Represents format of palette when loading.
		/// </summary>
		public enum PaletteFormat
		{
			/// <summary>
			/// Palette is stored in binary format. Each byte represents R or G or B.
			/// </summary>
			Binary,
			/// <summary>
			/// Each color is in one line, `R G B\n'
			/// </summary>
			Text,
			/// <summary>
			/// Each color is in one line, `R G B\n', ranging 0-63.
			/// </summary>
			TextDOS
		}
		
		/// <summary>
		/// Contains all interface images located in the main game executable.
		/// </summary>
		public static class MainExecutableImages
		{
			/// <summary>
			/// Checks if images have been loaded.
			/// </summary>
			public static bool Loaded{
				get{
					return images!=null;
				}
			}
			
			private static RawImage[] images;
			
			/// <summary>
			/// Returns list of all found images.
			/// </summary>
			public static IList<RawImage> Images
			{
				get{
					if(images==null)return null;
					return new ReadOnlyCollection<RawImage>(images);
				}
			}
			
			/// <summary>
			/// Loads all images.
			/// </summary>
			public static void Load()
			{
				images = new RawImage[infos.Length];
				using(FileStream stream = new FileStream(Paths.Main, FileMode.Open))
				{
					for(int i = 0; i < infos.Length; i++)
					{
						ImageLocationInfo info = infos[i];
						if(stream.Position != info.Position)
						{
							stream.Seek(info.Position, SeekOrigin.Begin);
						}
						images[i] = new RawImage(stream, info.Width, info.Height);
					}
				}
			}
			
			struct ImageLocationInfo
			{
				public long Position;
				public short Width;
				public short Height;
				
				public ImageLocationInfo(long pos, short width, short height)
				{
					Position = pos;
					Width = width;
					Height = height;
				}
			}
			
			static ImageLocationInfo[] infos = {
				new ImageLocationInfo(1031768,14,14),//defcur
				new ImageLocationInfo(1031964,16,16),//3dmovcur
				new ImageLocationInfo(1032220,16,16),//3dmovcur
				new ImageLocationInfo(1032476,16,16),//3dmovcur
				new ImageLocationInfo(1032732,16,16),//3dmovcur
				new ImageLocationInfo(1032988,16,16),//3dmovcur
				new ImageLocationInfo(1033244,16,16),//3dmovcur
				new ImageLocationInfo(1033500,16,16),//3dmovcur
				new ImageLocationInfo(1033756,16,16),//3dmovcur
				new ImageLocationInfo(1034012,16,16),//2dmovcur
				new ImageLocationInfo(1034268,16,16),//2dmovcur
				new ImageLocationInfo(1034524,16,16),//2dmovcur
				new ImageLocationInfo(1034780,16,16),//2dmovcur
				new ImageLocationInfo(1035036,16,16),//2dmovcur
				new ImageLocationInfo(1035292,16,16),//2dmovcur
				new ImageLocationInfo(1035548,16,16),//2dmovcur
				new ImageLocationInfo(1035804,16,16),//2dmovcur
				new ImageLocationInfo(1036060,14,12),//invcur
				new ImageLocationInfo(1036216,24,15),//cdcur
				new ImageLocationInfo(1036576,16,19),//hourglass
				new ImageLocationInfo(1036880,18,25),//mouse
				new ImageLocationInfo(1037330,8,8),//itemcur
				new ImageLocationInfo(1037394,20,19),//3dpntcuract
				new ImageLocationInfo(1037774,22,21),//3dpntcur
				new ImageLocationInfo(1038236,28,21),//chip
				new ImageLocationInfo(1038796,16,16),//3dmovcur
				new ImageLocationInfo(1039052,16,16),//3dmovcur
				new ImageLocationInfo(1039632,32,64),//background
				new ImageLocationInfo(1041680,3,16),//vertline1
				new ImageLocationInfo(1041728,3,16),//vertline2
				new ImageLocationInfo(1041776,3,16),//vertline3
				new ImageLocationInfo(1041824,3,16),//vertline4
				new ImageLocationInfo(1041872,16,3),//horzline1
				new ImageLocationInfo(1041920,16,3),//horzline2
				new ImageLocationInfo(1041968,16,3),//horzline3
				new ImageLocationInfo(1042016,16,3),//horzline4
				new ImageLocationInfo(1042064,16,16),//tlcor
				new ImageLocationInfo(1042320,16,16),//trcor
				new ImageLocationInfo(1042576,16,16),//blcor
				new ImageLocationInfo(1042832,16,16),//brcor
				new ImageLocationInfo(1043088,56,16),//exit1
				new ImageLocationInfo(1043984,56,16),//exit2
				new ImageLocationInfo(1044880,56,16),//exit3
				new ImageLocationInfo(1045776,8,8),//dmg
				new ImageLocationInfo(1045840,6,8),//arm
				new ImageLocationInfo(1045888,12,10),//gold
				new ImageLocationInfo(1046008,20,10),//rations
				new ImageLocationInfo(1046208,16,16),//block
				new ImageLocationInfo(1046464,16,16),//dmgditem
				new ImageLocationInfo(1046720,50,8),//bar
				new ImageLocationInfo(1047120,16,16),//cmbmove
				new ImageLocationInfo(1047376,16,16),//cmbmelee
				new ImageLocationInfo(1047632,16,16),//cmbrange
				new ImageLocationInfo(1047888,16,16),//cmbflee
				new ImageLocationInfo(1048144,16,16),//cmbcast
				new ImageLocationInfo(1048400,16,16),//cmtitem
				new ImageLocationInfo(1048656,32,27),//monster
				new ImageLocationInfo(1049520,32,27),//monsteractive
				new ImageLocationInfo(1050384,32,25),//watch
				new ImageLocationInfo(1051604,30,29),//compassDE
				new ImageLocationInfo(1052474,30,29),//compassEN
				new ImageLocationInfo(1053344,30,29),//compassFR
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054214,6,6),//compassAnim
				new ImageLocationInfo(1054502,18,18),//tileselect
				new ImageLocationInfo(1054826,22,22),//doorlock
				new ImageLocationInfo(1055309,34,48),//herzler
				new ImageLocationInfo(1056941,32,32),//damaged
				new ImageLocationInfo(1057965,32,32),//healed
				new ImageLocationInfo(1058989,14,13),//ill
				new ImageLocationInfo(1059171,16,16),//3dturn
				new ImageLocationInfo(1059427,16,16),//3dturn
				new ImageLocationInfo(1059683,16,16),//3dturn
				new ImageLocationInfo(1059939,16,16),//3dturn
				new ImageLocationInfo(1060195,16,16),//3dlook
				new ImageLocationInfo(1060451,16,16)//3dlook
			};
		}
	}
}

//1031768,14,14 defcur
//1031964,16,16 3dmovcur
//1032220,16,16 3dmovcur
//1032476,16,16 3dmovcur
//1032732,16,16 3dmovcur
//1032988,16,16 3dmovcur
//1033244,16,16 3dmovcur
//1033500,16,16 3dmovcur
//1033756,16,16 3dmovcur
//1034012,16,16 2dmovcur
//1034268,16,16 2dmovcur
//1034524,16,16 2dmovcur
//1034780,16,16 2dmovcur
//1035036,16,16 2dmovcur
//1035292,16,16 2dmovcur
//1035548,16,16 2dmovcur
//1035804,16,16 2dmovcur
//1036060,14,11 invcur
//1036216,24,15 cdcur
//1036576,16,19 hourglass
//1036880,18,25 mouse
//1037330,8,8 itemcur
//1037394,20,19 3dpntcuract
//1037774,22,21 3dpntcur
//1038236,28,21 chip
//1038796,16,16 3dmovcur
//1039052,16,16 3dmovcur
//1039632,32,64 background
//1041680,3,16 vertline1
//1041728,3,16 vertline2
//1041776,3,16 vertline3
//1041824,3,16 vertline4
//1041872,16,3 horzline1
//1041920,16,3 horzline2
//1041968,16,3 horzline3
//1042016,16,3 horzline4
//1042064,16,16 tlcor
//1042320,16,16 trcor
//1042576,16,16 blcor
//1042832,16,16 brcor
//1043088,56,16 exit1
//1043984,56,16 exit2
//1044880,56,16 exit3
//1045776,8,8 dmg
//1045840,6,8 arm
//1045888,12,10 gold
//1046008,20,10 rations
//1046208,16,16 block
//1046464,16,16 dmgditem
//1046720,50,8 bar
//1047120,16,16 cmbmove
//1047376,16,16 cmbmelee
//1047632,16,16 cmbrange
//1047888,16,16 cmbflee
//1048144,16,16 cmbcast
//1048400,16,16 cmtitem
//1048656,32,27 monster
//1049520,32,27 monsteractive
//1050384,32,27 watch
//1051604,30,29 compassDE
//1052474,30,29 compassEN
//1053344,30,29 compassFR
//1054214,6,6×8 compassAnim
//1054502,18,18 tileselect
//1054826,22,22 doorlock
//1055309,34,48 herzler
//1056941,32,32 damaged
//1057965,32,32 healed
//1058989,14,13 ill
//1059171,16,16 3dturn
//1059427,16,16 3dturn
//1059683,16,16 3dturn
//1059939,16,16 3dturn
//1060195,16,16 3dlook
//1060451,16,16 3dlook