using System;
using System.IO;
using AlbLib.Texts;
using AlbLib.XLD;

namespace AlbLib.Scripting
{
	/// <summary>
	/// Core scripting class.
	/// </summary>
	public static class Scripts
	{
		/// <summary>
		/// Returns script text.
		/// </summary>
		/// <param name="index">
		/// Zero-based script index.
		/// </param>
		/// <returns>
		/// Script.
		/// </returns>
		public static string GetScript(int index)
		{
			int subindex = index%100;
			int fileindex = index/100;
			string file = String.Format(Paths.ScriptsN, fileindex);
			if(File.Exists(file))
			{
				using(FileStream stream = new FileStream(file, FileMode.Open))
				{
					XLDSubfile subfile = XLDFile.ReadSubfile(stream, subindex);
					if(subfile == null)return String.Empty;
					byte[] content = subfile.Data;
					return TextCore.DefaultEncoding.GetString(content);
				}
			}else{
				return String.Empty;
			}
		}
		
		/// <summary>
		/// Executes script.
		/// </summary>
		/// <param name="script">
		/// The script text to execute.
		/// </param>
		/// <param name="executor">
		/// Virtual machine which executes the script.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public static bool RunScript(string script, IScriptExecutor executor)
		{
			return executor.Execute(script);
		}
		
		/// <summary>
		/// Executes script.
		/// </summary>
		/// <param name="index">
		/// The script index to execute.
		/// </param>
		/// <param name="executor">
		/// Virtual machine which executes the script.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public static bool RunScript(int index, IScriptExecutor executor)
		{
			return executor.Execute(GetScript(index));
		}
		
		/// <summary>
		/// Executes script.
		/// </summary>
		/// <param name="script">
		/// The script text to execute.
		/// </param>
		/// <param name="handler">
		/// Delegate which is called.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public static bool RunScript(string script, ExecuteHandler handler)
		{
			return handler(script);
		}
		
		/// <summary>
		/// Executes script.
		/// </summary>
		/// <param name="index">
		/// The script index to execute.
		/// </param>
		/// <param name="handler">
		/// Delegate which is called.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public static bool RunScript(int index, ExecuteHandler handler)
		{
			return handler(GetScript(index));
		}
	}
}