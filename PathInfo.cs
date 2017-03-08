using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlbLib.Texts;
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
			get;private set;
		}
		
		/// <summary>
		/// Directory containing resource.
		/// </summary>
		public string Directory{
			get;private set;
		}
		
		/// <summary></summary>
		public PathInfo(string path)
		{
			FileName = Path.GetFullPath(path);
			Directory = Path.GetDirectoryName(FileName);
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
		
		public virtual bool Match(string path)
		{
			return Path.GetFileName(path) == Path.GetFileName(FileName);
		}
		
		public PathInfo WithDefaultLanguage()
		{
			return Specify(TextCore.DefaultLanguageFolder);
		}
		
		protected string SpecifyFormat(params string[] args)
		{
			return Format((new[]{"{0}"}).Concat(args).ToArray());
		}
		
		protected virtual PathInfo SpecifyImpl(params string[] args)
		{
			return new PathInfo(SpecifyFormat(args));
		}
		
		public PathInfo Specify(params string[] args)
		{
			return SpecifyImpl(args);
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
