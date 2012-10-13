using System.Drawing;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Represents an object list which can be rendered using palette and needs index.
	/// </summary>
	public interface IAnimatedPaletteRenderable
	{
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(byte index, ImagePalette palette);
		
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(byte index, ImagePalette palette, RenderOptions options);
	}
}