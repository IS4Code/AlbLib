using System.Drawing;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Represents an object list which can be rendered and needs index.
	/// </summary>
	public interface IAnimatedRenderable
	{
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(byte index);
	}
}