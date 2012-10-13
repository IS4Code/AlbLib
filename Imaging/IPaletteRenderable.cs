using System.Drawing;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Represents an object which can be rendered using palette.
	/// </summary>
	public interface IPaletteRenderable
	{
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(ImagePalette palette);
		
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(ImagePalette palette, RenderOptions options);
	}
}