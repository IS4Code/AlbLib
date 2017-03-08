/* Date: 28.8.2014, Time: 0:01 */
using System;
using System.Collections.Generic;

namespace AlbLib
{
	public class SimpleRepository<T> : Repository<T> where T : IGameResource
	{
		private readonly Func<int,T> getter;
		private readonly Func<IEnumerable<KeyValuePair<int,T>>> enumerator;
		
		public SimpleRepository(Func<int,T> getter, Func<IList<T>> all)
		{
			this.getter = getter;
			this.enumerator = ()=>AllEnumerator(all);
		}
		
		public SimpleRepository(Func<int,T> getter, Func<IEnumerable<KeyValuePair<int,T>>> enumerator)
		{
			this.getter = getter;
			this.enumerator = enumerator;
		}
		
		public override PathInfo Path{
			get{
				return null;
			}
		}
		
		protected override T GetEntry(int id)
		{
			return getter(id);
		}
		
		private IEnumerable<KeyValuePair<int, T>> AllEnumerator(Func<IList<T>> all)
		{
			IList<T> list = all();
			for(int i = 0; i < list.Count; i++)
			{
				yield return new KeyValuePair<int,T>(i+1,list[i]);
			}
		}
		
		protected override IEnumerable<KeyValuePair<int, T>> GetPairEnumerator()
		{
			return enumerator();
		}
	}
}
