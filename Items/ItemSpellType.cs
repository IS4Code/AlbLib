using System;
namespace AlbLib.Items
{
	/// <summary>
	/// Item spell class.
	/// </summary>
	[Serializable]
	public enum ItemSpellType : byte
	{
		/// <summary>Dji Kas.</summary>
		DjiKas = 0,
		/// <summary>Enlightened one.</summary>
		EnlightenedOne = 1,
		/// <summary>Druid.</summary>
		Druid = 2,
		/// <summary>Oqulo Kamulos.</summary>
		OquloKamulos = 3
	}
}