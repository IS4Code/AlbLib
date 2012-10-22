using System;

namespace AlbLib.INI
{
	public partial class GameConfig
	{
		[Serializable]
		public class SystemConfig
		{
			[INIPropertyName("VESA")]
			public bool VESA{get;set;}
			[INIPropertyName("FPU")]
			public bool FPU{get;set;}
			[INIPropertyName("SOURCE_PATH")]
			public string SourcePath{get;set;}
			[INIPropertyName("LANGUAGE")]
			public int Language{get;set;}
		}
	}
}
