/* Date: 19.2.2017, Time: 13:29 */
using System;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Type of the event.
	/// </summary>
	[Serializable]
	public enum EventType : byte
	{
		/// <summary>
		/// Unused.
		/// </summary>
		Script = 0,
		MapExit = 1,
		Door = 2,
		Chest = 3,
		Text = 4,
		Spinner = 5,
		Trap = 6,
		ChangeUsedItem = 7,
		DataChange = 8,
		ChangeIcon = 9,
		Encounter = 10,
		PlaceAction = 11,
		Query = 12,
		Modify = 13,
		Action = 14,
		Signal = 15,
		CloneAutomap = 16,
		Sound = 17,
		StartDialogue = 18,
		CreateTransport = 19,
		Execute = 20,
		RemovePartyMember = 21,
		EndDialogue = 22,
		Wipe = 23,
		PlayAnimation = 24,
		Offset = 25,
		Pause = 26,
		SimpleChest = 27,
		AskSurrender = 28,
		DoScript = 29,
	}
}
