using System;
namespace AlbLib.Texts
{
	/// <summary>
	/// Style of font.
	/// </summary>
	[Serializable]
	public enum FontStyle : byte
	{
		/// <summary>
		/// Normal text (subfile 0).
		/// </summary>
		Normal = 0,
		/// <summary>
		/// Bold text (subfile 1).
		/// </summary>
		Bold = 1
	}
}