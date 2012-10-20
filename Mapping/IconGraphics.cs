using System.IO;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Used for loading tile images.
	/// </summary>
	public static class IconGraphics
	{
		private static readonly IndexedCache<RawImage[]> cache = new IndexedCache<RawImage[]>(LoadTileset, Cache.ZeroNull);
		
		/// <summary>
		/// Loads tileset as an array of RawImages.
		/// </summary>
		/// <param name="index">
		/// Zero-based tileset index.
		/// </param>
		/// <returns>
		/// Array representing the tileset.
		/// </returns>
		public static RawImage[] GetTileset(int index)
		{
			return cache.Get(index);
		}
		
		private static RawImage[] LoadTileset(int index)
		{
			int fx, tx;
			if(!Common.E(index, out fx, out tx))return null;
			using(FileStream stream = new FileStream(Paths.IconGraphicsN.Format(fx), FileMode.Open))
			{
				XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)tx);
				int len = nav.SubfileLength;
				RawImage[] tileset = new RawImage[len/256];
				for(int i = 0; i < len/256; i++)
				{
					tileset[i] = new RawImage(nav, 16, 16);
				}
				return tileset;
			}
		}
		
		public static RawImage GetTileGraphics(int tileset, int grindex)
		{
			return GetTileset(tileset)[grindex];
		}
		
		/// <summary>
		/// Gets tile graphics using tile index.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index.
		/// </param>
		/// <param name="index">
		/// Overlay or underlay.
		/// </param>
		public static RawImage GetTile(int tileset, int index)
		{
			if(index <= 1)return null;
			TileData[] tiledata = IconData.GetTileset(tileset);
			return GetTileGraphics(tileset, tiledata[index-2].GrID);
		}
		
		/// <summary>
		/// Returns image representing underlay portion of tile.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index.
		/// </param>
		/// <param name="tile">
		/// Tile.
		/// </param>
		/// <returns>
		/// Underlay image.
		/// </returns>
		public static RawImage GetTileUnderlay(int tileset, Tile tile)
		{
			return GetTile(tileset, tile.Underlay);
		}
		
		/// <summary>
		/// Returns image representing overlay portion of tile.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index.
		/// </param>
		/// <param name="tile">
		/// Tile.
		/// </param>
		/// <returns>
		/// Overlay image.
		/// </returns>
		public static RawImage GetTileOverlay(int tileset, Tile tile)
		{
			return GetTile(tileset, tile.Overlay);
		}
	}
}