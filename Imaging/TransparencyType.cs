using System;
namespace AlbLib.Imaging
{
	/// <summary>
	/// Type of transparency.
	/// </summary>
	[Serializable]
	public enum TransparencyType
	{
		/// <summary>
		/// No transparency.
		/// </summary>
		None = 0,
		/// <summary>
		/// Classic blending.
		/// </summary>
		Dark = 1,
		/// <summary>
		/// No blending, only major color.
		/// </summary>
		Main = 2,
		/// <summary>
		/// Glowing blending.
		/// </summary>
		Light = 3
	}
}