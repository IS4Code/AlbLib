/* Date: 28.8.2014, Time: 0:01 */
using System;
using System.Collections.Generic;
using System.IO;
using AlbLib.XLD;

namespace AlbLib
{
	public class XLDRepository<T> : Repository<T> where T : IGameResource
	{
		private readonly Func<int,Stream,int,T> init;
		private readonly Func<XLDPathInfo> pathGet;
		
		public override PathInfo Path{get{return XLDPath;}}
		public XLDPathInfo XLDPath{get{return pathGet();}}
		
		public XLDRepository(XLDPathInfo path, Func<int,Stream,int,T> initfunc) : this(()=>path, initfunc)
		{
			
		}
		
		public XLDRepository(Func<XLDPathInfo> path, Func<int,Stream,int,T> initfunc)
		{
			pathGet = path;
			init = initfunc;
		}
		
		protected override T GetEntry(int id)
		{
			int fi,si;
			if(Common.E(id, out fi, out si))
			{
				using(XLDNavigator nav = this.XLDPath.OpenXLD(fi))
				{
					nav.GoToSubfile((short)si);
					if(nav.SubfileLength == 0) return default(T);
					return init(id,nav,nav.SubfileLength);
				}
			}else{
				return default(T);
			}
		}
		
		protected override IEnumerable<KeyValuePair<int,T>> GetPairEnumerator()
		{
			foreach(var pair in this.XLDPath.EnumerateFiles())
			{
				using(XLDNavigator nav = new XLDNavigator(pair.Value))
				{
					for(short i = 0; i < nav.NumSubfiles; i++)
					{
						nav.GoToSubfile(i);
						int id = Common.E(pair.Key, i);
						T value;
						if(nav.SubfileLength == 0)
						{
							value = default(T);
						}else{
							value = init(id,nav,nav.SubfileLength);
						}
						yield return new KeyValuePair<int,T>(id, value);
					}
				}
			}
		}
	}
}
