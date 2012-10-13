using System;
using System.IO;
using AlbLib.Mapping;
using AlbLib.XLD;

namespace AlbLib.Imaging
{
	public static class AutoGFX
	{
		private static byte[][] gfx = new byte[2][];
		
		
		public static RawImage GetWall(WallForm form, int setindex)
		{
			return GetWall((int)form, setindex);
		}
		
		public static RawImage GetWall(WallForm form, MinimapType type)
		{
			return GetWall((int)form, (int)type);
		}
		
		public static RawImage GetWall(int index, MinimapType type)
		{
			return GetWall(index, (int)type);
		}
		
		public static RawImage GetWall(int index, int setindex)
		{
			CheckCache(setindex);
			byte[] wall = new byte[64];
			Array.Copy(gfx[setindex], 0x8C00+index*64, wall, 0, 64);
			return new RawImage(wall, 8, 8);
		}
		
		public static RawImage GetMapObject(int index, MinimapType type)
		{
			return GetMapObject(index, (int)type);
		}
		
		public static RawImage GetMapObject(int index, int setindex)
		{
			CheckCache(setindex);
			if(index<= 1)return null;
			if(index<= 8)return GetObj(0x9000+(index-2)*256*4, setindex);
			if(index<=19)return GetObj(0xAC00+(index-9)*256, setindex);
			if(index==19)return GetObj(0xBD00, setindex);
			return null;
		}
		
		private static RawImage GetObj(int offset, int setindex)
		{
			CheckCache(setindex);
			byte[] img = new byte[256];
			Array.Copy(gfx[setindex], offset, img, 0, 256);
			return new RawImage(img, 16, 16);
		}
		
		private static void CheckCache(int setindex)
		{
			if(gfx[setindex] == null)
			{
				using(FileStream stream = new FileStream(Paths.AutoGFXN.Format(0), FileMode.Open))
				{
					XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)setindex);
					gfx[setindex] = new byte[nav.SubfileLength];
					nav.Read(gfx[setindex], 0, nav.SubfileLength);
				}
			}
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