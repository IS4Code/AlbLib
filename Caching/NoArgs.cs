using System;
#pragma warning disable 1591

namespace AlbLib.Caching
{
	/// <summary>
	/// This structure is used if a cache has no descriptive arguments.
	/// </summary>
	[Serializable]
	public struct NoArgs : IEquatable<NoArgs>
	{
		public override bool Equals(object obj)
		{
			return obj is NoArgs;
		}
		
		public bool Equals(NoArgs obj)
		{
			return true;
		}
		
		public static bool operator ==(NoArgs a, NoArgs b)
		{
			return true;
		}
		
		public static bool operator !=(NoArgs a, NoArgs b)
		{
			return false;
		}
		
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
