using System;

namespace AlbLib.INI
{
	public partial class GameConfig
	{
		[Serializable]
		public class AlbionConfig
		{
			[INIPropertyName("MIDI_VOLUME")]
			public int MIDIVolume{get;set;}
			[INIPropertyName("DIGI_VOLUME")]
			public int DigiVolume {get;set;}
			[INIPropertyName("3D_WINDOW_SIZE")]
			public int _3DWindowSize{get;set;}
			[INIPropertyName("COMBAT_DETAIL_LEVEL")]
			public int CombatDetailLevel{get;set;}
			[INIPropertyName("COMBAT_MESSAGE_SPEED")]
			public int CombatMessageSpeed{get;set;}
			[INIPropertyName("SAVED_GAME_NR")]
			public int SavedGameNR{get;set;}
		}
	}
}
