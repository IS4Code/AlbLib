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
		public virtual Image Render(byte palette)
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
		public abstract Image Render(ImagePalette palette);
	}
}