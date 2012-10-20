using System;

namespace AlbLib.Caching
{
	/// <summary>
	/// Base class of cache types.
	/// </summary>
	public abstract class Cache
	{
		/// <summary>
		/// Clears all cached data.
		/// </summary>
		public abstract void Clear();
		
		/// <summary>
		/// Returns size of cache.
		/// </summary>
		public int Count{get;protected set;}
		
		/// <summary>
		/// Returns true if <paramref name="index"/> is zero or default.
		/// </summary>
		/// <param name="index">
		/// Index parameter.
		/// </param>
		/// <param name="elem">
		/// Return parameter.
		/// </param>
		/// <returns>
		/// True if <paramref name="index"/> is zero or default.
		/// </returns>
		public static bool ZeroNull<TElem, TIndex>(TIndex index, out TElem elem) where TIndex : IEquatable<TIndex>
		{
			elem = default(TElem);
			if(default(TIndex).Equals(index))
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// Returns true if <paramref name="index"/> is zero or default.
		/// </summary>
		/// <param name="index">
		/// Index parameter.
		/// </param>
		/// <param name="args">
		/// Doesn't matter.
		/// </param>
		/// <param name="elem">
		/// Return parameter.
		/// </param>
		/// <returns>
		/// True if <paramref name="index"/> is zero or default.
		/// </returns>
		public static bool ZeroNull<TElem, TIndex, TArgs>(TIndex index, TArgs args, out TElem elem) where TIndex : IEquatable<TIndex>
		{
			elem = default(TElem);
			if(default(TIndex).Equals(index))
				return true;
			else
				return false;
		}
	}
}
