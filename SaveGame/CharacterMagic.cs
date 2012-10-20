using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character magic class.
	/// </summary>
	[Flags, Serializable]
	public enum CharacterMagic : byte
	{
		/// <summary>
		/// Character can't do magic.
		/// </summary>
		None = 0,
		/// <summary>
		/// Dji Kas.
		/// </summary>
		DjiKas = 1,
		/// <summary>
		/// Enlightened one.
		/// </summary>
		EnlightenedOne = 2,
		/// <summary>
		/// Druid.
		/// </summary>
		Druid = 4,
		/// <summary>
		/// Oqulo.
		/// </summary>
		OquloKamulos = 8
	}
}