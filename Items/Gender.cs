using System;
namespace AlbLib.Items
{
	/// <summary>
	/// Item gender limitation.
	/// </summary>
	[Flags, Serializable]
	public enum Gender : byte
	{
		/// <summary>Nobody can use this.</summary>
		None = 0,
		/// <summary>Only for males.</summary>
		Male = 1,
		/// <summary>Only for females.</summary>
		Female = 2,
		/// <summary>For any gender.</summary>
		Any = 3
	}
}