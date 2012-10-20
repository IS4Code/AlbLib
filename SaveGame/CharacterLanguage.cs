using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character language.
	/// </summary>
	[Flags, Serializable]
	public enum CharacterLanguage : byte
	{
		/// <summary>
		/// None learnt.
		/// </summary>
		None = 0,
		/// <summary>
		/// Terran only.
		/// </summary>
		Terran = 1,
		/// <summary>
		/// Iskai only.
		/// </summary>
		Iskai = 2,
		/// <summary>
		/// Celtic only.
		/// </summary>
		Celtic = 4
	}
}