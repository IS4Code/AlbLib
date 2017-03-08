/* Date: 28.8.2014, Time: 0:28 */
using System;
using System.IO;
using AlbLib.Texts;

namespace AlbLib
{
	public abstract class StringRepository : Repository<StringRepository.StringResource>
	{
		protected abstract string OpenImpl(int id);
		
		protected override sealed StringResource GetEntry(int id)
		{
			return new StringResource(OpenImpl(id));
		}
		
		public class StringResource : IGameResource
		{
			public string Value{get; private set;}
			
			public StringResource(string value)
			{
				Value = value;
			}
			
			public int Save(Stream output)
			{
				byte[] buffer = TextCore.DefaultEncoding.GetBytes(Value);
				output.Write(buffer, 0, buffer.Length);
				return buffer.Length;
			}
			
			public override string ToString()
			{
				return Value;
			}
			
			#region Equals and GetHashCode implementation
			public override bool Equals(object obj)
			{
				StringRepository.StringResource other = obj as StringRepository.StringResource;
				if (other == null)
					return false;
				return this.Value == other.Value;
			}
			
			public override int GetHashCode()
			{
				return Value.GetHashCode();
			}
			
			public static bool operator ==(StringRepository.StringResource lhs, StringRepository.StringResource rhs)
			{
				if (ReferenceEquals(lhs, rhs))
					return true;
				if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
					return false;
				return lhs.Equals(rhs);
			}
			
			public static bool operator !=(StringRepository.StringResource lhs, StringRepository.StringResource rhs)
			{
				return !(lhs == rhs);
			}
			#endregion
		}
	}
}
