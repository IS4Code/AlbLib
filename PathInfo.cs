using System;
using System.Collections.Generic;
using System.IO;
using AlbLib.XLD;

namespace AlbLib
{
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
		
		/// <summary>
		/// Opens XLD navigator at index 0.
		/// </summary>
		/// <returns>
		/// XLD navigator at start of data.
		/// </returns>
		public XLDNavigator OpenXLD()
		{
			return OpenXLD(0);
		}
		
		/// <summary>
		/// Opens XLD navigator at <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// Start index.
		/// </param>
		/// <returns>
		/// XLD navigator at start of data.
		/// </returns>
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
}
