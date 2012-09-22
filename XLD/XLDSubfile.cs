using System.IO;
namespace AlbLib.XLD
{
	/// <summary>
	/// This class represent single subfile in a XLD.
	/// </summary>
	public class XLDSubfile
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
	}
}