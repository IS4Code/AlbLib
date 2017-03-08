using System;

namespace AlbLib.Caching
{
	public struct RefEq<T> : IEquatable<RefEq<T>>, IEquatable<T> where T : class
	{
		readonly T value;
		
		public T Value{
			get{
				return value;
			}
		}
		
		public RefEq(T value)
		{
			this.value = value;
		}
		
		public bool Equals(RefEq<T> other)
		{
			return value.Equals(other.value);
		}
		
		public bool Equals(T other)
		{
			return value.Equals(other);
		}
		
		public static implicit operator RefEq<T>(T value)
		{
			return new RefEq<T>(value);
		}
		
		public static implicit operator T(RefEq<T> refeq)
		{
			return refeq.Value;
		}
	}
}
