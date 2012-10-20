using System;
using System.IO;

namespace AlbLib.Mapping
{
	/// <summary>
	/// 3D map block.
	/// </summary>
	[Serializable]
	public struct Block : IMapSquare
	{
		/// <summary>
		/// Tile X position.
		/// </summary>
		public readonly byte X;
		
		/// <summary>
		/// Tile Y position.
		/// </summary>
		public readonly byte Y;
		
		public EventHeader Event{
			get;internal set;
		}
		
		public GotoPoint GotoPoint{
			get;internal set;
		}
		
		public byte Object{
			get;set;
		}
		
		public byte Floor{
			get;set;
		}
		
		public byte Ceiling{
			get;set;
		}
		
		public byte Wall{
			get{
				return (byte)(Object-100);
			}
			set{
				Object = (byte)(value+100);
			}
		}
		
		public bool IsWall{
			get{return Object>=101;}
		}
		
		public bool IsObject{
			get{return 1<=Object&&Object<=100;}
		}
		
		public bool IsEmpty{
			get{return Object==0||Object==101;}
		}
		
		public bool IsSpace{
			get{return Object==0;}
		}
		
		byte IMapSquare.X{
			get{return X;}
		}
		byte IMapSquare.Y{
			get{return Y;}
		}
		MapSquareType IMapSquare.Type{
			get{return MapSquareType.Block;}
		}
		
		/// <summary>
		/// Reads a map block.
		/// </summary>
		public Block(byte @object, byte floor, byte ceiling) : this()
		{
			Object = @object;
			Floor = floor;
			Ceiling = ceiling;
		}
		
		/// <summary>
		/// Reads a map block.
		/// </summary>
		public Block(Stream source) : this((byte)source.ReadByte(), (byte)source.ReadByte(), (byte)source.ReadByte())
		{
			
		}
		
		/// <summary>
		/// Reads a map block.
		/// </summary>
		public Block(byte x, byte y, byte @object, byte floor, byte ceiling) : this()
		{
			X = x;
			Y = y;
			Object = @object;
			Floor = floor;
			Ceiling = ceiling;
		}
		
		/// <summary>
		/// Reads a map block.
		/// </summary>
		public Block(byte x, byte y, Stream source) : this(x, y, (byte)source.ReadByte(), (byte)source.ReadByte(), (byte)source.ReadByte())
		{
			
		}
	}
}
