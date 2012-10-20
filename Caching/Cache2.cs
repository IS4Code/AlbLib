using System;
#pragma warning disable 1591

namespace AlbLib.Caching
{
	/// <summary>
	/// This cache class uses one index parameter.
	/// </summary>
	[Serializable]
	public class Cache<TElem, TIndex> : Cache<TElem, TIndex, NoArgs> where TIndex : IEquatable<TIndex>
	{
		/// <summary>
		/// This delegate is called if element is not found in a cache.
		/// </summary>
		[Serializable]
		public delegate TElem ReceiverDelegate2(TIndex index);
		
		/// <summary>
		/// This delegate is called at the beginning.
		/// </summary>
		[Serializable]
		public delegate bool RepeaterDelegate2(TIndex index, out TElem elem);
		
		public Cache(ReceiverDelegate2 receiver, RepeaterDelegate2 repeater) : base((index, args)=>receiver(index), (TIndex index, NoArgs args, out TElem elem)=>repeater(index, out elem))
		{}
		
		public Cache(ReceiverDelegate2 receiver) : base((index, args)=>receiver(index))
		{}
		
		public Cache(ReceiverDelegate receiver, RepeaterDelegate repeater) : base(receiver, repeater)
		{}
		
		public Cache(ReceiverDelegate receiver) : base(receiver)
		{}
	}
}
