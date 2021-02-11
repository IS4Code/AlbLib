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
		Man = 0,
		/// <summary>
		/// Female.
		/// </summary>
		Woman = 1,
		/// <summary>
		/// Asexual.
		/// </summary>
		Creature = 2
	}
}