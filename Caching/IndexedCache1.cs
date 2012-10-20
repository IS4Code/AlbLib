using System;

namespace AlbLib.Caching
{
	public class IndexedCache<TElem> : IndexedCache<TElem, NoArgs>
	{
		/// <summary>
		/// This delegate is called if element is not found in a cache.
		/// </summary>
		[Serializable]
		public delegate TElem ReceiverDelegate2(int index);
		
		/// <summary>
		/// This delegate is called at the beginning.
		/// </summary>
		[Serializable]
		public delegate bool RepeaterDelegate2(int index, out TElem elem);
		
		public IndexedCache(ReceiverDelegate2 receiver, RepeaterDelegate2 repeater) : base((index, args)=>receiver(index), (int index, NoArgs args, out TElem elem)=>repeater(index, out elem))
		{}
		
		public IndexedCache(ReceiverDelegate2 receiver) : base((index, args)=>receiver(index))
		{}
		
		public IndexedCache(ReceiverDelegate receiver, RepeaterDelegate repeater) : base(receiver, repeater)
		{}
		
		public IndexedCache(ReceiverDelegate receiver) : base(receiver)
		{}
	}
}
