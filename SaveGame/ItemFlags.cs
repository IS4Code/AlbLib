using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Special item flags.
	/// </summary>
	[Flags]
	public enum ItemFlags : byte
	{
		/// <summary>
		/// Item has no special flags.
		/// </summary>
		None = 0,
		/// <summary>
		/// Showing detailed info is available.
		/// </summary>
		ShowMoreInfo = 1,
		/// <summary>
		/// Item is broken and cannot be used.
		/// </summary>
		Broken = 2,
		/// <summary>
		/// Item is cursed.
		/// </summary>
		Cursed = 4
	}
}