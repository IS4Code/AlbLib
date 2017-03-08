/* Date: 19.2.2017, Time: 13:38 */
using System;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Flags specifying a trigger for a map event.
	/// </summary>
	[Flags, Serializable]
	public enum EventTrigger : short
	{
		None = 0,
		Normal = 1,
		Examine = 2,
		Touch = 4,
		Speak = 8,
		UseItem = 16,
		MapInit = 32,
		EveryStep = 64,
		EveryHour = 128,
		EveryDay = 256,
		Default = 512,
		Action = 1024,
		NPC = 2048,
		Take = 4096,
	}
}
