using System.IO;
namespace AlbLib.Mapping
{
	/// <summary>
	/// Map tile.
	/// </summary>
	public struct Tile
	{
		/// <summary>
		/// Tile X position.
		/// </summary>
		public readonly byte X;
		
		/// <summary>
		/// Tile Y position.
		/// </summary>
		public readonly byte Y;
		
		/// <summary>
		/// Underlay tile id.
		/// </summary>
		public short Underlay{
			get;set;
		}
		
		/// <summary>
		/// Overlay tile id.
		/// </summary>
		public short Overlay{
			get;set;
		}
		
		/// <summary>
		/// Reads a map tile.
		/// </summary>
		public Tile(byte data1, byte data2, byte data3) : this()
		{
			Overlay = (short)((data1<<4)|((data2&0xF0)>>4));
			Underlay = (short)(data3|((data2&0x0F)<<8));
		}
		
		/// <summary>
		/// Reads a map tile.
		/// </summary>
		public Tile(Stream source) : this((byte)source.ReadByte(), (byte)source.ReadByte(), (byte)source.ReadByte())
		{
			
		}
		
		/// <summary>
		/// Reads a map tile.
		/// </summary>
		public Tile(byte x, byte y, byte data1, byte data2, byte data3) : this()
		{
			X = x;
			Y = y;
			Overlay = (short)((data1<<4)|((data2&0xF0)>>4));
			Underlay = (short)(data3|((data2&0x0F)<<8));
		}
		
		/// <summary>
		/// Reads a map tile.
		/// </summary>
		public Tile(byte x, byte y, Stream source) : this(x, y, (byte)source.ReadByte(), (byte)source.ReadByte(), (byte)source.ReadByte())
		{
			
		}
	}
}