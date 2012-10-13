//Do not compile this file

Paths.SetXLDLIBS(Environment.CurrentDirectory);
Paths.Main = "MAIN.EXE";
Drawing.TransparentIndex = 0;
GameInterfaceImageCache.Load();
for(int i = 0; i < GameInterfaceImageCache.Images.Count; i++)
{
	GameInterfaceImageCache.Images[i].DrawToBitmap(0).Save("interface\\"+i+".png");
}



Stopwatch watch = new Stopwatch();
byte[] data = File.ReadAllBytes("SCREEN.LBM");
watch.Start();
for(int i = 0; i < 10000; i++)
{
	ILBMImage img = new ILBMImage(data);
	img.DrawToBitmap();
}
watch.Stop();
Console.WriteLine(watch.ElapsedMilliseconds);




using(FileStream stream = new FileStream("SAMPLES0.XLD", FileMode.Open))
{
	int length = XLDFile.ReadToIndex(stream, 1);
	RawPCMSound sound = new RawPCMSound(stream, length);
	SoundPlayer player = new SoundPlayer(sound.ToWAVEStream());
	player.Play();
}




ImagePalette pal = ImagePalette.Create(Color.White, Color.Black);
pal += ImagePalette.Create(Color.Red, Color.Blue);
foreach(Color c in pal)
{
	Console.WriteLine(c);
}





Paths.SetXLDLIBS(Environment.CurrentDirectory);
Items.LoadItemStates();
Texts.LoadItemNames();

XLD.XLDInfo xld;
int pos;
byte[] buffer = File.ReadAllBytes(@"C:\HRY\Albion\SAVES\SAVE.006");
SaveGame.SaveGameInfo info;
using(MemoryStream stream = new MemoryStream(buffer))
{
	info = new SaveGame.SaveGameInfo(stream);
	xld = XLD.FindXLD(stream, 0, out pos);
}

var ch = new SaveGame.Character(xld.Subfiles[0]);
SaveGame.ItemStack stack = ch.Inventory[0,0];
Console.WriteLine(ch.Name+" has "+stack);
stack.Flags = SaveGame.ItemFlags.Broken | SaveGame.ItemFlags.ShowMoreInfo;
ch.Inventory[0,0] = stack;
xld.Subfiles[0] = ch.ToRawData();
xld.ToRawData().CopyTo(buffer, pos);
File.WriteAllBytes(@"C:\HRY\Albion\SAVES\SAVE.006", buffer);




using(FileStream stream = new FileStream("MAPDATA2.XLD", FileMode.Open))
{
	int len = XLDFile.ReadToIndex(stream, 0);
	Map map = new Map(stream);
	Bitmap bmp = new Bitmap(map.Width*16, map.Height*16);
	Graphics gr = Graphics.FromImage(bmp);
	foreach(Tile tile in map.Data)
	{
		short underlay = tile.Underlay;
		if(underlay > 1)
		{
			RawImage icon = IconGraphics.GetTileUnderlay(map.Tileset-1, tile);
			gr.DrawImageUnscaled(icon.DrawToBitmap((short)(map.Palette-1)), tile.X*16, tile.Y*16);
		}
		short overlay = tile.Overlay;
		if(overlay > 1)
		{
			RawImage icon = IconGraphics.GetTileOverlay(map.Tileset-1, tile);
			gr.DrawImageUnscaled(icon.DrawToBitmap((short)(map.Palette-1)), tile.X*16, tile.Y*16);
		}
	}
	bmp.Save("map.png");
}

byte[] pal = new byte[]
{
	22,// - Mountains and grassland
	23,// - Test dungeon
	26,// - Umajo day
	31,// - Dungeon
	32,// - Dungeon
	33,// - Dungeon
	34,// - Khamulon dungeon
	35,// - Iskai dungeon
	36,// - Cavern
	37,// - Toronto service floor
	38,// - Argim
	39,// - Forest day
	40,// - Town night
	41,// - Town day
	42,// - Jungle day
	43,// - Savanna
	51,// - Umajo night
	52,// - Forest night
	53 // - Jungle night
};

ImageBase bg;
using(FileStream stream = new FileStream("COMBACK0.XLD", FileMode.Open))
{
	XLDFile.ReadToIndex(stream, 1);
	bg = new RawImage(stream, 360, 192);
}
ImageBase obj;
using(FileStream stream = new FileStream("MONGFX0.XLD", FileMode.Open))
{
	XLDFile.ReadToIndex(stream, 0);
	obj = new HeaderedImage(stream);
}

GraphicPlane plane = new GraphicPlane();
plane.Palette = 23;
plane.Background = bg;
GraphicObject gr = new GraphicObject(obj, new Point(130, 50));
gr.Transparency = TransparencyType.None;
plane.Objects = new[]{gr};
plane.Render().Save("plane4.png");


ImagePalette.TransparentIndex = 0;
using(FileStream stream = new FileStream("MAPDATA2.XLD", FileMode.Open))
{
	int len = XLDFile.ReadToIndex(stream, 0);
	Map map = new Map(stream);
	Bitmap bmp = new Bitmap(map.Width*16, map.Height*16);
	Graphics gr = Graphics.FromImage(bmp);
	foreach(Tile tile in map.Data)
	{
		short underlay = tile.Underlay;
		if(underlay > 1)
		{
			RawImage icon = IconGraphics.GetTileUnderlay(map.Tileset-1, tile);
			gr.DrawImageUnscaled(icon.Render(new ModifierPalette(ImagePalette.GetPalette((byte)(map.Palette-1))+ImagePalette.GetGlobalPalette()){Modifiers = new[]{BlockModifier.Night(0.5)}}), tile.X*16, tile.Y*16);
		}
		short overlay = tile.Overlay;
		if(overlay > 1)
		{
			RawImage icon = IconGraphics.GetTileOverlay(map.Tileset-1, tile);
			gr.DrawImageUnscaled(icon.Render(new ModifierPalette(ImagePalette.GetPalette((byte)(map.Palette-1))+ImagePalette.GetGlobalPalette()){Modifiers = new[]{BlockModifier.Night(0.5)}}), tile.X*16, tile.Y*16);
		}
	}
	bmp.Save("map.png");
}


using(FileStream stream = new FileStream("CREDITS.SMK", FileMode.Open))
{
	BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
	string sig = new String(reader.ReadChars(4));
	int width = reader.ReadInt32();
	int height = reader.ReadInt32();
	int frames = reader.ReadInt32();
	int framerate = reader.ReadInt32();
	int flags = reader.ReadInt32();
	int[] audiosize = new int[7];
	for(int i = 0; i < 7; i++)audiosize[i] = reader.ReadInt32();
	int treesize = reader.ReadInt32();
	int mmapsize = reader.ReadInt32();
	int mclrsize = reader.ReadInt32();
	int fullsize = reader.ReadInt32();
	int typesize = reader.ReadInt32();
	int[] audiorate = new int[7];
	for(int i = 0; i < 7; i++)audiorate[i] = reader.ReadInt32();
	int dummy = reader.ReadInt32();
	FrameSize[] framesizes = new FrameSize[frames];
	for(int i = 0; i < frames; i++)framesizes[i] = new FrameSize(reader.ReadUInt32());
	FrameType[] frametypes = new FrameType[frames];
	for(int i = 0; i < frames; i++)frametypes[i] = new FrameType(reader.ReadByte());
	
}

struct FrameSize
{
	public readonly int Size;
	public readonly bool Keyframe;
	
	public FrameSize(uint size)
	{
		Size = (int)(size&0xFFFFFFFC);
		Keyframe = (size&1)==1;
	}
}

struct FrameType
{
	public readonly bool Palette;
	public readonly bool[] AudioTrack;
	
	public FrameType(byte type) : this()
	{
		if((type&1) != 0)
		{
			Palette = true;
		}
		
		AudioTrack = new bool[7];
		for(int i = 0; i < 7; i++)
		{
			if((type&(1<<i)) != 0)
			{
				AudioTrack[i] = true;
			}
		}
	}
}


using(FileStream stream = new FileStream("res/WAVELIB0.XLD", FileMode.Open))
{
	int length = XLDFile.ReadToIndex(stream, 6);
	HeaderedPCMSound sound = new HeaderedPCMSound(stream);
	SoundPlayer player = new SoundPlayer();
	foreach(Sample s in sound.Samples)
	{
		MemoryStream data = s.Sound.ToWAVEStream();
		player.Stream = data;
		player.Play();
		Console.ReadKey(true);
	}
	player.Stop();
}


		/*Encoding enc = new VisualEncoding("slovak");
		string text = "\"Čo by som len teraz dal za to, keby som mal so sebou pár meracích prístrojov! Nemôžu si predsa robiť takéto žarty z fyzikálnych zákonov!\"";
		byte[] encoded = enc.GetBytes(text+"\0");
		string text2 = enc.GetString(encoded);
		Console.WriteLine(text2);*/
		/*string str = "Ahoj\nsvete";
		byte[] bytes = enc.GetBytes(str);
		string str2 = new String(enc.GetChars(bytes));*/
		
		/*ImagePalette pal = ImagePalette.Create(new Color[192])+ImagePalette.GetGlobalPalette();
		RawImage img = new RawImage(new byte[1], 1);
		img.Render(pal).Save("x.png");*/
		
		/*using(FileStream stream = new FileStream("3DFLOOR2.XLD", FileMode.Open))
		{
			foreach(XLDSubfile sub in XLDFile.EnumerateSubfiles(stream))
			{
				if(sub.Length == 0)continue;
				int pal = 0;
				int pal2 = -1;

				RawImage img = new RawImage(sub.Data, 64);
				Image bmp = img.Render(pal);
				bmp.Save("floor2/"+sub.Index+"-"+pal+".png");
				if(pal2 != -1)
				{
					bmp = img.Render(pal2);
					bmp.Save("floor2/"+sub.Index+"-"+pal2+".png");
				}
			}
		}*/
		
		/*Stopwatch sw = new Stopwatch();
		sw.Start();
		foreach(var pair in Paths.MapDataN.EnumerateAllSubfiles(1))
		{
			XLDSubfile sub = pair.Value;
			if(sub.Length==0)continue;
			int id = pair.Key;
			if(id%10==0)Console.WriteLine(id);
			Map map = new Map(id, sub.GetInputStream());
			if(map.Type != MapType.Map2D)continue;
			map.Combine().Render().Save("maps/"+id+".png");
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);*/
		
		/*RawImage npcimg;
		using(FileStream stream = new FileStream("npc.raw", FileMode.Open))
		{
			npcimg = new RawImage(stream, 16, 16);
		}*/
		
		/*foreach(var pair in Paths.MapDataN.EnumerateAllSubfiles(1))
		{
			if(pair.Value.Length == 0)continue;
			Map map = new Map(pair.Key, pair.Value.GetInputStream());
			if(map.Corrupted)Console.WriteLine(map.Id);
		}
		
		MainExecutableImages.Load();
		RawImage npcimg = MainExecutableImages.Images[70];*/
		
		/*using(FileStream stream = new FileStream("res/MAPDATA1.XLD", FileMode.Open))
		{
			int size = XLDFile.ReadToIndex(stream, 5);
			byte[] map = new byte[size];
			stream.Read(map,0,size);
			File.WriteAllBytes("map105.map", map);
		}*/
		
		/*using(FileStream stream = new FileStream("map300.map", FileMode.Open))
		{
			Map map = new Map(300, stream);
			GraphicPlane plane = map.Combine();
			foreach(NPC npc in map.NPCs)
			{
				foreach(Position p in npc.Positions)
				{
					GraphicObject obj = new GraphicObject(npcimg, new Point(p.X*16, p.Y*16));
					plane.Objects.Add(obj);
				}
			}
			plane.Render().Save("npcs.png");
		}*/
		
		/*RawImage[] floors = new RawImage[100];
		foreach(XLDSubfile sub in XLDFile.EnumerateSubfiles("res/3DFLOOR0.XLD"))
		{
			floors[sub.Index] = new RawImage(sub.Data, 64, 64);
		}
		RawImage[] blocks = new RawImage[20];
		
		
		RawImage[] minimap;
		
		Map map = Map.Load(110);
		ImagePalette src = ImagePalette.GetFullPalette(map.Palette);
		ImagePalette mmpal = new MinimapPalette(src, MinimapType.Classic);
		byte labdata = map.Labdata;
		using(FileStream stream = new FileStream(Paths.LabDataN.Format(labdata/100), FileMode.Open))
		{
			XLDFile.ReadToIndex(stream, labdata%100);
			BinaryReader reader = new BinaryReader(stream);
			reader.ReadBytes(38);
			reader.ReadBytes(reader.ReadInt16()*66);
			short floorsc = reader.ReadInt16();
			minimap = new RawImage[floorsc];
			for(int i = 0; i < floorsc; i++)
			{
				reader.ReadBytes(6);
				short gfxid = reader.ReadInt16();
				reader.ReadInt16();
				minimap[i] = Minimize(floors[gfxid-1], src, mmpal);
			}
		}
		
		GraphicPlane plane = new GraphicPlane(map.Width*8, map.Height*8);
		plane.Palette = mmpal;
		foreach(Block blk in map.BlockData)
		{
			if(blk.Floor == 0)continue;
			GraphicObject obj = new GraphicObject(minimap[blk.Floor-1], new Point(blk.X*8, blk.Y*8));
			plane.Objects.Add(obj);
		}
		plane.Render().Save("minimap3.png");*/
		
		//TextCore.DefaultEncoding = new VisualEncoding("czech");
		
		/*MainExecutableImages.Load();
		ImagePalette.TransparentIndex = 0;
		RawImage gold = MainExecutableImages.Images[44];
		gold.Render(19).Save("protection.png");*/
		
		/*RawImage img = new RawImage(File.ReadAllBytes("ITEMGFX"), 16);
		ImagePalette.TransparentIndex = 0;
		img.Render(1).Save("items.png");*/
		
		/*Dictionary<ItemClass,string> classes = new Dictionary<ItemClass, string>
		{
			{ItemClass.Ammo, "Ammunition"},
			{ItemClass.Amulet, "Amulet"},
			{ItemClass.Armour, "Armour"},
			{ItemClass.Document, "Document"},
			{ItemClass.Drink, "Drink"},
			{ItemClass.Helmet, "Helmet"},
			{ItemClass.Jewel, "Jewel"},
			{ItemClass.Key, "Key"},
			{ItemClass.Lockpick, "Lockpick"},
			{ItemClass.Magical, "Magical"},
			{ItemClass.Melee, "Melee weapon"},
			{ItemClass.Normal, "Normal"},
			{ItemClass.Ranged, "Ranged weapon"},
			{ItemClass.Ring, "Ring"},
			{ItemClass.Shield, "Shield"},
			{ItemClass.Shoes, "Shoes"},
			{ItemClass.Special, "Special"},
			{ItemClass.Spell, "Spell"},
			{ItemClass.Staff, "Staff"},
			{ItemClass.Tool, "Tool"},
		};
		
		Dictionary<AttributeType,string> attributes = new Dictionary<AttributeType, string>
		{
			{AttributeType.Dexterity, "Dexterity"},
			{AttributeType.Intelligence, "Intelligence"},
			{AttributeType.Luck, "Luck"},
			{AttributeType.MagicResistance, "Magic resistance"},
			{AttributeType.MagicTalent, "Magic talent"},
			{AttributeType.Speed, "Speed"},
			{AttributeType.Stamina, "Stamina"},
			{AttributeType.Strength, "Strength"},
		};
		
		Dictionary<SkillType,string> skills = new Dictionary<SkillType, string>
		{
			{SkillType.CloseRangeCombat, "Close range combat"},
			{SkillType.CriticalHit, "Critical hit"},
			{SkillType.Lockpicking, "Lockpicking"},
			{SkillType.LongRangeCombat, "Long range combat"},
		};
		
		Dictionary<ItemSpellType,string> spells = new Dictionary<ItemSpellType, string>
		{
			{ItemSpellType.DjiKas,"Dji-Kas"},
			{ItemSpellType.Druid,"Druid"},
			{ItemSpellType.EnlightenedOne,"Enlightened One"},
			{ItemSpellType.OquloKamulos,"Oqulo Kamulos"},
		};
		
		Dictionary<int,string> systexts = new Dictionary<int, string>();
		using(FileStream stream = File.Open("SYSTEXTS", FileMode.Open))
		{
			int b;
			byte[] numb = new byte[4];
			while((b=stream.ReadByte())!=-1)
			{
				if(b == 0x5B)//[
				{
					stream.Read(numb, 0, 4);
					if(stream.ReadByte() == 0x3A)//:
					{
						int key = Int32.Parse(Encoding.ASCII.GetString(numb));
						StringBuilder val = new StringBuilder();
						while((b=stream.ReadByte()) != 0x5D)//]
						{
							val.Append(TextCore.DefaultEncoding.GetString(new[]{(byte)b}));
						}
						systexts[key] = val.ToString();
					}else{
						continue;
					}
				}
			}
		}*/
		
		/*string[] names = new string[463];
		for(short s = 1; s <= 462; s++)
		{
			names[s] = ItemState.GetItemState(s).TypeName;
		}
		Paths.ItemName = "res/Kopie - ITEMNAME.DAT";
		TextCore.DefaultEncoding = new VisualEncoding("default");*/
		/*using(StreamWriter writer = File.CreateText("items.html"))
		{
			writer.WriteLine(File.ReadAllText("itemheader.html"));
			writer.WriteLine("<table rules=all>");
			for(short s = 1; s <= 462; s++)
			{
				if((s-1)%15==0)writer.WriteLine("<tr><th>ID</th><th>Image</th><th>Name</th><th>Class</th><th>Weight</th><th>Value</th><th>Properties</th><th>Bonuses</th></tr>");
				ItemState state = ItemState.GetItemState(s);
				writer.Write("<tr>");
				writer.Write("<th>"+s+"</th>");
				writer.Write("<td align=center><div class=\"itemicon\" style=\"background-position: 0 -"+state.Icon*32+"px\"></div></td>");
				writer.Write("<td>"+UpperFirst(state.TypeName)+"</td>");
				writer.Write("<td>"+classes[state.Class]+"</td>");
				writer.Write("<td>"+state.Weight+" g</td>");
				writer.Write("<td>"+state.Value+" <img src=\"res/gold.png\"></td>");
				
				List<string> properties = new List<string>();
				if(state.PhysicalDamageCaused != 0)properties.Add( "Damage: "+state.PhysicalDamageCaused+" <img src=\"res/damage.png\">" );
				if(state.PhysicalDamageProtection != 0)properties.Add( "Protection: "+state.PhysicalDamageProtection+" <img src=\"res/protection.png\">" );

				if((state.Count1&2)==2)properties.Add("Important");
				if((state.Count1&4)==4)properties.Add("Stackable");
				
				if((state.Count2&1)==1)properties.Add("Explored");
				if((state.Count2&4)==4)properties.Add("Cursed");
				if(properties.Count>0)writer.Write("<td>"+String.Join("<br>", properties)+"</td>");
				else writer.Write("<td class=\"empty\"></td>");
				
				List<string> bonuses = new List<string>();
				if(state.AttributeBonus != 0)bonuses.Add( attributes[state.AttributeType]+": "+state.AttributeBonus.ToString("+#;-#;0") );
				if(state.LifePointsBonus != 0)bonuses.Add( "Life points: "+state.LifePointsBonus.ToString("+#;-#;0") );
				if(state.SpellPointsBonus != 0)bonuses.Add( "Spell points: "+state.SpellPointsBonus.ToString("+#;-#;0") );
				if(state.SkillBonus != 0)bonuses.Add( skills[state.SkillTypeBonus]+": "+state.SkillBonus.ToString("+#;-#;0") );
				
				if(state.Skill1Tax != 0)bonuses.Add( skills[state.SkillType1Tax]+": "+(-state.Skill1Tax).ToString("+#;-#;0") );
				if(state.Skill2Tax != 0)bonuses.Add( skills[state.SkillType2Tax]+": "+(-state.Skill2Tax).ToString("+#;-#;0") );
				
				if(state.SpellID != 0)bonuses.Add( spells[state.Spell]+" &ndash; "+UpperFirst(systexts[203+(byte)state.Spell*30+state.SpellID-1]) );
				//else writer.Write("<td class=\"empty\"></td>");
				
				if(bonuses.Count>0)writer.Write("<td>"+String.Join("<br>", bonuses)+"</td>");
				else writer.Write("<td class=\"empty\"></td>");
				writer.WriteLine("</tr>");
			}
			writer.WriteLine("</table>");
		}*/
		
		
		
		
		
		/*Map map = Map.Load(283);
		map.Combine().Render().Save("map283.png");*/
		
		/*foreach(var pair in Paths.MapDataN.EnumerateAllSubfiles(1))
		{
			if(pair.Value.Length==0)continue;
			Console.WriteLine(pair.Key);
			Map map = new Map(pair.Key, pair.Value.GetInputStream());
			if(map.Type == MapType.Map3D)
			{
				map.Combine().Render().Save("maps3d/"+pair.Key+".png");
			}else continue;
		}*/

				double width = Math.Sqrt(3)/2*(64);
		double height = 1.5*(64);
		Bitmap bmp = new Bitmap(500, 500);
		Graphics gr = Graphics.FromImage(bmp);
		
		Matrix wallmatrix = new Matrix();
		wallmatrix.Translate(100, 100);
		wallmatrix.Rotate(60);
		wallmatrix.Translate(-100, -100);
		wallmatrix.Scale(1.25f, (float)(1/Math.Sqrt(2)));
		wallmatrix.Translate(100, 100);
		wallmatrix.Rotate(45);
		
		gr.Transform = wallmatrix;
		
		Image wallimg = LabGraphics.GetWall(109, 1).Render(3);
		
		gr.DrawImage(wallimg, 0, 0, wallimg.Width*0.5f+1, wallimg.Height*0.5f+1);
		
		bmp.Save("walliso.png");
		
		/*var m = Map.Load(101);
		LabData ld = LabData.GetLabData(m.Labdata);
		ImagePalette pal = ImagePalette.GetFullPalette(m.Palette);
		
		double diagonal = (m.Width+m.Height)/2d;
		double width = diagonal*(64*Math.Sqrt(3));
		double height = diagonal*(64);
		
		Bitmap bmp = new Bitmap((int)width, (int)height);
		
		Matrix floormatrix = new Matrix();
		floormatrix.Scale(1.25f, (float)(1/Math.Sqrt(2)));
		floormatrix.Rotate(45);
		floormatrix.Translate((float)(width/4), -(float)(height/4));
		
		Matrix wallmatrix = new Matrix();
		wallmatrix.Translate((float)(width/4), -(float)(height/4));
		wallmatrix.Rotate(60);
		wallmatrix.Scale(1.25f, (float)(1/Math.Sqrt(2)));
		wallmatrix.Rotate(45);
		
		
		Graphics gr = Graphics.FromImage(bmp);
		
		foreach(Block b in m.BlockData)
		{
			RawImage floor = LabGraphics.GetFloor(m.Labdata, b.Floor);
			if(floor != null)
			{
				gr.Transform = floormatrix;
				Image floorimg = floor.Render(pal);
				gr.DrawImage(floorimg, b.X*64, b.Y*64, 65, 65);
			}
				
			if(b.IsWall)
			{
				RawImage wall = LabGraphics.GetWall(m.Labdata, b.Wall);
				if(wall != null)
				{
					gr.Transform = wallmatrix;
					Image wallimg = wall.Render(pal);
					gr.DrawImage(wallimg, b.X*64, b.Y*64, wall.Width*0.8f+1, wall.Height*0.5f+1);
				}
			}
		}
		
		bmp.Save("mapiso.png");
		//*/
