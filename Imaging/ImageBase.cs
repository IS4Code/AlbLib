using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Base image class.
	/// </summary>
	[Serializable]
	public abstract class ImageBase : IRenderable, IGameResource
	{
		/// <returns>Width</returns>
		public abstract int GetWidth();
		/// <returns>Height</returns>
		public abstract int GetHeight();
		
		/// <summary>
		/// Byte array containing actual pixels of image.
		/// </summary>
		public virtual byte[] ImageData{get;protected set;}
		
		/// <summary>
		/// Converts entire image to format-influenced byte array.
		/// </summary>
		/// <returns>
		/// Byte array containing image.
		/// </returns>
		public abstract byte[] ToRawData();
		
		public int Save(Stream output)
		{
			byte[] data = this.ToRawData();
			output.Write(data, 0, data.Length);
			return data.Length;
		}
		
		public bool Equals(IGameResource obj)
		{
			return this.Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			if(obj is ImageBase)
			{
				return ((ImageBase)obj).ImageData.SequenceEqual(this.ImageData);
			}else{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			return ImageData.GetHashCode();
		}
		
		public static bool operator ==(ImageBase lhs, ImageBase rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(ImageBase lhs, ImageBase rhs)
		{
			return !(lhs == rhs);
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
		public Image Render(int palette)
		{
			return Render(ImagePalette.GetFullPalette(palette));
		}
		
		/// <summary>
		/// Draws the image to bitmap.
		/// </summary>
		/// <param name="palette">
		/// Palette.
		/// </param>
		/// <returns>
		/// Drawn image.
		/// </returns>
		public Image Render(ImagePalette palette)
		{
			return Render(new RenderOptions(palette));
		}
		
		public virtual Image Render(RenderOptions options)
		{
			return Drawing.DrawBitmap(ImageData, GetWidth(), GetHeight(), options);
		}
		
		public static byte[,] Assemble(byte[] arr, int width, int height)
		{
			byte[,] result = new byte[width,height];
			for(int y = 0; y < width; y++)
			for(int x = 0; x < height; x++)
			{
				result[x,y] = arr[y*width+x];
			}
			return result;
		}
		
		public static byte[] Disassemble(byte[,] arr)
		{
			byte[] result = new byte[arr.Length];
			int width = arr.GetLength(0);
			int height = arr.GetLength(1);
			for(int y = 0; y < width; y++)
			for(int x = 0; x < height; x++)
			{
				result[y*width+x] = arr[x,y];
			}
			return result;
		}
		
		public RawImage ExtractLight()
		{
			byte[] res = new byte[ImageData.Length];
			for(int i = 0; i < res.Length; i++)
			{
				byte b = ImageData[i];
				if(b >= 192)
					res[i] = b;
			}
			return new RawImage(res, GetWidth(), GetHeight());
		}
		
		public bool ExtractLight(out RawImage exported)
		{
			byte[] res = new byte[ImageData.Length];
			bool success = false;
			for(int i = 0; i < res.Length; i++)
			{
				byte b = ImageData[i];
				if(b >= 192)
				{
					res[i] = b;
					success = true;
				}
			}
			exported = new RawImage(res, GetWidth(), GetHeight());
			return success;
		}
	}
}