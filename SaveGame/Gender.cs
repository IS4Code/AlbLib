using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Gender of character.
	/// </summary>
	[Serializable]
	public enum Gender : byte
	{
		/// <summary>
		/// Male.
		/// </summary>
		Male = 0,
		/// <summary>
		/// Female.
		/// </summary>
		Female = 1,
		/// <summary>
		/// Asexual.
		/// </summary>
		Asexual = 2
	}
}