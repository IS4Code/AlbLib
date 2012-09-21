using System;
namespace AlbLib.SaveGame
{
	/// <summary>
	/// Character condition states.
	/// </summary>
	[Flags]
	public enum CharacterConditions : ushort
	{
		/// <summary>
		/// Unconscious.
		/// </summary>
		Unconscious = 0x0100,
		/// <summary>
		/// Poisoned.
		/// </summary>
		Poisoned = 0x0200,
		/// <summary>
		/// Ill.
		/// </summary>
		Ill = 0x0400,
		/// <summary>
		/// Exhausted.
		/// </summary>
		Exhausted = 0x0800,
		/// <summary>
		/// Paralyzed.
		/// </summary>
		Paralyzed = 0x1000,
		/// <summary>
		/// Fleeing.
		/// </summary>
		Fleeing = 0x2000,
		/// <summary>
		/// Intoxicated.
		/// </summary>
		Intoxicated = 0x4000,
		/// <summary>
		/// Blind.
		/// </summary>
		Blind = 0x8000,
		/// <summary>
		/// Panicking.
		/// </summary>
		Panicking = 0x01,
		/// <summary>
		/// Asleep.
		/// </summary>
		Asleep = 0x02,
		/// <summary>
		/// Insane.
		/// </summary>
		Insane = 0x04,
		/// <summary>
		/// Irritated.
		/// </summary>
		Irritated = 0x08
	}
}