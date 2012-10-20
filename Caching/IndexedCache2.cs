using System;

namespace AlbLib.Caching
{
	public partial class IndexedCache<TElem, TArgs> : Cache<TElem, int, TArgs> where TArgs : IEquatable<TArgs>
	{
		protected Switch[] ElemArray;
		
		public IndexedCache(ReceiverDelegate receiver, RepeaterDelegate repeater) : this(receiver)
		{
			Repeater = repeater;
		}
		
		public IndexedCache(ReceiverDelegate receiver) : base()
		{
			if(receiver == null)throw new ArgumentNullException("receiver");
			Receiver = receiver;
			ElemArray = new Switch[2];
		}
		
		public override TElem Get(int index, TArgs args)
		{
			TElem elem;
			if(Repeater != null && Repeater(index, args, out elem))return elem;
			if(index >= ElemArray.Length)
			{
				int newsize = ElemArray.Length;
				while(newsize < index)newsize*=newsize;
				Array.Resize(ref ElemArray, newsize);
			}
			if(!ElemArray[index].Set || !ElemArray[index].Args.Equals(args))
			{
				ElemArray[index] = new Switch(Receiver(index, args), args);
			}
			return ElemArray[index].Value;
		}
		
		public override void Clear()
		{
			ElemArray = new Switch[2];
			Count = 0;
		}
	}
}
