using System.Drawing;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Base image class.
	/// </summary>
	public abstract class ImageBase : IPaletteRenderable
	{
		/// <returns>Width</returns>
		public abstract int GetWidth();
		/// <returns>Height</returns>
		public abstract int GetHeight();
		
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
		public virtual Image Render(int palette)
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
		public virtual Image Render(ImagePalette palette)
		{
			return Render(palette, null);
		}
		
		public virtual Image Render(ImagePalette palette, RenderOptions options)
		{
			return Drawing.DrawBitmap(ImageData, GetWidth(), GetHeight(), palette, options);
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