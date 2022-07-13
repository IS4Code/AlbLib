/* Date: 11.8.2014, Time: 13:59 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AlbLib.Imaging;
using AlbLib.Items;
using AlbLib.Mapping;
using AlbLib.SaveGame;
using AlbLib.Sounds;
using AlbLib.Texts;
using AlbLib.XLD;

namespace AlbLib
{
	public static class GameData
    {
        public static readonly XLDRepository<SaveGame.NPC> NPCCharacters = new XLDRepository<SaveGame.NPC>(
            () => Paths.NPCCharacters,
            (i, s, l) => new SaveGame.NPC(s)
        );

        public static readonly XLDRepository<SaveGame.Character> PartyCharacters = new XLDRepository<SaveGame.Character>(
            () => Paths.PartyCharacters,
            (i, s, l) => new SaveGame.Character(s)
        );

        public static readonly XLDRepository<SaveGame.Monster> MonsterCharacters = new XLDRepository<SaveGame.Monster>(
            () => Paths.MonsterChars,
            (i, s, l) => new SaveGame.Monster(s)
        );

        public static readonly XLDRepository<Scripting.EventSet> EventSets = new XLDRepository<Scripting.EventSet>(
            () => Paths.EventSets,
            (i, s, l) => new Scripting.EventSet(s)
        );

        #region Maps
        public static readonly XLDRepository<Map> Maps = new XLDRepository<Map>(
			()=>Paths.MapData,
			(i,s,l)=>new Map(i,s)
		);

        public static readonly XLDRepository<BlockList> Blocks = new XLDRepository<BlockList>(
            () => Paths.BlockList,
            (i, s, l) => new BlockList(i, s)
        );

        #region 3D
        public static readonly XLDRepository<LabData> LabData = new XLDRepository<LabData>(
			()=>Paths.LabData,
			(i,s,l)=>new LabData(s)
		);
		
		public static readonly XLDRepository<AutoGFX> AutomapGraphics = new XLDRepository<AutoGFX>(
			()=>Paths.AutomapGraphics,
			(i,s,l)=>new AutoGFX(s,l)
		);
		
		public static readonly XLDRepository<HeaderedImage>
			Backgrounds3D = GetHeaderedImages(()=>Paths.Backgrounds3D, true);
		
		public static readonly XLDRepository<RawImage>
			Objects3D = GetRawImages(()=>Paths.Objects3D);
		
		public static readonly XLDRepository<RawImage>
			Walls3D = GetRawImages(()=>Paths.Walls3D);
		
		public static readonly XLDRepository<RawImage>
			Overlays3D = GetRawImages(()=>Paths.Overlays3D);
		
		public static readonly XLDRepository<HeaderedImage>
			BodyImages = GetHeaderedImages(()=>Paths.BodyImages, true);
		
		public static readonly XLDRepository<RawImage>
			Floors3D = GetRawImages(()=>Paths.Floors3D, 64, 64);
		#endregion
		
		#region 2D
		public static readonly XLDRepository<HeaderedImage>
			BigNPC = GetHeaderedImages(()=>Paths.NPCBig, true);
		
		public static readonly XLDRepository<HeaderedImage>
			SmallNPC = GetHeaderedImages(()=>Paths.NPCSmall, true);
		
		public static readonly XLDRepository<HeaderedImage>
			BigParty = GetHeaderedImages(()=>Paths.PartyBig, true);
		
		public static readonly XLDRepository<HeaderedImage>
			SmallParty = GetHeaderedImages(()=>Paths.PartySmall, true);
		
		public static readonly ArrayXLDRepository<TileData> MapIcons = new ArrayXLDRepository<TileData>(
			()=>Paths.IconData,
			(i,s,l)=>AlbLib.Mapping.MapIcons.ReadIconData(s,l)
		);
		
		public static readonly ArrayXLDRepository<RawImage> IconGraphics = new ArrayXLDRepository<RawImage>(
			()=>Paths.IconGraphics,
			(i,s,l)=>AlbLib.Mapping.MapIcons.ReadIconGraphics(s,l)
		);
		#endregion
		#endregion
		
		#region Cutscenes
		public static readonly XLDRepository<ILBMImage> Pictures = new XLDRepository<ILBMImage>(
			()=>Paths.Pictures,
			(i,s,l)=>ILBMImage.FromStream(s)
		);
		
		public static readonly XLDRepository<XLDSubfile> Flics = new XLDRepository<XLDSubfile>(
			()=>Paths.Flics.WithDefaultLanguage(),
			(i,s,l)=>new XLDSubfile(s,l)
		);
		#endregion
		
		#region Combat
		public static readonly XLDRepository<RawImage>
			CombatBackgrounds = GetRawImages(()=>Paths.CombatBackgrounds, 360);

        public static readonly XLDRepository<HeaderedImage>
            CombatGraphics = GetHeaderedImages(() => Paths.CombatGraphics, false);

        public static readonly XLDRepository<Monster> Monsters = new XLDRepository<Monster>(
			()=>Paths.MonsterChars,
			(i,s,l)=>new Monster(s)
		);
		
		public static readonly XLDRepository<TransparencyTable> TransparencyTables = new XLDRepository<TransparencyTable>(
			()=>Paths.TransparencyTables,
			(i,s,l)=>new TransparencyTable(i,s)
		);
		#endregion
		
		#region Texts
		public static readonly SysTextRepository SystemTexts = new SysTextRepository(
			()=>Paths.SystemTexts.WithDefaultLanguage()
		);
		
		public static readonly XLDRepository<TextLibrary>
			EventTexts = GetTexts(()=>Paths.EventTexts.WithDefaultLanguage());
		
		public static readonly XLDRepository<TextLibrary>
			MapTexts = GetTexts(()=>Paths.MapTexts.WithDefaultLanguage());
		#endregion
		
		public static readonly XLDRepository<WaveLib> WaveLibs = new XLDRepository<WaveLib>(
			()=>Paths.WaveLibs,
			(i,s,l)=>new WaveLib(s)
		);
		
		public static readonly SimpleRepository<ImagePalette> FullPalettes = new SimpleRepository<ImagePalette>(
			(i)=>Palettes.Open(i)+ImagePalette.GetGlobalPalette(),
			()=>Palettes.IndexEnumerate().Select(p => new KeyValuePair<int,ImagePalette>(p.Key, p.Value+ImagePalette.GetGlobalPalette()))
		);
		public static readonly XLDRepository<ImagePalette> Palettes = new XLDRepository<ImagePalette>(
			()=>Paths.PaletteN,
			(i,s,l)=>ImagePalette.Load(s,l/3,PaletteFormat.Binary)
		);
		
		public static readonly SimpleRepository<ItemType> Items = new SimpleRepository<ItemType>(
			(i)=>ItemType.GetItemType(i),
			()=>(IList<ItemType>)ItemType.GetItemTypes()
		);
		
		private static XLDRepository<HeaderedImage> GetHeaderedImages(Func<XLDPathInfo> path, bool constsize)
		{
			return new XLDRepository<HeaderedImage>(path, (i,s,l)=>new HeaderedImage(s, constsize));
		}
		private static XLDRepository<RawImage> GetRawImages(Func<XLDPathInfo> path)
		{
			return new XLDRepository<RawImage>(path, (i,s,l)=>new RawImage(s,l));
		}
		private static XLDRepository<RawImage> GetRawImages(Func<XLDPathInfo> path, int width)
		{
			return new XLDRepository<RawImage>(path, (i,s,l)=>new RawImage(s,width,(l+width-1)/width));
		}
		private static XLDRepository<RawImage> GetRawImages(Func<XLDPathInfo> path, int width, int height)
		{
			return new XLDRepository<RawImage>(path, (i,s,l)=>new RawImage(s,width,Math.Max(height, (l+width-1)/width)));
		}
		private static XLDRepository<TextLibrary> GetTexts(Func<XLDPathInfo> path)
		{
			return new XLDRepository<TextLibrary>(path, (i,s,l)=>new TextLibrary(s));
		}
		
		public static IEnumerable<IRepository> GetRepositories()
		{
			return typeof(GameData).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.IsInitOnly).Select(f => f.GetValue(null)).OfType<IRepository>();
		}
	}
}
