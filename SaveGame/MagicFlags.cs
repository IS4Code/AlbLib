using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character magic class.
	/// </summary>
	[Flags, Serializable]
	public enum MagicFlags : byte
	{
		/// <summary>
		/// Character can't do magic.
		/// </summary>
		None = 0,
		
		/// <summary>
		/// Dji-Kas magic.
		/// </summary>
		DjiKas = 1 << Magic.DjiKas,
		
		/// <summary>
		/// Enlightened One magic.
		/// </summary>
		EnlightenedOne = 1 << Magic.EnlightenedOne,
		
		/// <summary>
		/// Druidic magic.
		/// </summary>
		Druidic = 1 << Magic.Druidic,
		
		/// <summary>
		/// Oqulo Kamulos magic.
		/// </summary>
		OquloKamulos = 1 << Magic.OquloKamulos,
		
		/// <summary>
		/// Demonic magic.
		/// </summary>
		Demonic = 1 << Magic.Demonic
	}
}