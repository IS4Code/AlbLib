using System;
using System.Collections.Generic;
#pragma warning disable 1591

namespace AlbLib.Caching
{
	/// <summary>
	/// This cache class uses two index parameters, main index and descriptive arguments.
	/// </summary>
	[Serializable]
	public class Cache<TElem, TIndex, TArgs> : Cache where TIndex : IEquatable<TIndex> where TArgs : IEquatable<TArgs>
	{
		/// <summary>
		/// This delegate is called if element is not found in a cache.
		/// </summary>
		[Serializable]
		public delegate TElem ReceiverDelegate(TIndex index, TArgs args);
		
		/// <summary>
		/// This delegate is called at the beginning.
		/// </summary>
		[Serializable]
		public delegate bool RepeaterDelegate(TIndex index, TArgs args, out TElem elem);
		
		/// <summary>
		/// This delegate is called if element is not found in a cache.
		/// </summary>
		protected ReceiverDelegate Receiver;
		
		/// <summary>
		/// This delegate is called at the beginning.
		/// </summary>
		protected RepeaterDelegate Repeater;
		
		/// <summary>
		/// List of elements in the cache.
		/// </summary>
		protected List<TElem> ElemList;
		
		/// <summary>
		/// List of indexes in the cache.
		/// </summary>
		protected List<TIndex> IndexList;
		
		/// <summary>
		/// List of arguments in the cache.
		/// </summary>
		protected List<TArgs> ArgList;
		
		/// <summary>
		/// Returns or creates element in the cache.
		/// </summary>
		public TElem this[TIndex index]
		{
			get{
				return Get(index, default(TArgs));
			}
		}
		
		/// <summary>
		/// Returns or creates element in the cache.
		/// </summary>
		public TElem this[TIndex index, TArgs args]
		{
			get{
				return Get(index, args);
			}
		}
		
		/// <summary>
		/// Returns or creates element in the cache.
		/// </summary>
		public TElem Get(TIndex index)
		{
			return Get(index, default(TArgs));
		}
		
		/// <summary>
		/// Returns or creates element in the cache.
		/// </summary>
		public virtual TElem Get(TIndex index, TArgs args)
		{
			TElem elem;
			if(Repeater != null && Repeater(index, args, out elem))return elem;
			for(int i = 0; i < Count; i++)
			{
				if(IndexList[i].Equals(index) && ArgList[i].Equals(args))
				{
					return ElemList[i];
				}
			}
			elem = Receiver(index, args);
			ElemList.Add(elem);
			IndexList.Add(index);
			ArgList.Add(args);
			Count += 1;
			return elem;
		}
		
		public Cache(ReceiverDelegate receiver, RepeaterDelegate repeater) : this(receiver)
		{
			Repeater = repeater;
		}
		
		public Cache(ReceiverDelegate receiver)
		{
			if(receiver == null)throw new ArgumentNullException("receiver");
			Receiver = receiver;
			ElemList = new List<TElem>();
			IndexList = new List<TIndex>();
			ArgList = new List<TArgs>();
		}
		
		protected Cache()
		{
			
		}
		
		/// <summary>
		/// Clears all cached data.
		/// </summary>
		public override void Clear()
		{
			ElemList.Clear();
			IndexList.Clear();
			ArgList.Clear();
			Count = 0;
		}
	}
}
