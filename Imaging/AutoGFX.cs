using System;
using System.IO;
using AlbLib.Caching;
using AlbLib.Mapping;
using AlbLib.XLD;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Contains stuff from automap graphics.
	/// </summary>
	public static class AutoGFX
	{
		private static IndexedCache<byte[]> cache = new IndexedCache<byte[]>(LoadCache);
		
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
			byte[] wall = new byte[64];
			Array.Copy(cache[setindex], 0x8C00+index*64, wall, 0, 64);
			return new RawImage(wall, 8, 8);
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
		public static RawImage GetMapObject(int index, MinimapType type)
		{
			return GetMapObject(index, (int)type);
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
		public static RawImage GetMapObject(int index, int setindex)
		{
			if(index<= 1)return null;
			if(index<= 8)return GetObj(0x9000+(index-2)*256*4, setindex);
			if(index<=19)return GetObj(0xAC00+(index-9)*256, setindex);
			if(index==19)return GetObj(0xBD00, setindex);
			return null;
		}
		
		private static RawImage GetObj(int offset, int setindex)
		{
			byte[] img = new byte[256];
			Array.Copy(cache[setindex], offset, img, 0, 256);
			return new RawImage(img, 16, 16);
		}
		
		private static byte[] LoadCache(int setindex)
		{
			using(FileStream stream = new FileStream(Paths.AutoGFXN.Format(0), FileMode.Open))
			{
				XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)setindex);
				byte[] gfx = new byte[nav.SubfileLength];
				nav.Read(gfx, 0, gfx.Length);
				return gfx;
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