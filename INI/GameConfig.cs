using System;
using System.Reflection;

namespace AlbLib.INI
{
	[Serializable]
	public partial class GameConfig : Config
	{
		[INIPropertyName("SYSTEM")]
		public SystemConfig System{get;set;}
		[INIPropertyName("VESA")]
		public VESAConfig VESA{get;set;}
		[INIPropertyName("ALBION")]
		public AlbionConfig Albion{get;set;}
		
		public static GameConfig Default{
			get{
				return new GameConfig(Paths.Setup);
			}
		}
		
		public GameConfig()
		{
			System = new SystemConfig();
			VESA = new VESAConfig();
			Albion = new AlbionConfig();
		}
		
		public GameConfig(string file) : base(file)
		{}
	}
}
