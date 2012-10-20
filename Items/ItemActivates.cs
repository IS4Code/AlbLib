using System;
namespace AlbLib.Items
{
	/// <summary>
	/// What item activates.
	/// </summary>
	[Serializable]
	public enum ItemActivates : byte
	{
		/// <summary>Compass activated.</summary>
		Compass = 0,
		/// <summary>Monster eye activated.</summary>
		MonsterEye = 1,
		/// <summary>Clock activated.</summary>
		Clock = 3
	}
}