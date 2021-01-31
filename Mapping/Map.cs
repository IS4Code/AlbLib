using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.Texts;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Class representing game map.
	/// </summary>
	[Serializable]
	public class Map : IGameResource
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
		public MapType Type{get;private set;}
		
		/// <summary>
		/// Map music.
		/// </summary>
		public byte BackgroundMusic{get;set;}
		
		/// <summary>
		/// Width in tiles.
		/// </summary>
		public byte Width{
			get{
				switch(Type)
				{
					case MapType.Map2D:
						return (byte)TileData.GetLength(0);
					case MapType.Map3D:
						return (byte)BlockData.GetLength(0);
				}
				return 0;
			}
		}
		
		/// <summary>
		/// Height in tiles.
		/// </summary>
		public byte Height{
			get{
				switch(Type)
				{
					case MapType.Map2D:
						return (byte)TileData.GetLength(1);
					case MapType.Map3D:
						return (byte)BlockData.GetLength(1);
				}
				return 0;
			}
		}
		
		private byte tilesid;
		
		/// <summary>
		/// One-based tileset ID.
		/// </summary>
		public byte Tileset{
			get{
				if(Type == MapType.Map2D)
				{
					return tilesid;
				}else throw new InvalidOperationException("Only 2D maps have tileset.");
			}
			set{
				if(Type == MapType.Map2D)
				{
					tilesid = value;
				}else throw new InvalidOperationException("Only 2D maps have tileset.");
			}
		}
		
		/// <summary>
		/// One-based labdata ID.
		/// </summary>
		public byte Labdata{
			get{
				if(Type == MapType.Map3D)
				{
					return tilesid;
				}else throw new InvalidOperationException("Only 3D maps have labdata.");
			}
			set{
				if(Type == MapType.Map3D)
				{
					tilesid = value;
				}else throw new InvalidOperationException("Only 3D maps have labdata.");
			}
		}
		
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
		
		public EventHeader[] AutoEvents{get;set;}
		
		public MapEvent[] Events{get;set;}
		
		/// <summary>
		/// NPC/monster data.
		/// </summary>
		public NPC[] NPCs{get;set;}
		
		public IEnumerable<NPC> UsedNPCs{
			get{
				foreach(NPC npc in NPCs)
				{
					if(!npc.IsEmpty)yield return npc;
				}
			}
		}
		
		private Tile[,] tiledata;
		private Block[,] blockdata;
		
		/// <summary>
		/// Array of 2D map tiles.
		/// </summary>
		public Tile[,] TileData{
			get{
				if(Type == MapType.Map2D)
				{
					return tiledata;
				}else throw new InvalidOperationException("Only 2D maps have tiles.");
			}
			set{
				if(Type == MapType.Map2D)
				{
					if(value == null || value.GetLength(0) > Byte.MaxValue || value.GetLength(1) > Byte.MaxValue)throw new ArgumentException("Map cannot be empty.", "value");
					tiledata = value;
				}else throw new InvalidOperationException("Only 2D maps have tiles.");
			}
		}
		
		/// <summary>
		/// Array of 3D map blocks.
		/// </summary>
		public Block[,] BlockData{
			get{
				if(Type == MapType.Map3D)
				{
					return blockdata;
				}else throw new InvalidOperationException("Only 3D maps have blocks.");
			}
			set{
				if(Type == MapType.Map3D)
				{
					if(value == null || value.GetLength(0) > Byte.MaxValue || value.GetLength(1) > Byte.MaxValue)throw new ArgumentException("Map cannot be empty.", "value");
					blockdata = value;
				}else throw new InvalidOperationException("Only 3D maps have blocks.");
			}
		}
		
		/// <summary>
		/// Array of map data, tiles or blocks.
		/// </summary>
		/*public IMapSquare[,] Data{
			get{
				IMapSquare[,] data = new IMapSquare[Width,Height];
				Array source = null;
				switch(Type)
				{
					case MapType.Map2D:
						source = TileData;
						break;
					case MapType.Map3D:
						source = BlockData;
						break;
				}
				foreach(IMapSquare square in source)
				{
					data[square.X,square.Y] = square;
				}
				return data;
			}
		}*/
		
		private GotoPoint[] gotopoints;
		
		public GotoPoint[] GotoPoints{
			get{
				if(Type == MapType.Map3D)
				{
					return gotopoints;
				}else throw new InvalidOperationException("Only 3D maps have goto-points.");
			}
			set{
				if(Type == MapType.Map3D)
				{
					gotopoints = value;
				}else throw new InvalidOperationException("Only 3D maps have goto-points.");
			}
		}
		
		private byte[] automap;
		
		public byte[] Automap{
			get{
				if(Type == MapType.Map3D)
				{
					return automap;
				}else throw new InvalidOperationException("Only 3D maps have automap.");
			}
			set{
				if(Type == MapType.Map3D)
				{
					automap = value;
				}else throw new InvalidOperationException("Only 3D maps have automap.");
			}
		}
		
		/// <summary>
		/// True if an exception occured while loading map.
		/// </summary>
		public bool Corrupted{
			get{
				return Error != null;
			}
		}
		
		public Exception Error{
			get; private set;
		}
		
		public short[] ActiveEvents{get;set;}
		
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
			
			BinaryReader reader = new BinaryReader(stream, TextCore.DefaultEncoding);
			
			//Basic info
			Flags = reader.ReadByte();
			NumNPC = reader.ReadByte();
			Type = (MapType)reader.ReadByte();
			if(Type != MapType.Map2D && Type != MapType.Map3D)throw new InvalidDataException("Unknown map format.");
			BackgroundMusic = reader.ReadByte();
			byte width = reader.ReadByte();
			byte height = reader.ReadByte();
			tilesid = reader.ReadByte();
			ComGFX = reader.ReadByte();
			Palette = reader.ReadByte();
			AnimRate = reader.ReadByte();
			
			//NPCs
			int npcstruct;
			if(NumNPC == 0)
			{
				npcstruct = 32;
			}else if(NumNPC == 64)
			{
				npcstruct = 96; 
			}else{
				npcstruct = NumNPC;
			}
			NPCs = new NPC[npcstruct];
			for(int i = 0; i < npcstruct; i++)
			{
				NPCs[i] = new NPC(stream);
			}
			
			if(Type == MapType.Map2D)
			{
				//Tile data
				tiledata = new Tile[width,height];
				for(byte y = 0; y < height; y++)
				for(byte x = 0; x < width; x++)
				{
					tiledata[x,y] = new Tile(x,y,stream);
				}
			}else if(Type == MapType.Map3D)
			{
				//Block data
				blockdata = new Block[width,height];
				for(byte y = 0; y < height; y++)
				for(byte x = 0; x < width; x++)
				{
					blockdata[x,y] = new Block(x,y,stream);
				}
			}
			
			try{
				short autoevents = reader.ReadInt16();
				AutoEvents = new EventHeader[autoevents];
				for(int i = 0; i < autoevents; i++)
				{
					AutoEvents[i] = new EventHeader(reader);
				}
				
				//EventHeader[][] tileEvents = new EventHeader[height][];
				for(int y = 0; y < height; y++)
				{
					short events = reader.ReadInt16();
					//tileEvents[y] = new EventHeader[events];
					for(int i = 0; i < events; i++)
					{
						EventHeader e = new EventHeader(reader);
						if(Type == MapType.Map2D)
							TileData[e.XPos,y].Event = e;
						else if(Type == MapType.Map3D)
							BlockData[e.XPos,y].Event = e;
					}
				}
				
				short numevents = reader.ReadInt16();
				Events = new MapEvent[numevents];
				for(int i = 0; i < numevents; i++)
				{
					Events[i] = new MapEvent(i, reader);
				}
				
				//NPC positions
				foreach(NPC npc in UsedNPCs)
				{
					if((npc.Movement & 3) != 0)
					{
						npc.Positions = new Position[1];
						npc.Positions[0] = new Position((byte)(reader.ReadByte()-1), (byte)(reader.ReadByte()-1));
					}else{
						npc.Positions = new Position[1152];
						for(int i = 0; i < 1152; i++)
						{
							npc.Positions[i] = new Position((byte)(reader.ReadByte()-1), (byte)(reader.ReadByte()-1));
						}
					}
				}
				
				//Active events
				int aesize;
				if(Type == MapType.Map2D)
				{
					aesize = 250;
				}else{
				//Goto-points
					short numgotop = reader.ReadInt16();
					gotopoints = new GotoPoint[numgotop];
					for(int i = 0; i < numgotop; i++)
					{
						GotoPoint gp = gotopoints[i] = new GotoPoint(reader);
						if(gp.X < width && gp.Y < height)
							BlockData[gp.X,gp.Y].GotoPoint = gp;
					}
				//Automap gfx remapping structure
					automap = reader.ReadBytes(64);
				//Active events	
					aesize = 64;
				}
				ActiveEvents = new short[aesize];
				for(int i = 0; i < aesize; i++)
				{
					ActiveEvents[i] = reader.ReadInt16();
				}
			}catch(EndOfStreamException e)
			{
				Error = e;
			}
		}
		
		/// <summary>
		/// Loads existing map using in-game ID. (Alt+F2)
		/// </summary>
		public static Map Load(int id)
		{
			int fid, sid;
			if(!Common.E(id, out fid, out sid))return null;
			using(FileStream stream = new FileStream(Paths.MapData.Format(fid), FileMode.Open))
			{
				XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)sid);
				int size = nav.SubfileLength;
				if(size == 0) return null;
				return new Map(id, nav);
			}
		}
		
		public GraphicPlane Combine()
		{
			return Combine(CombineArgs.Default);
		}
		
		/// <summary>
		/// Combines all tiles to form a graphic plane.
		/// </summary>
		/// <returns>
		/// Newly created graphic plane.
		/// </returns>
		public GraphicPlane Combine(CombineArgs args)
		{
			if(this.Type == MapType.Map2D)
			{
				GraphicPlane plane = new GraphicPlane(this.Width*16, this.Height*16);
				plane.Palette = ImagePalette.GetFullPalette(this.Palette);
				foreach(Tile t in this.TileData)
				{
					Point loc = new Point(t.X*16, t.Y*16);
					if(args.ShowUnderlays)
					{
						RawImage ul = MapIcons.GetTileUnderlay(this.Tileset, t).First();
						plane.Objects.Add(new GraphicObject(ul, loc));
					}
					if(!args.ShowHelpers&&IsHelperTile(tilesid, t.Overlay))continue;
					if(args.ShowOverlays)
					{
						RawImage ol = MapIcons.GetTileOverlay(this.Tileset, t).First();
						plane.Objects.Add(new GraphicObject(ol, loc));
					}
				}
				if(args.ShowNPCs2D)
				{
					foreach(NPC npc in UsedNPCs)
					{
						ImageBase img = NPCGraphics.GetNPCBig(npc.ObjectID)[6];
						Point loc = new Point(npc.X*16, npc.Y*16-img.GetHeight()+16);
						plane.Objects.Add(new GraphicObject(img, loc));
					}
				}
				return plane;
			}else if(this.Type == MapType.Map3D)
			{
				MinimapType mt = MinimapType.Classic;
				LabData ld = LabData.GetLabData(this.Labdata);
				
				GraphicPlane plane = new GraphicPlane(this.Width*8, this.Height*8);
				ImagePalette src = ImagePalette.GetFullPalette(this.Palette);
				ImagePalette dest = plane.Palette = new MinimapPalette(src);
				
				if(ld != null)
				{
					if(args.ShowFloors)
					{
						//Floors
						IndexedCache<RawImage> floorcache = new IndexedCache<RawImage>(i=>Minimize(LabGraphics.GetFloor(this.Labdata, i), src, dest));
						foreach(Block b in this.BlockData)
						{
							if(b.Floor != 0)
							{
								Point loc = new Point(b.X*8, b.Y*8);
								plane.Objects.Add(new GraphicObject(floorcache[b.Floor], loc));
							}
						}
					}
					
					if(args.ShowWalls)
					{
						//Walls
						foreach(Block b in this.BlockData)
						{
							if(b.IsWall && !ld.GetMinimapForm(b).VisibleOnMinimap)
							{
								WallForm form = WallForm.Close;
								if(b.Y > 0 &&        BlockData[b.X  , b.Y-1].IsWall)form |= WallForm.OpenTop;
								if(b.X < Width-1 &&  BlockData[b.X+1, b.Y  ].IsWall)form |= WallForm.OpenRight;
								if(b.Y < Height-1 && BlockData[b.X  , b.Y+1].IsWall)form |= WallForm.OpenBottom;
								if(b.X > 0 &&        BlockData[b.X-1, b.Y  ].IsWall)form |= WallForm.OpenLeft;
								Point loc = new Point(b.X*8, b.Y*8);
								plane.Objects.Add(new GraphicObject(AutoGFX.GetWall(form, mt), loc));
							}
						}
					}
				}
				
				foreach(Block b in this.BlockData)
				{
					Point loc = new Point(b.X*8, (b.Y-1)*8);
					if(args.ShowNPCs3D)
					{
						//NPCs
						foreach(NPC npc in UsedNPCs)
						{
							if(npc.Positions[0].X == b.X && npc.Positions[0].Y == b.Y)
							{
								byte type = 0;
								if(npc.Interaction == 2)type = 8;
								else type = 17;
								if(type != 0)
									plane.Objects.Add(new GraphicObject(AutoGFX.GetIcon(type, mt), loc));
							}
						}
					}
					
					if(args.ShowObjects)
					{
						//Objects
						if(ld != null)
						{
							if(!b.IsEmpty)
							{
								byte type = ld.GetMinimapForm(b).MinimapType;
								plane.Objects.Add(new GraphicObject(AutoGFX.GetIcon(type, mt), loc));
							}
						}
					}
					
					if(args.ShowGotoPoints)
					{
						//Goto-points
						if(b.GotoPoint!=null)
						{
							plane.Objects.Add(new GraphicObject(AutoGFX.GetIcon(18, mt), loc));
						}
					}
				}
				return plane;
			}
			return null;
		}
		
		public IEnumerable<IMapSquare> EnumerateSquares()
		{
			if(Type == MapType.Map2D)
			{
				foreach(Tile t in TileData)yield return t;
			}else if(Type == MapType.Map3D)
			{
				foreach(Block b in BlockData)yield return b;
			}
		}
		
		public int Save(Stream output)
		{
			throw new NotImplementedException();
		}
		
		public bool Equals(IGameResource obj)
		{
			return this.Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			if(obj is Map)
			{
				Map map = (Map)obj;
				throw new NotImplementedException();
			}else{
				return false;
			}
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
		
		public static bool operator ==(Map lhs, Map rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(Map lhs, Map rhs)
		{
			return !(lhs == rhs);
		}

		
		private static RawImage Minimize(ImageBase source, ImagePalette src, ImagePalette dest)
		{
			if(source == null)return null;
			byte[] full = source.ImageData;
			byte[] mini = new byte[64];
			
			for(int y = 0; y < 8; y++)
			for(int x = 0; x < 8; x++)
			{
				int rs = 0, gs = 0, bs = 0, cn = 0;
				for(int y2 = 0; y2 < 8; y2++)
				for(int x2 = 0; x2 < 8; x2++)
				{
					Color c = src[full[x*8+x2+(y*8+y2)*64]];
					rs += c.R;
					gs += c.G;
					bs += c.B;
					cn += 1;
				}
				Color average = Color.FromArgb(Convert.ToInt32(rs/(float)cn), Convert.ToInt32(gs/(float)cn), Convert.ToInt32(bs/(float)cn));
				byte avgbyte = (byte)dest.GetNearestColorIndex(average);
				mini[x+y*8] = avgbyte;
			}
			return new RawImage(mini, 8, 8);
		}
		
		public static bool IsHelperTile(int tileset, int tile)
		{
			switch(tileset)
			{
				case 2:
					switch(tile)
					{
						case 2689:case 2690:return true;
					}
					break;
				case 3:
					switch(tile)
					{
						case 2763:case 2764:return true;
					}
					break;
				case 5:
					switch(tile)
					{
						case 3018:case 3019:return true;
					}
					break;
				case 6:
					switch(tile)
					{
						case 2605:case 2606:return true;
					}
					break;
				case 7:
					switch(tile)
					{
						case 2931:case 2932:case 3559:return true;
					}
					break;
				case 8:
					switch(tile)
					{
						case 2471:case 2472:case 2550:return true;
					}
					break;
				case 9:
					switch(tile)
					{
						case 2598:case 2599:case 2810:case 2811:return true;
					}
					break;
				case 10:
					switch(tile)
					{
						case 2048:case 2049:return true;
					}
					break;
			}
			return false;
		}
	}
}