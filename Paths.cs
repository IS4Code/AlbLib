using System;
using System.IO;

namespace AlbLib
{
	/// <summary>
	/// Contains all paths used in this library.
	/// </summary>
	public static class Paths
	{
		/// <summary>
		/// Absolute path to any XLDLIBS/PALETTE{0}.XLD.
		/// </summary>
		public static PathInfo PaletteN;
		
		/// <summary>
		/// Absolute path to XLDLIBS/PALETTE.000.
		/// </summary>
		public static PathInfo GlobalPalette;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/SCRIPT{0}.XLD.
		/// </summary>
		public static PathInfo ScriptsN;
		
		/// <summary>
		/// Absolute path to XLDLIBS/ITEMNAME.DAT.
		/// </summary>
		public static PathInfo ItemName;
		
		/// <summary>
		/// Absolute path to XLDLIBS/ITEMLIST.DAT.
		/// </summary>
		public static PathInfo ItemList;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/SAMPLES{0}.XLD.
		/// </summary>
		public static PathInfo SamplesN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/ICONGFX{0}.XLD.
		/// </summary>
		public static PathInfo IconGraphicsN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/ICONDAT{0}.XLD.
		/// </summary>
		public static PathInfo IconDataN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/MAPDATA{0}.XLD.
		/// </summary>
		public static PathInfo MapDataN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/TRANSTB{0}.XLD.
		/// </summary>
		public static PathInfo TransparencyTablesN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/LABDATA{0}.XLD.
		/// </summary>
		public static PathInfo LabDataN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/3DFLOOR{0}.XLD.
		/// </summary>
		public static PathInfo _3DFloorN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/3DWALLS{0}.XLD.
		/// </summary>
		public static PathInfo _3DWallsN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/3DOBJEC{0}.XLD.
		/// </summary>
		public static PathInfo _3DObjectsN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/3DOVERL{0}.XLD.
		/// </summary>
		public static PathInfo _3DOverlaysN;
		
		/// <summary>
		/// Absolute path to any XLDLIBS/AUTOGFX{0}.XLD.
		/// </summary>
		public static PathInfo AutoGFXN;
		
		/// <summary>
		/// Absolute path to MAIN.EXE.
		/// </summary>
		public static PathInfo Main;
		
		/// <summary>
		/// Sets all paths.
		/// </summary>
		/// <param name="path">Absolute path to XLDLIBS folder.</param>
		public static void SetXLDLIBS(string path)
		{
			PaletteN = Path.Combine(path, "PALETTE{0}.XLD");
			GlobalPalette = Path.Combine(path, "PALETTE.000");
			ScriptsN = Path.Combine(path, "SCRIPT{0}.XLD");
			ItemName =  Path.Combine(path, "ITEMNAME.DAT");
			ItemList =  Path.Combine(path, "ITEMLIST.DAT");
			SamplesN =  Path.Combine(path, "SAMPLES{0}.XLD");
			IconGraphicsN =  Path.Combine(path, "ICONGFX{0}.XLD");
			IconDataN =  Path.Combine(path, "ICONDAT{0}.XLD");
			LabDataN = Path.Combine(path, "LABDATA{0}.XLD");
			MapDataN =  Path.Combine(path, "MAPDATA{0}.XLD");
			_3DFloorN = Path.Combine(path, "3DFLOOR{0}.XLD");
			_3DWallsN = Path.Combine(path, "3DWALLS{0}.XLD");
			_3DObjectsN = Path.Combine(path, "3DOBJEC{0}.XLD");
			_3DOverlaysN = Path.Combine(path, "3DOVERL{0}.XLD");
			AutoGFXN = Path.Combine(path, "AUTOGFX{0}.XLD");
			TransparencyTablesN =  Path.Combine(path, "TRANSTB{0}.XLD");
		}
	}
}
