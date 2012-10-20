using System.Drawing;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Represents an object which can be rendered.
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// Renders object.
		/// </summary>
		Image Render(RenderOptions options);
	}
}