using System;
namespace AlbLib.Texts
{
	/// <summary>
	/// Color index palette for texts.
	/// </summary>
	[Serializable]
	public class TextColor
	{
		byte[] colors;
		
		public byte this[int color]
		{
			get{
				return colors[color];
			}
		}
		
		/// <param name="colors">
		/// Array normally of five bytes.
		/// </param>
		public TextColor(byte[] colors)
		{
			if(colors.Length < 5)throw new ArgumentException("Array must have at least 5 elements.", "colors");
			this.colors = colors;
		}
	}
}