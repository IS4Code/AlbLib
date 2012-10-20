using System.IO;
using AlbLib.Caching;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// This class represents static tile data.
	/// </summary>
	public static class IconData
	{
		private static readonly IndexedCache<TileData[]> cache = new IndexedCache<TileData[]>(LoadTileset, Cache.ZeroNull);
		
		/// <summary>
		/// Returns data array for tileset.
		/// </summary>
		/// <param name="index">
		/// One-based tileset index.
		/// </param>
		public static TileData[] GetTileset(int index)
		{
			return cache.Get(index);
		}
		
		private static TileData[] LoadTileset(int index)
		{
			int fx, tx;
			if(!Common.E(index, out fx, out tx))return null;
			
			using(FileStream stream = new FileStream(Paths.IconDataN.Format(fx), FileMode.Open))
			{
				XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)tx);
				int len = nav.SubfileLength;
				TileData[] tileset = new TileData[len/8];
				for(int i = 0; i < len/8; i++)
				{
					tileset[i] = new TileData(i, nav);
				}
				return tileset;
			}
		}
		
		/// <summary>
		/// Returns tile data from tileset.
		/// </summary>
		/// <param name="tileset">
		/// One-based tileset index.
		/// </param>
		/// <param name="index">
		/// Tile index.
		/// </param>
		public static TileData GetTile(int tileset, int index)
		{
			return GetTileset(tileset)[index];
		}
	}
}