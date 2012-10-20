using System;
namespace AlbLib.Scripting
{
	/// <summary>
	/// Delegate which handles scripts.
	/// </summary>
	[Serializable]
	public delegate bool ExecuteHandler(string script);
}