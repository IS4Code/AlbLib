using System;
namespace AlbLib.Items
{
	/// <summary>
	/// Class of item.
	/// </summary>
	[Serializable]
	public enum ItemClass : byte
	{
		/// <summary>Unknown, unset class.</summary>
		Unknown = 0,
		/// <summary>Armour.</summary>
		Armor = 1,
		/// <summary>Helmet.</summary>
		Helmet = 2,
		/// <summary>Shoes.</summary>
		Shoes = 3,
		/// <summary>Shield.</summary>
		Shield = 4,
		/// <summary>Melee.</summary>
		CloseRangeWeapon = 5,
		/// <summary>Ranged.</summary>
		LongRangeWeapon = 6,
		/// <summary>Ammo.</summary>
		Ammo = 7,
		/// <summary>Document.</summary>
		Document = 8,
		/// <summary>Spell.</summary>
		SpellScroll = 9,
		/// <summary>Drink.</summary>
		Drink = 10,
		/// <summary>Amulet.</summary>
		Amulet = 11,
		/// <summary>Ring.</summary>
		Ring = 13,
		/// <summary>Jewel.</summary>
		Valuable = 14,
		/// <summary>Tool.</summary>
		Tool = 15,
		/// <summary>Key.</summary>
		Key = 16,
		/// <summary>Normal/junk.</summary>
		Normal = 17,
		/// <summary>Magical.</summary>
		MagicalItem = 18,
		/// <summary>Special.</summary>
		SpecialItem = 19,
		/// <summary>Lockpick.</summary>
		Lockpick = 21,
		/// <summary>Staff/torch.</summary>
		Torch = 22
	}
}