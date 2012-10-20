using System;
namespace AlbLib.Items
{
	/// <summary>
	/// Attribute type for item.
	/// </summary>
	[Serializable]
	public enum AttributeType : byte
	{
		/// <summary>Strength.</summary>
		Strength = 0,
		/// <summary>Intelligence.</summary>
		Intelligence = 1,
		/// <summary>Dexterity.</summary>
		Dexterity = 2,
		/// <summary>Speed.</summary>
		Speed = 3,
		/// <summary>Stamina.</summary>
		Stamina = 4,
		/// <summary>Luck.</summary>
		Luck = 5,
		/// <summary>Magic resistance.</summary>
		MagicResistance = 6,
		/// <summary>Magic talent.</summary>
		MagicTalent = 7
	}
}