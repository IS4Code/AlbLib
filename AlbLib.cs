using System;
using System.IO;
using System.Collections.Generic;
using AlbLib.XLD;

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
	
	/// <summary>
	/// Informations about path to Albion resource.
	/// </summary>
	public class PathInfo
	{
		/// <summary>
		/// Absolute file path.
		/// </summary>
		public string FileName{
			get;set;
		}
		
		/// <summary>
		/// Directory containing resource.
		/// </summary>
		public string Directory{
			get;set;
		}
		
		/// <summary>
		/// File name formatted with *.
		/// </summary>
		public string SearchPattern{
			get;set;
		}
		
		/// <summary></summary>
		public PathInfo(string path)
		{
			FileName = Path.GetFullPath(path);
			Directory = Path.GetDirectoryName(FileName);
			SearchPattern = String.Format(Path.GetFileName(path), '*');
		}
		
		public XLDNavigator OpenXLD()
		{
			return OpenXLD(0);
		}
		
		public XLDNavigator OpenXLD(int index)
		{
			return new XLDNavigator(String.Format(FileName, index));
		}
		
		/// <summary>
		/// Enumerates through all file sequences.
		/// </summary>
		public IEnumerable<string> EnumerateList()
		{
			return EnumerateList(0);
		}
		
		/// <summary>
		/// Enumerates through all file sequences.
		/// </summary>
		public IEnumerable<string> EnumerateList(int start)
		{
			int i = start;
			string path;
			while(File.Exists(path = String.Format(this.FileName, i)))
			{
				yield return path;
				i += 1;
			}
		}
		
		/// <summary>
		/// Enumerates through all file sequences.
		/// </summary>
		public IEnumerable<KeyValuePair<int,string>> EnumeratePairList()
		{
			return EnumeratePairList(0);
		}
		
		/// <summary>
		/// Enumerates through all file sequences.
		/// </summary>
		public IEnumerable<KeyValuePair<int,string>> EnumeratePairList(int start)
		{
			int i = start;
			string path;
			while(File.Exists(path = String.Format(this.FileName, i)))
			{
				yield return new KeyValuePair<int,string>(i, path);
				i += 1;
			}
		}
		
		/// <summary>
		/// Enumerates through all file and subfile sequences.
		/// </summary>
		public IEnumerable<KeyValuePair<int,XLDSubfile>> EnumerateAllSubfiles()
		{
			return EnumerateAllSubfiles(0);
		}
		
		/// <summary>
		/// Enumerates through all file and subfile sequences.
		/// </summary>
		public IEnumerable<KeyValuePair<int,XLDSubfile>> EnumerateAllSubfiles(int start)
		{
			int i = start;
			string path;
			while(File.Exists(path = String.Format(this.FileName, i)))
			{
				foreach(XLDSubfile sub in XLDFile.EnumerateSubfiles(path))
				{
					yield return new KeyValuePair<int,XLDSubfile>(Common.E(i, sub.Index), sub);
				}
				i += 1;
			}
		}
		
		/// <summary>
		/// Formats variable path.
		/// </summary>
		public string Format(params object[] args)
		{
			return String.Format(this.FileName, args);
		}
		
		/// <summary>
		/// Formats variable path.
		/// </summary>
		public string Format(int arg1)
		{
			return String.Format(this.FileName, arg1);
		}
		
		/// <summary></summary>
		public static implicit operator PathInfo(string path)
		{
			return new PathInfo(path);
		}
		
		/// <summary></summary>
		public static implicit operator String(PathInfo info)
		{
			return info.ToString();
		}
		
		/// <summary></summary>
		public override string ToString()
		{
			return FileName;
		}
	}
	
	/// <summary>
	/// Contains various common functions and magic contants.
	/// </summary>
	public static class Common
	{
		/// <summary>
		/// 63×ColorConversion = 255
		/// </summary>
		public const double ColorConversion = 4.047619047619047619047619047619;
		
		public static bool E(int index, out int fileIndex, out int subfileIndex)
		{
			if(index == 0)
			{
				fileIndex = 0; subfileIndex = 0;
				return false;
			}
			fileIndex = index/100;
			subfileIndex = index<100?index-1:index%100;
			return true;
		}
		
		public static int E(int fileIndex, int subfileIndex)
		{
			return fileIndex==0?subfileIndex+1:fileIndex*100+subfileIndex;
		}
		
		private static readonly byte[] skipBuffer = new byte[4096];
		
		public static int Skip(this Stream input, int bytes)
		{
			int read = 0;
			do{
				if(bytes > 4096)
				{
					read += input.Read(skipBuffer, 0, 4096);
					bytes -= 4096;
				}else{
					read += input.Read(skipBuffer, 0, bytes);
					bytes = 0;
				}
			}while(bytes != 0);
			return read;
		}
	}
}