using System;
using System.IO;
using System.Linq;

namespace AlbLib.XLD
{
	/// <summary>
	/// This class represent single subfile in a XLD.
	/// </summary>
	[Serializable]
	public class XLDSubfile : IGameResource
	{
		/// <summary>
		/// Subfile contents.
		/// </summary>
		public byte[] Data{get;set;}
		
		/// <summary>
		/// Contents length.
		/// </summary>
		public int Length{
			get{
				if(Data == null)return 0;
				return Data.Length;
			}
		}
		
		/// <summary>
		/// Assigned index.
		/// </summary>
		public short Index{get;private set;}
		
		public XLDSubfile(byte[] data, short index)
		{
			Data = data;
			Index = index;
		}
		
		public XLDSubfile(short index)
		{
			Index = index;
		}
		
		/// <param name="stream">
		/// Source stream.
		/// </param>
		/// <param name="length">
		/// Contents length.
		/// </param>
		public XLDSubfile(Stream stream, int length) : this(stream, length, -1)
		{
			
		}
		
		/// <param name="stream">
		/// Source stream.
		/// </param>
		/// <param name="length">
		/// Contents length.
		/// </param>
		/// <param name="index">
		/// Assigned index.
		/// </param>
		public XLDSubfile(Stream stream, int length, short index)
		{
			Data = new byte[length];
			stream.Read(Data, 0, length);
			Index = index;
		}
		
		/// <summary>
		/// Returns memory stream containg subfile contents.
		/// </summary>
		public MemoryStream GetInputStream()
		{
			return new MemoryStream(Data, false);
		}
		
		public int Save(Stream output)
		{
			output.Write(Data, 0, Data.Length);
			return Data.Length;
		}
		
		public bool Equals(IGameResource obj)
		{
			return Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			if(obj is XLDSubfile)
			{
				return ((XLDSubfile)obj).Data.SequenceEqual(this.Data);
			}
			return false;
		}
		
		public override int GetHashCode()
		{
			return Data.GetHashCode();
		}
		
		public static bool operator ==(XLDSubfile lhs, XLDSubfile rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(XLDSubfile lhs, XLDSubfile rhs)
		{
			return !(lhs == rhs);
		}

	}
}