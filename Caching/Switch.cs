using System;

namespace AlbLib.Caching
{
	public partial class IndexedCache<TElem, TArgs> : Cache<TElem, int, TArgs> where TArgs : IEquatable<TArgs>
	{
		protected struct Switch
		{
			public readonly bool Set;
			public readonly TElem Value;
			public readonly TArgs Args;
			
			public Switch(TElem value) : this()
			{
				Set = true;
				Value = value;
			}
			
			public Switch(TElem value, TArgs args) : this()
			{
				Set = true;
				Value = value;
				Args = args;
			}
		}
	}
}
