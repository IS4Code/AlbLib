using System;

namespace AlbLib.INI
{
	public partial class GameConfig
	{
		[Serializable]
		public class VESAConfig
		{
			[INIPropertyName("MODE_1024x768x16.2M")]
			public bool Mode1024x768x16_2M{get;set;}
			[INIPropertyName("MODE_800x600x16.2M")]
			public bool Mode800x600x16_2M{get;set;}
			[INIPropertyName("MODE_640x480x16.2M")]
			public bool Mode640x480x16_2M{get;set;}
			[INIPropertyName("MODE_1024x768x64k")]
			public bool Mode1024x768x64k{get;set;}
			[INIPropertyName("MODE_800x600x64k")]
			public bool Mode800x600x64k{get;set;}
			[INIPropertyName("MODE_640x480x64k")]
			public bool Mode640x480x64k{get;set;}
			[INIPropertyName("MODE_1024x768x32k")]
			public bool Mode1024x768x32k{get;set;}
			[INIPropertyName("MODE_800x600x32k")]
			public bool Mode800x600x32k{get;set;}
			[INIPropertyName("MODE_640x480x32k")]
			public bool Mode640x480x32k{get;set;}
			[INIPropertyName("MODE_1024x768x256")]
			public bool Mode1024x768x256{get;set;}
			[INIPropertyName("MODE_800x600x256")]
			public bool Mode800x600x256{get;set;}
			[INIPropertyName("MODE_640x480x256")]
			public bool Mode640x480x256{get;set;}
			[INIPropertyName("MODE_640x400x256")]
			public bool Mode640x400x256{get;set;}
			[INIPropertyName("MEMORY")]
			public int Memory{get;set;}
			[INIPropertyName("VESA_VERSION_SUBNUMBER")]
			public int VesaVersionSubNumber{get;set;}
			[INIPropertyName("VESA_VERSION_NUMBER")]
			public int VesaVersionNumber{get;set;}
			[INIPropertyName("OEM")]
			public string OEM{get;set;}
		}
	}
}
