using System;
using System.Drawing;
using System.IO;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Class representing game map.
	/// </summary>
	public class Map
	{
		/// <summary>
		/// Id of map.
		/// </summary>
		public readonly int Id;
		
		/// <summary>
		/// Switch for wait/rest, light-environment, NPC converge range, possibly more.
		/// </summary>
		public byte Flags{get;set;}
		
		/// <summary>
		/// Amount of NPCs and monsters.
		/// </summary>
		public byte NumNPC{get;set;}
		
		/// <summary>
		/// Map type.
		/// </summary>
		public MapType Type{get;set;}
		
		/// <summary>
		/// Used sound. (?)
		/// </summary>
		public byte Sound{get;set;}
		
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
		
		/// <summary>
		/// One-based tileset ID.
		/// </summary>
		public byte Tileset{get;set;}
		
		/// <summary>
		/// Combat background graphics.
		/// </summary>
		public byte ComGFX{get;set;}
		
		/// <summary>
		/// One-based palette ID.
		/// </summary>
		public byte Palette{get;set;}
		
		/// <summary>
		/// Frequency of animations.
		/// </summary>
		public byte AnimRate{get;set;}
		
		private Tile[,] data;
		
		/// <summary>
		/// Array of map tiles.
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
		
		/// <summary>
		/// Loads map from stream.
		/// </summary>
		/// <param name="stream">
		/// Source stream.
		/// </param>
		public Map(Stream stream) : this(-1, stream)
		{
			
		}
		
		/// <summary>
		/// Loads map from stream and assigns an id.
		/// </summary>
		/// <param name="id">
		/// Id of map.
		/// </param>
		/// <param name="stream">
		/// Source stream.
		/// </param>
		public Map(int id, Stream stream)
		{
			Id = id;
			
			BinaryReader reader = new BinaryReader(stream);
			Flags = reader.ReadByte();
			NumNPC = reader.ReadByte();
			Type = (MapType)reader.ReadByte();
			Sound = reader.ReadByte();
			byte width = reader.ReadByte();
			byte height = reader.ReadByte();
			Tileset = reader.ReadByte();
			ComGFX = reader.ReadByte();
			Palette = reader.ReadByte();
			AnimRate = reader.ReadByte();
			if(NumNPC == 0)
			{
				reader.ReadBytes(320);
			}else if(NumNPC == 0x40)
			{
				reader.ReadBytes(960);
			}else{
				reader.ReadBytes(NumNPC*10);
			}
			data = new Tile[width,height];
			for(byte y = 0; y < height; y++)
			for(byte x = 0; x < width; x++)
			{
				data[x,y] = new Tile(x,y,stream);
			}
		}
		
		/// <summary>
		/// Loads existing map using in-game ID. (Alt+F2)
		/// </summary>
		public static Map Load(int id)
		{
			int fid = id/100;
			int sid = id%100;
			using(FileStream stream = new FileStream(Paths.MapDataN.Format(fid), FileMode.Open))
			{
				XLDFile.ReadToIndex(stream, sid);
				return new Map(id, stream);
			}
		}
		
		/// <summary>
		/// Combines all tiles to form a graphic plane.
		/// </summary>
		/// <returns>
		/// Newly created graphic plane.
		/// </returns>
		public GraphicPlane Combine()
		{
			GraphicPlane plane = new GraphicPlane(this.Width*16, this.Height*16);
			plane.Palette = ImagePalette.GetFullPalette(this.Palette);
			foreach(Tile t in this.Data)
			{
				RawImage ul = IconGraphics.GetTileUnderlay(this.Tileset, t);
				RawImage ol = IconGraphics.GetTileOverlay(this.Tileset, t);
				Point loc = new Point(t.X*16, t.Y*16);
				plane.Objects.Add(new GraphicObject(ul, loc));
				plane.Objects.Add(new GraphicObject(ol, loc));
			}
			return plane;
		}
	}
}