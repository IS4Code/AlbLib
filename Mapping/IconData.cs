using System.IO;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	/// <summary>
	/// This class represents static tile data.
	/// </summary>
	public static class IconData
	{
		private static readonly TileData[][] tilesets = new TileData[4096][];
		
		/// <summary>
		/// Returns data array for tileset.
		/// </summary>
		/// <param name="index">
		/// Zero-based tileset index
		/// </param>
		public static TileData[] GetTileset(int index)
		{
			if(tilesets[index] == null)
			{
				int fx = index/100;
				int tx = index%100;
				using(FileStream stream = new FileStream(Paths.IconDataN.Format(fx), FileMode.Open))
				{
					int len = XLDFile.ReadToIndex(stream, tx);
					tilesets[index] = new TileData[len/8];
					for(int i = 0; i < len/8; i++)
					{
						var data = new TileData(i, stream);
						tilesets[index][i] = data;
					}
				}
			}
			return tilesets[index];
		}
		
		/// <summary>
		/// Returns tile data from tileset.
		/// </summary>
		/// <param name="tileset">
		/// Zero-based tileset index
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