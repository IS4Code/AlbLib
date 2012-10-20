using System;
namespace AlbLib.Items
{
	/// <summary>
	/// Item slot.
	/// </summary>
	[Serializable]
	public enum ItemSlot : byte
	{
		/// <summary>Inventory item only.</summary>
		Inventory = 0,
		/// <summary>Necklace.</summary>
		Neck = 1,
		/// <summary>Helmet.</summary>
		Head = 2,
		/// <summary>Tail only.</summary>
		Tail = 3,
		/// <summary>Right hand.</summary>
		RightHand = 4,
		/// <summary>Chest.</summary>
		Chest = 5,
		/// <summary>Left hand.</summary>
		LeftHand = 6,
		/// <summary>Right finger ring.</summary>
		RightFinger = 7,
		/// <summary>Shoes.</summary>
		Feet = 8,
		/// <summary>Left finger ring.</summary>
		LeftFinger = 9,
		/// <summary>Right hand or tail item.</summary>
		RightHandOrTail = 10
	}
}