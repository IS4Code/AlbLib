namespace AlbLib.Localization
{
	/// <summary>
	/// Color index palette for texts.
	/// </summary>
	public class TextColor
	{
		byte[] colors;
		
		/// <param name="colors">
		/// Array normally of five bytes.
		/// </param>
		public TextColor(byte[] colors)
		{
			this.colors = colors;
		}
	}
}