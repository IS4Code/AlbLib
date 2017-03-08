/* Date: 25.3.2015, Time: 19:32 */
using System;
using System.Collections;

namespace AlbLib
{
	public interface IRepository : IEnumerable
	{
		PathInfo Path{get;}
		Type DataType{get;}
		object Open(object id);
		object Open(int id);
	}
}
