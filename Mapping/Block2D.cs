using System;
using System.IO;

namespace AlbLib.Mapping
{
	/// <summary>
	/// BLKLIST.
	/// </summary>
	public class Block2D
	{
		/// <summary>
		/// Width in tiles.
		/// </summary>
		public byte Width{
			get{
				return (byte)Data.GetLength(0);
			}
		}
		
		/// <summary>
		/// Height in tiles.
		/// </summary>
		public byte Height{
			get{
				return (byte)Data.GetLength(1);
			}
		}
		
		/// <param name="stream">
		/// Source stream.
		/// </param>
		public Block2D(Stream stream)
		{
			byte width = (byte)stream.ReadByte();
			byte height = (byte)stream.ReadByte();
			data = new Tile[width,height];
			for(byte y = 0; y < height; y++)
			for(byte x = 0; x < width; x++)
			{
				data[x,y] = new Tile(x, y, stream);
			}
		}
		
		private Tile[,] data;
		
		/// <summary>
		/// Block of tiles.
		/// </summary>
		public Tile[,] Data{
			get{
				return data;
			}
			set{
				if(value.GetLength(0) > Byte.MaxValue || value.GetLength(1) > Byte.MaxValue)throw new ArgumentException(null,"value");
				data = value;
			}
		}
	}
}