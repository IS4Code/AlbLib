using System;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Represents format of palette when loading.
	/// </summary>
	[Serializable]
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
}