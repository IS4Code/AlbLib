/* Date: 27.8.2014, Time: 10:44 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AlbLib.XLD;

namespace AlbLib
{
	public class XLDPathInfo : PathInfo
	{
		/// <summary>
		/// File name formatted with *.
		/// </summary>
		public string SearchPattern{
			get;private set;
		}
		
		/// <summary>
		/// Regex to match for a file.
		/// </summary>
		public Regex MatchPattern{
			get;private set;
		}
		
		public XLDPathInfo(string path) : base(path)
		{
			string file = Path.GetFileName(path);
			SearchPattern = String.Format(file, '*');
			string[] split = String.Format(file, "|").Split('|');
			MatchPattern = new Regex("^.*"+Regex.Escape(split[0])+@"(\d+)"+Regex.Escape(split[1])+"$");
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
		
		public override bool Match(string path)
		{
			return MatchPattern.Match(Path.GetFileName(path)).Success;
		}
		
		/// <summary>
		/// Enumerates through all file sequences.
		/// </summary>
		public IEnumerable<KeyValuePair<int,string>> EnumerateFiles()
		{
			return System.IO.Directory.EnumerateFiles(Directory, SearchPattern).Select(f => MatchPattern.Match(f)).Where(m => m.Success).Select(m => new KeyValuePair<int,string>(Int32.Parse(m.Groups[1].Value), m.Value)).OrderBy(p => p.Key);
		}
		
		public new XLDPathInfo WithDefaultLanguage()
		{
			return (XLDPathInfo)base.WithDefaultLanguage();
		}
		
		protected override PathInfo SpecifyImpl(params string[] args)
		{
			return Specify(args);
		}
		
		public new XLDPathInfo Specify(params string[] args)
		{
			return new XLDPathInfo(SpecifyFormat(args));
		}
	}
}
