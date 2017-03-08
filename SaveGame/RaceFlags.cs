using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Item race limitation.
	/// </summary>
	[Flags, Serializable]
	public enum RaceFlags : byte
	{
		None = 0,
		Terran = 1 << Race.Terran,
		Iskai = 1 << Race.Iskai,
		Celt = 1 << Race.Celt,
		KengetKamulos = 1 << Race.KengetKamulos,
		DjiCantos = 1 << Race.DjiCantos,
		Mahino = 1 << Race.Mahino,
		Decadent = 1 << Race.Decadent,
		Umajo = 1 << Race.Umajo
	}
}