using System;
namespace AlbLib.Mapping
{
	/// <summary>
	/// Type of map.
	/// </summary>
	[Serializable]
	public enum MapType : byte
	{
		/// <summary>
		/// Undefined type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// 3D.
		/// </summary>
		Map3D = 1,
		/// <summary>
		/// 2D.
		/// </summary>
		Map2D = 2
	}
}