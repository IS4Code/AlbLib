using System;
using System.IO;
using System.Linq;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.Mapping;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Contains stuff from automap graphics.
	/// </summary>
	public class AutoGFX : IGameResource
	{
		public RawImage[] Backgrounds{get; private set;}
		public RawImage[] Walls{get; private set;}
		public RawImage[] Icons{get; private set;}
		public RawImage Pointer{get; set;}
		public RawImage[] PointerHeads{get; private set;}
		
		public AutoGFX()
		{
			Backgrounds = new RawImage[560];
			Walls = new RawImage[16];
			Icons = new RawImage[20];
			PointerHeads = new RawImage[7];
		}
		
		public AutoGFX(Stream input, int length) : this()
		{
			if(length != 51200)
			{
				throw new InvalidDataException("Invalid stream length.");
			}
			for(int i = 0; i < 560; i++)
			{
				Backgrounds[i] = new RawImage(input, 8, 8);
			}
			for(int i = 0; i < 16; i++)
			{
				Walls[i] = new RawImage(input, 8, 8);
			}
			for(int i = 0; i < 17; i++)
			{
				int size = 256;
				switch(i)
				{
					case 0:case 1:case 2:case 3:case 4:case 5:case 6:
						size *= 4;
						break;
					case 16:
						size *= 8;
						break;
				}
				Icons[i] = new RawImage(input, 16, 16, size);
			}
			Pointer = new RawImage(input, 16, 32);
			for(int i = 0; i < 7; i++)
			{
				PointerHeads[i] = new RawImage(input, 16, 16);
			}
		}
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="form">
		/// Formation of wall.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public RawImage GetWall(WallForm form)
		{
			return GetWall((int)form);
		}
		
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="index">
		/// Formation of wall.
		/// </param>
		/// <param name="type">
		/// Automap type.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public RawImage GetWall(int index)
		{
			return Walls[index];
		}
		
		public RawImage GetIcon(MapIcon icon)
		{
			return GetIcon((int)icon);
		}
		
		/// <summary>
		/// Gets minimap object image.
		/// </summary>
		/// <param name="index">
		/// Object type index.
		/// </param>
		/// <param name="type">
		/// Automap type.
		/// </param>
		/// <returns>
		/// Object image.
		/// </returns>
		public RawImage GetIcon(int index)
		{
			if(index <= 1) return null;
			return Icons[index-2];
		}
		
		public int Save(Stream output)
		{
			return Backgrounds.Concat(Walls).Concat(Icons).Concat(new[]{Pointer}).Concat(PointerHeads).Sum(img => img.Save(output));
		}
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="form">
		/// Formation of wall.
		/// </param>
		/// <param name="setindex">
		/// Automap type index.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public static RawImage GetWall(WallForm form, int setindex)
		{
			return GetWall((int)form, setindex);
		}
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="form">
		/// Formation of wall.
		/// </param>
		/// <param name="type">
		/// Automap type.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public static RawImage GetWall(WallForm form, MinimapType type)
		{
			return GetWall((int)form, (int)type);
		}
		
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="index">
		/// Formation of wall.
		/// </param>
		/// <param name="type">
		/// Automap type.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public static RawImage GetWall(int index, MinimapType type)
		{
			return GetWall(index, (int)type);
		}
		
		/// <summary>
		/// Gets minimap wall image.
		/// </summary>
		/// <param name="index">
		/// Formation of wall.
		/// </param>
		/// <param name="setindex">
		/// Automap type index.
		/// </param>
		/// <returns>
		/// Wall image.
		/// </returns>
		public static RawImage GetWall(int index, int setindex)
		{
			return GameData.AutomapGraphics.Open(setindex).GetWall(index);
		}
		
		/// <summary>
		/// Gets minimap object image.
		/// </summary>
		/// <param name="index">
		/// Object type index.
		/// </param>
		/// <param name="type">
		/// Automap type.
		/// </param>
		/// <returns>
		/// Object image.
		/// </returns>
		public static RawImage GetIcon(int index, MinimapType type)
		{
			return GetIcon(index, (int)type);
		}
		
		
		/// <summary>
		/// Gets minimap object image.
		/// </summary>
		/// <param name="index">
		/// Object type index.
		/// </param>
		/// <param name="setindex">
		/// Automap type index.
		/// </param>
		/// <returns>
		/// Object image.
		/// </returns>
		public static RawImage GetIcon(int index, int setindex)
		{
			return GameData.AutomapGraphics.Open(setindex).GetIcon(index);
		}
	}
}
/*
1x closed door 9
4x merchant 11
6x open door 10
2x tavern 12
1x exit 14

Rathaus 24834
6102 110000100000010

Wohnhaus 29953
7501 111010100000001

Stadttor 25344
6300 110001100000000

Gemischtwaren 23808
5D00 101110100000000

Marktplatz 26114
6602 110011000000010

Waffenschmied 23298
5B02 101101100000010

Hafen 25858
6502 110010100000010

Wohnhaus 29441
7301 111001100000001
*/