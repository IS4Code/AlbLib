/* Date: 29.8.2014, Time: 17:23 */
using System;

namespace AlbLib.SaveGame
{
	[Serializable]
	public enum Magic : byte
	{
		/// <summary>
		/// Dji-Kas magic.
		/// </summary>
		DjiKas = 0,
		
		/// <summary>
		/// Enlightened One magic.
		/// </summary>
		EnlightenedOne = 1,
		
		/// <summary>
		/// Druidic magic.
		/// </summary>
		Druidic = 2,
		
		/// <summary>
		/// Oqulo Kamulos magic.
		/// </summary>
		OquloKamulos = 3,
		
		/// <summary>
		/// Demonic magic.
		/// </summary>
		Demonic = 5
	}
}
