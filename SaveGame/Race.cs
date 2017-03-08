using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Race of character.
	/// </summary>
	[Serializable]
	public enum Race : byte
	{
		/// <summary>
		/// Terran.
		/// </summary>
		Terran = 0,
		/// <summary>
		/// Iskai.
		/// </summary>
		Iskai = 1,
		/// <summary>
		/// Celt.
		/// </summary>
		Celt = 2,
		/// <summary>
		/// Kenget Kamulos.
		/// </summary>
		KengetKamulos = 3,
		/// <summary>
		/// Dji-Cantos.
		/// </summary>
		DjiCantos = 4,
		/// <summary>
		/// Mahino.
		/// </summary>
		Mahino = 5,
		/// <summary>
		/// Decadent.
		/// </summary>
		Decadent = 6,
		/// <summary>
		/// Umajo.
		/// </summary>
		Umajo = 7,
		/// <summary>
		/// Monster.
		/// </summary>
		Monster = 14
	}
}