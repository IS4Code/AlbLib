using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AlbLib
{
	/// <summary>
	/// Contains all paths used in this library.
	/// </summary>
	public static class Paths
	{
		[Path("PALETTE{0}.XLD")]
		public static XLDPathInfo PaletteN;
		
		[Path("PALETTE.000")]
		public static PathInfo GlobalPalette;
		
		[Path("SCRIPT{0}.XLD")]
		public static XLDPathInfo ScriptsN;
		
		[Path("ITEMNAME.DAT")]
		public static PathInfo ItemName;
		
		[Path("ITEMLIST.DAT")]
		public static PathInfo ItemList;
		
		[Path("SAMPLES{0}.XLD")]
		public static XLDPathInfo Samples;
		
		[Path("ICONGFX{0}.XLD")]
		public static XLDPathInfo IconGraphics;
		
		[Path("ICONDAT{0}.XLD")]
		public static XLDPathInfo IconData;
		
		[Path("MAPDATA{0}.XLD")]
		public static XLDPathInfo MapData;
		
		[Path("TRANSTB{0}.XLD")]
		public static XLDPathInfo TransparencyTables;
		
		[Path("LABDATA{0}.XLD")]
		public static XLDPathInfo LabData;
		
		[Path("3DFLOOR{0}.XLD")]
		public static XLDPathInfo Floors3D;
		
		[Path("3DWALLS{0}.XLD")]
		public static XLDPathInfo Walls3D;
		
		[Path("3DOBJEC{0}.XLD")]
		public static XLDPathInfo Objects3D;
		
		[Path("3DOVERL{0}.XLD")]
		public static XLDPathInfo Overlays3D;
		
		[Path("3DBCKGR{0}.XLD")]
		public static XLDPathInfo Backgrounds3D;
		
		[Path("AUTOGFX{0}.XLD")]
		public static XLDPathInfo AutomapGraphics;
		
		[Path("NPCGR{0}.XLD")]
		public static XLDPathInfo NPCBig;
		
		[Path("NPCKL{0}.XLD")]
		public static XLDPathInfo NPCSmall;
		
		[Path("PARTGR{0}.XLD")]
		public static XLDPathInfo PartyBig;
		
		[Path("PARTKL{0}.XLD")]
		public static XLDPathInfo PartySmall;
		
		[Path("PICTURE{0}.XLD")]
		public static XLDPathInfo Pictures;
		
		[Path("FBODPIX{0}.XLD")]
		public static XLDPathInfo BodyImages;
		
		[Path("COMBACK{0}.XLD")]
		public static XLDPathInfo CombatBackgrounds;

        [Path("COMGFX{0}.XLD")]
        public static XLDPathInfo CombatGraphics;

        [Path("EVNTSET{0}.XLD")]
        public static XLDPathInfo EventSets;

        [Path("{1}/EVNTTXT{0}.XLD")]
		public static XLDPathInfo EventTexts;
		
		[Path("FLICS{0}.XLD")]
		[Path("{1}/FLICS{0}.XLD")]
		public static XLDPathInfo Flics;
		
		[Path("{1}/MAPTEXT{0}.XLD")]
		public static XLDPathInfo MapTexts;
		
		[Path("{1}/SYSTEXTS")]
		public static PathInfo SystemTexts;
		
		[Path("{1}/WORDLIS{0}.XLD")]
		public static XLDPathInfo WordLists;

        [Path("INITIAL/NPCCHAR{0}.XLD")]
        public static XLDPathInfo NPCCharacters;

        [Path("INITIAL/PRTCHAR{0}.XLD")]
        public static XLDPathInfo PartyCharacters;

        [Path("MONCHAR{0}.XLD")]
		public static XLDPathInfo MonsterChars;
		
		[Path("WAVELIB{0}.XLD")]
		public static XLDPathInfo WaveLibs;
		
		[Path("MAIN.EXE")]
		public static string Main;
		
		[Path("SETUP.INI")]
		public static string Setup;
		
		[Path("XLDLIBS")]
		public static string XLDLibs;
		
		/// <summary>
		/// Sets all XLD paths.
		/// </summary>
		/// <param name="path">
		/// Absolute path to XLDLIBS folder.
		/// </param>
		public static void SetXLDLIBS(string path)
		{
			foreach(FieldInfo fi in typeof(Paths).GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				if(typeof(PathInfo).IsAssignableFrom(fi.FieldType))
				{
					PathAttribute[] paths = (PathAttribute[])fi.GetCustomAttributes(typeof(PathAttribute), true);
					foreach(var attr in paths)
					{
						string fullpath = Path.Combine(path, attr.Path);
						/*if(TestPath(fullpath))
						{
							if(fi.FieldType == typeof(XLDPathInfo))
							{
								fi.SetValue(null, new XLDPathInfo(fullpath));
							}else{
								fi.SetValue(null, new PathInfo(fullpath));
							}
							break;
						}else{
							continue;
						}*/
						if(fi.FieldType == typeof(XLDPathInfo))
						{
							fi.SetValue(null, new XLDPathInfo(fullpath));
						}else{
							fi.SetValue(null, new PathInfo(fullpath));
						}
					}
				}
			}
		}
		
		private static bool TestPath(string path)
		{
			if(File.Exists(path)) return true;
			path = String.Format(path, "*", "ENGLISH", "INITIAL");
			string file = Path.GetFileName(path);
			string dir = Path.GetDirectoryName(path);
			try{
				return Directory.EnumerateFiles(dir, file).Any();
			}catch(DirectoryNotFoundException)
			{
				return false;
			}
		}
		
		/// <summary>
		/// Sets all paths.
		/// </summary>
		/// <param name="path">
		/// Absolute path to game root folder.
		/// </param>
		public static void SetGameDir(string path)
		{
			Main = Path.Combine(path, "MAIN.EXE");
			Setup = Path.Combine(path, "SETUP.INI");
			XLDLibs = Path.Combine(path, "XLDLIBS");
			SetXLDLIBS(XLDLibs);
		}
		
		[AttributeUsage(AttributeTargets.Field, AllowMultiple=true)]
		private class PathAttribute : Attribute
		{
			public string Path{get;private set;}
			
			public PathAttribute(string path)
			{
				Path = path;
			}
		}
	}
}
