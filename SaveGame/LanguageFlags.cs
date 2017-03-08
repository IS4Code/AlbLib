using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character language.
	/// </summary>
	[Flags, Serializable]
	public enum LanguageFlags : byte
	{
		/// <summary>
		/// None learnt.
		/// </summary>
		None = 0,
		/// <summary>
		/// Terran only.
		/// </summary>
		Terran = 1 << Language.Terran,
		/// <summary>
		/// Iskai only.
		/// </summary>
		Iskai = 1 << Language.Iskai,
		/// <summary>
		/// Celtic only.
		/// </summary>
		Celtic = 1 << Language.Celtic
	}
}