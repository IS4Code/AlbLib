/* Date: 28.8.2014, Time: 0:01 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlbLib
{
	public class ArrayXLDRepository<T> : XLDRepository<ArrayXLDRepository<T>.ArrayResource> where T : IGameResource
	{
		public ArrayXLDRepository(Func<XLDPathInfo> path, Func<int,Stream,int,T[]> initfunc) : base(path, (i,s,l)=>new ArrayResource(initfunc(i,s,l)))
		{
			
		}
		
		public new T[] Open(int id)
		{
			return base.Open(id).Value;
		}
		
		public new IEnumerable<KeyValuePair<int,T[]>> IndexEnumerate()
		{
			return base.IndexEnumerate().Select(p => new KeyValuePair<int,T[]>(p.Key, p.Value.Value));
		}
		
		public class ArrayResource : IList<T>, IGameResource
		{
			public T[] Value{get;private set;}
			
			public ArrayResource(T[] value)
			{
				Value = value;
			}
			
			public int Save(Stream output)
			{
				int count = 0;
				foreach(T elem in Value)
				{
					count += elem.Save(output);
				}
				return count;
			}
			
			T IList<T>.this[int index]
			{
				get {
					return Value[index];
				}
				set {
					Value[index] = value;
				}
			}
			
			int ICollection<T>.Count{
				get{
					return Value.Length;
				}
			}
			
			bool ICollection<T>.IsReadOnly{
				get{
					return Value.IsReadOnly;
				}
			}
			
			int IList<T>.IndexOf(T item)
			{
				return ((IList<T>)Value).IndexOf(item);
			}
			
			void IList<T>.Insert(int index, T item)
			{
				((IList<T>)Value).Insert(index, item);
			}
			
			void IList<T>.RemoveAt(int index)
			{
				((IList<T>)Value).RemoveAt(index);
			}
			
			void ICollection<T>.Add(T item)
			{
				((ICollection<T>)Value).Add(item);
			}
			
			void ICollection<T>.Clear()
			{
				((ICollection<T>)Value).Clear();
			}
			
			bool ICollection<T>.Contains(T item)
			{
				return ((ICollection<T>)Value).Contains(item);
			}
			
			void ICollection<T>.CopyTo(T[] array, int arrayIndex)
			{
				((ICollection<T>)Value).CopyTo(array, arrayIndex);
			}
			
			bool ICollection<T>.Remove(T item)
			{
				return ((ICollection<T>)Value).Remove(item);
			}
			
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return ((IEnumerable<T>)Value).GetEnumerator();
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return Value.GetEnumerator();
			}
			
			#region Equals and GetHashCode implementation
			public override bool Equals(object obj)
			{
				ArrayResource other = obj as ArrayResource;
				if (other == null)
					return false;
				return object.Equals(this.Value, other.Value);
			}
			
			public override int GetHashCode()
			{
				return Value.GetHashCode();
			}
			
			public static bool operator ==(ArrayResource lhs, ArrayResource rhs)
			{
				if (ReferenceEquals(lhs, rhs))
					return true;
				if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
					return false;
				return lhs.Equals(rhs);
			}
			
			public static bool operator !=(ArrayResource lhs, ArrayResource rhs)
			{
				return !(lhs == rhs);
			}
			#endregion
		}
	}
}
